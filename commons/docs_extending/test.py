from source._ext.csharp import DefinitionReader

reader = DefinitionReader("some.namespace.JObject<MyType1, MyType2> PerformActions(some.namespace.ClientConnection conn, another.namespace.JObject actionsData)")
t = reader.read_next_type()
n = reader.read_next_name()
p = reader.read_next_method_params()

print("OK")