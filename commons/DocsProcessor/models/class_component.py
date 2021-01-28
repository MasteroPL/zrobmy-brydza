from abc import ABC, abstractmethod

class ClassComponentDef(ABC):

    def get_class_fullname(self):
        return self.namespace

    def get_class_name_and_namespace(self):
        ns_parts = self.namespace.split(".")
        name = ns_parts.pop(len(ns_parts) - 1)
        namespace = '.'.join(ns_parts)
        return name, namespace