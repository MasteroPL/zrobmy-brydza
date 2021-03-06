from source._ext.csharp import DefinitionReader

reader = DefinitionReader("some.namespace.JObject<MyType1<this.one.has.namespace.MyType3>, MyType2>")
result = reader.read_next_type()

print(result.name)