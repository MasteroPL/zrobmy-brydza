from docutils import nodes
from docutils.parsers.rst import Directive, directives

def visit_csharpdocs_generic(self, node):
    self.body.append(self.starttag(node, 'div'))
def depart_csharpdocs_generic(self, node):
    self.body.append("</div>")

#
# Commons
#
class CSharpType:

    def __init__(self, name=None, namespace=None, generics=None):
        self.name = name
        self.namespace = namespace
        self.generics = generics if generics is not None else []


class CSharpParam:

    def __init__(self, name=None, type:CSharpType=None, default:str=None):
        self.name = name
        self.type = type
        self.default = default

class CSharpMethod:

    def __init__(self, name:str=None, return_type:CSharpType=None, params=None):
        self.name = name
        self.return_type = return_type
        self.params = params if params is not None else []

class CSharpConstructor:

    def __init__(self, name:str=None, params=None):
        self.name = name
        self.params = params if params is not None else []

class CSharpException:

    def __init__(self, xtype:CSharpType, description:str=None):
        self.type = xtype
        self.description = description

class DefinitionReader:

    class FormatError(Exception):
        pass

    small_letters = list(map(chr, range(ord('a'), ord('z')+1)))
    big_letters = list(map(chr, range(ord('A'), ord('Z')+1)))
    digits = list(map(chr, range(ord('0'), ord('9')+1)))

    CHARS_WHITESPACE = (
        " ",
        "\t",
        "\n",
        "\r",
        ",",
    )
    CHARS_ALLOWED_NAME_ENDER = (
        *CHARS_WHITESPACE,
        "=",
        "(",
        ")",
    )
    CHARS_ALLOWED_TYPE_ENDER = (
        *CHARS_WHITESPACE,
        ">",
        "}",
    )
    CHARS_ALLOWED_FOR_REGULAR_NAME_FIRST = (
        *small_letters,
        *big_letters,
        "_",
    )
    CHARS_ALLOWED_FOR_REGULAR_NAME = (
        *CHARS_ALLOWED_FOR_REGULAR_NAME_FIRST,
        *digits,
    )
    CHARS_ALLOWED_FOR_NAMESPACE = (
        *CHARS_ALLOWED_FOR_REGULAR_NAME,
        ".",
    )

    def __init__(self, source_text:str):
        self.source_text = source_text
        self.pointer = 0
        self.text_length = len(source_text)

    def read_generic(self):
        index = self.pointer

        while self.text_length > index and self.source_text[index] in self.CHARS_WHITESPACE:
            index += 1

        if self.text_length <= index:
            self.pointer = index
            return None

        if self.source_text[index] != "<":
            raise self.FormatError("Generic must start with a '<'!")

        index += 1
        generics = []

        while self.text_length > index and self.source_text[index] != ">":
            self.pointer = index
            cur_type = self.read_next_type()
            generics.append(cur_type)
            index = self.pointer

            while self.text_length > index and self.source_text[index] in self.CHARS_WHITESPACE:
                index += 1

        if self.source_text[index] == ">":
            index += 1

        self.pointer = index

        return generics


    def read_next_type(self):
        index = self.pointer
        result = CSharpType()

        while self.text_length > index and self.source_text[index] in self.CHARS_WHITESPACE:
            index += 1

        if self.text_length > index and self.source_text[index] not in self.CHARS_ALLOWED_FOR_REGULAR_NAME_FIRST:
            raise self.FormatError("Invalid first character of type")

        if self.text_length <= index:
            self.pointer = index
            return None

        start_index = index

        while self.text_length > index and self.source_text[index] in self.CHARS_ALLOWED_FOR_NAMESPACE:
            index+=1

        end_index = index
        self.pointer = index

        full_result = self.source_text[start_index:end_index]
        parts = full_result.split(".")

        try:
            generics = self.read_generic()
            result.generics = generics
        except self.FormatError:
            pass

        index = self.pointer
        if self.text_length > index and self.source_text[index] not in self.CHARS_ALLOWED_TYPE_ENDER:
            raise self.FormatError("Invalid last character (not whitespace, neither allowed for type)")

        if len(parts) == 1:
            result.name = parts[0]
            result.namespace = None
            return result
        else:
            result.name = parts[-1]
            result.namespace = ".".join(parts[0:-1])
            return result


    def read_next_name(self):
        index = self.pointer

        while self.text_length > index and self.source_text[index] in self.CHARS_WHITESPACE:
            index += 1

        if self.text_length > index and self.source_text[index] not in self.CHARS_ALLOWED_FOR_REGULAR_NAME_FIRST:
            raise self.FormatError("Invalid first character of name")

        if self.text_length <= index:
            self.pointer = index
            return None

        start_index = index

        while self.text_length > index and self.source_text[index] in self.CHARS_ALLOWED_FOR_REGULAR_NAME:
            index+=1

        if self.text_length > index and self.source_text[index] not in self.CHARS_ALLOWED_NAME_ENDER:
            raise self.FormatError("Invalid last character (not whitespace, neither allowed for name)")

        end_index = index
        self.pointer = index
        full_result = self.source_text[start_index:end_index]

        return full_result

    
    def read_next_method_params(self):
        index = self.pointer

        while self.text_length > index and self.source_text[index] in self.CHARS_WHITESPACE:
            index += 1

        if self.text_length > index and self.source_text[index] != "(":
            raise self.FormatError("Invalid first character of method params (has to be '(')")

        index += 1

        if self.text_length <= index:
            self.pointer = index
            return None

        start_index = index

        params = []

        while(self.text_length > index and self.source_text[index] != ")"):
            self.pointer = index
            cur_type = self.read_next_type()
            cur_name = self.read_next_name()
            cur_default = None
            index = self.pointer

            if self.source_text[index] == "=":
                index += 1
                istart = index

                while self.text_length > index and self.source_text[index] != ")" and self.source_text[index] != ",":
                    index += 1
                
                iend = index

                cur_default = self.source_text[istart:iend]

            params.append(CSharpParam(name=cur_name, type=cur_type, default=cur_default))

        index += 1
        self.pointer = index
        return params


    def read_next_method(self):
        t = self.read_next_type()
        n = self.read_next_name()
        p = self.read_next_method_params()

        return CSharpMethod(name = n, return_type=t, params=p)

    def read_next_constructor(self):
        n = self.read_next_name()
        p = self.read_next_method_params()

        return CSharpConstructor(name=n, params=p)

    def read_next_exception(self):
        index = self.pointer

        while self.text_length > index and self.source_text[index] in self.CHARS_WHITESPACE:
            index += 1

        if self.text_length > index and self.source_text[index] != "{":
            raise self.FormatError("Invalid first character of throws (has to be '{')")

        index += 1

        if self.text_length <= index:
            self.pointer = index
            return None

        start_index = index
        self.pointer = index

        xtype = self.read_next_type()

        index = self.pointer

        if self.text_length > index and self.source_text[index] != "}":
            raise self.FormatError("Invalid last character (has to be '}')")

        index += 1
        description = ""
        if index+1 < self.text_length:
            description = self.source_text[index:]

        return CSharpException(xtype, description=description)


