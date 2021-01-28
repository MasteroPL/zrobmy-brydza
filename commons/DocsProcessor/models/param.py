

class ParamDef:

    def __init__(self,
        name:str=None,
        type_fullname:str=None,
        type_name:str=None,
        type_namespace:str=None,
        description:str=None
    ):
        self.name = name
        self.type_fullname = type_fullname
        self.type_name = type_name
        self.type_namespace = type_namespace
        self.description = description