
class ClassDef:

    def __init__(self, 
        fullname:str=None,
        name:str=None, 
        namespace:str=None, 
        summary:str=None,
        remarks:str=None,
        events=None,
        fields=None,
        properties=None,
        methods=None
    ):
        self.fullname = fullname
        self.name = name
        self.namespace = namespace
        self.summary = summary
        self.remarks = remarks
        self.events = events
        self.fields = fields
        self.properties = properties
        self.methods = methods

        if self.events is None:
            self.events = []
        if self.fields is None:
            self.fields = []
        if self.properties is None:
            self.properties = []
        if self.methods is None:
            self.methods = []