# 
# Class Directive
#

class csharpdocs_class_node(nodes.Structural, nodes.Element):
    pass

def visit_csharpdocs_class_node(self, node):
    self.body.append(self.starttag(node, 'div'))
def depart_csharpdocs_class_node(self, node):
    self.body.append("</div>")


class csharpdocs_class_definition_node(nodes.Structural, nodes.Element):
    pass

def visit_csharpdocs_class_definition_node(self, node):
    self.body.append(self.starttag(node, 'div'))
def depart_csharpdocs_class_definition_node(self, node):
    self.body.append("</div>")



class ClassDirective(Directive):
    required_arguments = 0
    optional_arguments = 0
    final_argument_whitespace = True
    option_spec = {
        "access": directives.unchanged,
        "baseclass": directives.unchanged,
    }

    has_content = True
    add_index = True

    def _append_type(self, parent_node, xtype:CSharpType):
        if xtype.namespace is not None:
            parent_node += nodes.inline(text=xtype.namespace + ".", classes=["csharpdocs-type-hidden"])
        parent_node += nodes.inline(text=xtype.name, classes=["csharpdocs-type"])

        if xtype.generics is not None:
            if len(xtype.generics) > 0:
                parent_node += nodes.inline(text="<", classes=["csharpdocs-generic-symbol"])

                first = True
                for generic in xtype.generics:
                    if not first:
                        parent_node += nodes.inline(text=",", classes=["csharpdocs-separator"])

                    self._append_type(parent_node, generic)
                    first = False

                parent_node += nodes.inline(text=">", classes=["csharpdocs-generic-symbol"])

    def _append_name(self, parent_node, name:str, default:str=None, classes=None):
        if classes is None:
            classes=["csharpdocs-name"]
        parent_node += nodes.inline(text=name, classes=classes)

        if default is not None:
            parent_node += nodes.inline(text="=", classes=["csharpdocs-default-value-sign"])
            parent_node += nodes.inline(text=default, classes=["csharpdocs-default-value"])


    def run(self):
        sett = self.state.document.settings
        language_code = sett.language_code
        env = self.state.document.settings.env
        config = env.config

        options = self.options

        idb = nodes.make_id("csharpdocs-class-" + self.content[0].replace(" ", "_"))
        node = csharpdocs_class_node(ids=[idb], classes=["csharpdocs-class-node"])

        def_node = csharpdocs_class_definition_node(classes=["csharpdocs-class-definition-node"])

        reader = DefinitionReader(self.content[0])
        class_type = reader.read_next_type()
        if options.__contains__("baseclass"):
            reader = DefinitionReader(options["baseclass"])
            baseclass_type = reader.read_next_type()
        else:
            baseclass_type = None

        if class_type.namespace is not None:
            def_node += nodes.paragraph(text=class_type.namespace, classes=["csharpdocs-class-definition-namespace"])

        # Class node
        class_node = nodes.paragraph(classes=["csharpdocs-class-definition-class"])

        access = options["access"]
        if access is not None and access != "":
            class_node += nodes.inline(text=access + " ", classes=["csharpdocs-class-definition-access"])

        class_node += nodes.inline(text="class ", classes=["csharpdocs-class-definition-class-text"])
        
        class_type_node = nodes.inline(classes=["csharpdocs-class-definition-class-type-node"])
        self._append_type(class_type_node, class_type)
        class_node += class_type_node

        # Base class node
        if baseclass_type != None:
            class_node += nodes.inline(
                text=" : ",
            )

            self._append_type(class_node, baseclass_type)

        def_node += class_node

        summary = "\n".join(self.content[1:])
        par = nodes.paragraph(text=summary, classes=["csharpdocs-class-summary"])

        node += def_node
        node += par

        return [ node, ]


