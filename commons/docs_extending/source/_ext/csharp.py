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

        while self.text_length > index and self.source_text[index] not in self.CHARS_ALLOWED_FOR_REGULAR_NAME_FIRST:
            index+=1

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

        if len(parts) == 1:
            result.name = parts[0]
            result.namespace = None
            return result
        else:
            result.name = parts[-1]
            result.namespace = ".".join(parts[0:-1])
            return result


            


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
        "namespace": directives.unchanged,
        "baseclassname": directives.unchanged,
        "baseclassnamespace": directives.unchanged,
    }

    has_content = True
    add_index = True

    def run(self):
        sett = self.state.document.settings
        language_code = sett.language_code
        env = self.state.document.settings.env
        config = env.config

        options = self.options

        namespace = options["namespace"]

        idb = nodes.make_id("csharpdocs-class-" + namespace + "-" + self.name)
        node = csharpdocs_class_node(ids=[idb], classes=["csharpdocs-class-node"])

        def_node = csharpdocs_class_definition_node(classes=["csharpdocs-class-definition-node"])

        if namespace != "":
            def_node += nodes.paragraph(text=namespace, classes=["csharpdocs-class-definition-namespace"])

        # Class node
        class_node = nodes.paragraph(classes=["csharpdocs-class-definition-class"])

        access = options["access"]
        if access is not None and access != "":
            class_node += nodes.inline(text=access + " ", classes=["csharpdocs-class-definition-access"])

        class_node += nodes.inline(text="class ", classes=["csharpdocs-class-definition-class-text"])
        class_node += nodes.inline(text=self.content[0], classes=["csharpdocs-class-definition-classname"])

        # Base class node
        base_class = options["baseclassname"]
        base_class_namespace = options["baseclassnamespace"]
        if base_class != "" and base_class != None:
            class_node += nodes.inline(
                text=" : ",
            )

            base_class_node = nodes.inline(classes=["csharpdocs-class-definition-base-class"])

            if base_class_namespace != "" and base_class_namespace is not None:
                base_class_node += nodes.inline(
                    text=base_class_namespace + ".",
                    classes=["csharpdocs-class-definition-base-class-full-namespace"]
                )
                base_class_node += nodes.inline(
                    text=base_class,
                    classes=["csharpdocs-class-definition-base-class-short-name"]
                )

            class_node += base_class_node

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

    def _process_method_definition(self, method_node, method_definition):
        ret_type_start = 0
        ret_type_end = 0
        name_start = 0
        name_end = 0
        args_start = 0
        args_end = 0
        

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
        method_node = nodes.paragraph(classes=["csharpdocs-method-definiton-method"])

        if options["access"] != "":
            method_node += nodes.inline(text=access + " ", classes=["csharpdocs-method-definition-access"])

        #method_node += nodes.inline(text=self.content[0], classes)


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

    return {
        "version": "0.1",
        "parallel_read_safe": True,
        "parallel_write_safe": True
    }