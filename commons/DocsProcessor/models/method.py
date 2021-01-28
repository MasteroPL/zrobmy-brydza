from models.class_component import ClassComponentDef

class MethodDef(ClassComponentDef):

    def __init__(self,
        type:str="M",
        fullname:str=None,
        name:str=None,
        namespace:str=None,
        summary:str=None,
        remarks:str=None,
        params=None,
        returns=None,
        throws=None
    ):
        self.fullname = fullname
        self.name = name
        self.namespace = namespace
        self.summary = summary
        self.remarks = remarks
        self.params = params
        self.returns = returns
        self.throws = throws

        if self.params is None:
            self.params = []
        if self.throws is None:
            self.throws = []