#
# Method directive
#
class csharpdocs_method_node(nodes.Structural, nodes.Element):
    pass

class csharpdocs_method_definition_node(nodes.Structural, nodes.Element):
    pass


class MethodDirective(Directive):
    required_arguments = 0
    optional_arguments = 0
    final_argument_whitespace = True
    option_spec = {
        "returns": directives.unchanged,
        "access": directives.unchanged,
        **dict(zip([('param(' + str(i) + ')') for i in range(1, 20)], [directives.unchanged] * 20)),
        **dict(zip([('throws(' + str(i) + ')') for i in range(1, 20)], [directives.unchanged] * 20)),
    }

    has_content = True
    add_index = True

    def _append_type(self, parent_node, xtype:CSharpType):
        if xtype.namespace is not None:
            parent_node += nodes.inline(text=xtype.namespace + ".", classes=["csharpdocs-type-hidden"])
        parent_node += nodes.inline(text=xtype.name, classes=["csharpdocs-type"])

        if len(xtype.generics) > 0:
            parent_node += nodes.inline(text="<", classes=["csharpdocs-generic-symbol"])

            first = True
            for generic in xtype.generics:
                if not first:
                    parent_node += nodes.inline(text=",", classes=["csharpdocs-separator"])

                self._append_type(parent_node, generic)
                first = False

            parent_node += nodes.inline(text=">", classes=["csharpdocs-generic-symbol"])

    def _append_name(self, parent_node, name:str, default:str=None, classes=None):
        if classes is None:
            classes=["csharpdocs-name"]
        parent_node += nodes.inline(text=name, classes=classes)

        if default is not None:
            parent_node += nodes.inline(text="=", classes=["csharpdocs-default-value-sign"])
            parent_node += nodes.inline(text=default, classes=["csharpdocs-default-value"])

    def _append_method_params(self, parent_node, params):
        parent_node += nodes.inline(text="(", classes=["csharpdocs-method-params-symbol"])

        first = True
        for param in params:
            if not first:
                parent_node += nodes.inline(text=", ", classes=["csharpdocs-separator"])

            self._append_type(parent_node, param.type)
            parent_node += nodes.inline(text=" ")
            self._append_name(parent_node, param.name, param.default)
            first = False

        parent_node += nodes.inline(text=")", classes=["csharpdocs-method-params-symbol"])

        

    def run(self):
        sett = self.state.document.settings
        language_code = sett.language_code
        env = self.state.document.settings.env
        config = env.config

        options = self.options

        idb = nodes.make_id("csharpdocs-method-" + self.content[0].replace(" ", "_"))
        node = csharpdocs_method_node(ids=[idb], classes=["csharpdocs-method-node"])

        def_node = csharpdocs_method_definition_node(classes=["csharpdocs-method-definition-node"])

        # Method node
        method_node = nodes.paragraph(classes=["csharpdocs-method-definition-method"])

        if options["access"] != "":
            method_node += nodes.inline(text=options["access"] + " ", classes=["csharpdocs-method-definition-access"])

        reader = DefinitionReader(self.content[0])
        method_obj = reader.read_next_method()

        # return type
        self._append_type(method_node, method_obj.return_type)
        method_node += nodes.inline(text=" ")
        self._append_name(method_node, method_obj.name, classes=["csharpdocs-method-name"])
        self._append_method_params(method_node, method_obj.params)

        def_node += method_node
        node += def_node

        node += nodes.paragraph(text="\n".join(self.content[1:]), classes=["csharpdocs-method-description"])

        params_node = nodes.bullet_list(classes=["csharpdocs-method-params-ul"])
        for i in range(len(method_obj.params)):
            item_node = nodes.list_item()
            item_node += nodes.inline(text=method_obj.params[i].name + ": ", classes=["csharpdocs-method-param-name"])

            #desc_lines = options["param(" + str(i + 1) + ")"].split("\n")

            item_node += nodes.inline(text=options["param(" + str(i + 1) + ")"], classes=["csharpdocs-method-param-description"])
            params_node += item_node

        node += params_node

        returns_node = nodes.bullet_list(classes=["csharpdocs-method-returns-ul"])
        item_node = nodes.list_item()
        item_node += nodes.inline(text="Zwraca: ", classes=["csharpdocs-method-returns"])
        item_node += nodes.inline(text=options["returns"], classes=["csharpdocs-method-returns-description"])
        returns_node += item_node
        node += returns_node

        exceptions_node = nodes.bullet_list(classes=["csharpdocs-method-throws-ul"])
        index = 1
        while options.__contains__("throws(" + str(index) + ")"):
            exception_node = nodes.list_item()
            reader = DefinitionReader(options["throws(" + str(index) + ")"])
            ex = reader.read_next_exception()

            type_node = nodes.inline(classes=["csharpdocs-type-node", "csharpdocs-method-exception"])
            self._append_type(type_node, ex.type)

            type_node += nodes.inline(text=": ", classes=["csharpdocs-type"])

            exception_node += type_node
            exception_node += nodes.inline(text=ex.description, classes=["csharpdocs-method-exception-description"])

            exceptions_node += exception_node
            index += 1

        node += exceptions_node


        return [node]

