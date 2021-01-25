import xml.etree.ElementTree as ET
from models.assembly import AssemblyDef
from models.class import ClassDef
from models.param import ParamDef
from models.method import MethodDef
from models.event import EventDef
from models.field import FieldDef
from models.property import PropertyDef

class XMLDocs:

    @staticmethod
    def process_xml_file(xml_file_path):
        tree = ET.parse(xml_file_path)
        root = tree.getroot()

        assembly = AssemblyDef()

        for child in root:
            if child.tag == "assembly":
                assembly.name = XMLDocs.process_assembly_data(child)

            elif child.tag == "members":
                classes = XMLDocs.process_members(child)

        print("OK")


    @staticmethod
    def process_assembly_data(assembly_root):
        name = assembly_root.get("name")

        return name

    @staticmethod
    def process_members(members_root):
        classes = {}

        for member_root in members_root:
            member_name = member_root.attrib["name"]
            member_type, member_def = XMLDocs.process_member_name(member_name)

            if member_type == "T":
                # TODO procesowanie docsów klasy
                classes[member_def.fullname] = member_def

            else:
                class_full_name = member_def.get_class_fullname()
                
                # Jeśli nie ma w dokumentacji definicji dla klasy, generujemy domyślną
                if not classes.__contains__(class_full_name):
                    class_name, class_namespace = member_def.get_class_name_and_namespace()

                    class_def = ClassDef(
                        fullname=class_full_name,
                        name=class_name,
                        namespace=class_namespace
                    )

                    classes[class_full_name] = class_def

                # W przeciwnym wypadku aktualizujemy istniejącą klasę
                else:
                    class_def = classes[class_full_name]

                if member_type == "M":
                    # TODO procesowanie docsów membera

                    class_def.methods.append(member_def)

                elif member_type == "F":
                    # TODO procesowanie pola

                    class_def.fields.append(member_def)

                elif member_type == "P":
                    # TODO procesowanie własności

                    class_def.properties.append(member_def)


                elif member_type == "E":
                    # TODO procesowanie eventu

                    class_def.events.append(member_def)


    #
    # Sekcja przetwarzania nazwy komponentu
    #


    @staticmethod
    def process_member_name(name:str):
        mtype = name[0]
        fullname = name[2:]

        if mtype == "T":
            result = XMLDocs.process_member_name_generic(fullname, ClassDef)
            return "T", result
        elif mtype == "M":
            result = XMLDocs.process_member_name_method(fullname)
            return "M", result
        elif mtype == "E":
            result = XMLDocs.process_member_name_generic(fullname, EventDef)
            return "E", result
        elif mtype == "F":
            result = XMLDocs.process_member_name_generic(fullname, FieldDef)
            return "F", result
        elif mtype == "P":
            result = XMLDocs.process_member_name_generic(fullname, PropertyDef)
            return "P", result
        else:
            raise NotImplemented()
        

    @staticmethod
    def process_member_name_generic(name:str, class_type:type):
        mfullname = name
        mfullname_parts = mfullname.split(".")
        mname = mfullname_parts.pop(len(mfullname_parts) - 1)
        mnamespace = '.'.join(mfullname_parts)

        return class_type(
            fullname=mfullname,
            name=mname,
            namespace=mnamespace
        )


    @staticmethod
    def process_member_name_method(name:str):
        # Podział na nazwę metody i parametry metody
        parts = name.split("(")

        mfullname = parts[0]
        mparams_str = parts[1][0:-1]

        # Przetwarzanie nazwy metody
        if mname[-5:] == "#cotr":
            mfullname = mfullname[2:-6]
            mtype = "#COTR"
        else:
            mfullname = mfullname[2:]
            mtype = "M"

        mfullname_parts = mfullname.split(".")
        mparams_strs = mparams_str.split(",")

        mname = mfullname_parts.pop(len(mname_parts) - 1)
        mnamespace = '.'.join(mfullname_parts)

        mmethod = MethodDef(
            type=mtype,
            fullname=mfullname,
            name=mname,
            namespace=mnamespace
        )

        mparams = []
        for mparam_str in mparams_strs:
            mparam_type_fullname = mparam_str
            mparam_type_parts = mparam_type_fullname.split(".")
            mparam_type_name = mparam_type_parts.pop(len(mparam_parts) - 1)
            mparam_type_namespace = '.'.join(mparam_type_parts)

            mparam = ParamDef(
                type_name=mparam_type_name,
                type_fullname=mparam_type_fullname,
                type_namespace=mparam_type_namespace
            )

            mparams.append(mparam)

        mmethod.params = mparams

        return mmethod


