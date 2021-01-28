from models.class_component import ClassComponentDef

class PropertyDef(ClassComponentDef):

    def __init__(self,
        fullname:str=None,
        name:str=None,
        namespace:str=None,
        summary:str=None,
        remarks:str=None,
        value:str=None
    ):
        self.fullname = fullname
        self.name = name
        self.namespace = namespace
        self.summary = summary
        self.remarks = remarks
        self.value = value