class ConstructorDirective(MethodDirective):

    def run(self):
        sett = self.state.document.settings
        language_code = sett.language_code
        env = self.state.document.settings.env
        config = env.config

        options = self.options

        idb = nodes.make_id("csharpdocs-constructor-" + self.content[0].replace(" ", "_"))
        node = csharpdocs_method_node(ids=[idb], classes=["csharpdocs-constructor-node"])

        def_node = csharpdocs_method_definition_node(classes=["csharpdocs-method-definition-node"])

        # Method node
        method_node = nodes.paragraph(classes=["csharpdocs-method-definition-method"])

        if options["access"] != "":
            method_node += nodes.inline(text=options["access"] + " ", classes=["csharpdocs-method-definition-access"])

        reader = DefinitionReader(self.content[0])
        method_obj = reader.read_next_constructor()

        # return type
        self._append_name(method_node, method_obj.name, classes=["csharpdocs-method-name"])
        self._append_method_params(method_node, method_obj.params)

        def_node += method_node
        node += def_node

        node += nodes.paragraph(text="\n".join(self.content[1:]), classes=["csharpdocs-method-description"])

        params_node = nodes.bullet_list(classes=["csharpdocs-method-params-ul"])
        for i in range(len(method_obj.params)):
            item_node = nodes.list_item()
            item_node += nodes.inline(text=method_obj.params[i].name + ": ", classes=["csharpdocs-method-param-name"])

            #desc_lines = options["param(" + str(i + 1) + ")"].split("\n")

            item_node += nodes.inline(text=options["param(" + str(i + 1) + ")"], classes=["csharpdocs-method-param-description"])
            params_node += item_node
        node += params_node

        exceptions_node = nodes.bullet_list(classes=["csharpdocs-method-throws-ul"])
        index = 1
        while options.__contains__("throws(" + str(index) + ")"):
            exception_node = nodes.list_item()
            reader = DefinitionReader(options["throws(" + str(index) + ")"])
            ex = reader.read_next_exception()

            type_node = nodes.inline(classes=["csharpdocs-type-node", "csharpdocs-method-exception"])
            self._append_type(type_node, ex.type)

            type_node += nodes.inline(text=": ", classes=["csharpdocs-type"])

            exception_node += type_node
            exception_node += nodes.inline(text=ex.description, classes=["csharpdocs-method-exception-description"])

            exceptions_node += exception_node
            index += 1

        node += exceptions_node


        return [node]

class csharpdocs_property_node(nodes.Structural, nodes.Element):
    pass

class csharpdocs_property_definition_node(nodes.Structural, nodes.Element):
    pass

class PropertyDirective(Directive):
    required_arguments = 0
    optional_arguments = 0
    final_argument_whitespace = True
    option_spec = {
        "returns": directives.unchanged,
        "access": directives.unchanged,
        **dict(zip([('param(' + str(i) + ')') for i in range(1, 20)], [directives.unchanged] * 20)),
        **dict(zip([('throws(' + str(i) + ')') for i in range(1, 20)], [directives.unchanged] * 20)),
    }

    has_content = True
    add_index = True

    def _append_type(self, parent_node, xtype:CSharpType):
        if xtype.namespace is not None:
            parent_node += nodes.inline(text=xtype.namespace + ".", classes=["csharpdocs-type-hidden"])
        parent_node += nodes.inline(text=xtype.name, classes=["csharpdocs-type"])

        if len(xtype.generics) > 0:
            parent_node += nodes.inline(text="<", classes=["csharpdocs-generic-symbol"])

            first = True
            for generic in xtype.generics:
                if not first:
                    parent_node += nodes.inline(text=",", classes=["csharpdocs-separator"])

                self._append_type(parent_node, generic)
                first = False

            parent_node += nodes.inline(text=">", classes=["csharpdocs-generic-symbol"])

    def _append_name(self, parent_node, name:str, default:str=None, classes=None):
        if classes is None:
            classes=["csharpdocs-name"]
        parent_node += nodes.inline(text=name, classes=classes)

        if default is not None:
            parent_node += nodes.inline(text="=", classes=["csharpdocs-default-value-sign"])
            parent_node += nodes.inline(text=default, classes=["csharpdocs-default-value"])

    def run(self):
        sett = self.state.document.settings
        language_code = sett.language_code
        env = self.state.document.settings.env
        config = env.config

        options = self.options

        idb = nodes.make_id("csharpdocs-class-" + self.content[0].replace(" ", "_"))
        node = csharpdocs_class_node(ids=[idb], classes=["csharpdocs-class-node"])

        node = csharpdocs_method_node(ids=[idb], classes=["csharpdocs-property-node"])

        def_node = csharpdocs_property_definition_node(classes=["csharpdocs-property-definition-node"])

        property_node = nodes.paragraph(classes=["csharpdocs-property-definition-property"])

        if options["access"] != "":
            property_node += nodes.inline(text=options["access"] + " ", classes=["csharpdocs-property-definition-access"])

        reader = DefinitionReader(self.content[0])
        xtype = reader.read_next_type()
        name = reader.read_next_name()

        self._append_type(property_node, xtype)
        property_node += nodes.inline(text=" ")
        self._append_name(property_node, name)

        def_node += property_node
        node += def_node

        node += nodes.paragraph(text="\n".join(self.content[1:]), classes=["csharpdocs-property-description"])

        return [node]



def setup(app):
    # Class
    app.add_node(
        csharpdocs_class_node, 
        html=(
            visit_csharpdocs_class_node,
            depart_csharpdocs_class_node
        )
    )
    app.add_node(
        csharpdocs_class_definition_node,
        html=(
            visit_csharpdocs_class_definition_node,
            depart_csharpdocs_class_definition_node
        )
    )
    app.add_directive("csharpdocsclass", ClassDirective)

    # Method
    app.add_node(
        csharpdocs_method_node,
        html=(
            visit_csharpdocs_generic,
            depart_csharpdocs_generic
        )
    )
    app.add_node(
        csharpdocs_method_definition_node,
        html=(
            visit_csharpdocs_generic,
            depart_csharpdocs_generic
        )
    )
    app.add_directive("csharpdocsmethod", MethodDirective)
    app.add_directive("csharpdocsconstructor", ConstructorDirective)

    app.add_node(
        csharpdocs_property_node,
        html=(
            visit_csharpdocs_generic,
            depart_csharpdocs_generic
        )
    )
    app.add_node(
        csharpdocs_property_definition_node,
        html=(
            visit_csharpdocs_generic,
            depart_csharpdocs_generic
        )
    )
    app.add_directive("csharpdocsproperty", PropertyDirective)

    return {
        "version": "0.1",
        "parallel_read_safe": True,
        "parallel_write_safe": True
    }