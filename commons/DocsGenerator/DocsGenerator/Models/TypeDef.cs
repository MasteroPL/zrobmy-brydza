using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsGenerator.Models {
    public class TypeDef {
        protected static Dictionary<int, List<TypeDef>> DefinedGenericTypes = new Dictionary<int, List<TypeDef>>();

        public Type Type;
        public string FullName;
        public string Name;
        public string Namespace;

        public TypeDef(Type type) {
            if (type.IsGenericType) {
                int id = Int32.Parse(type.Name.Split('`')[1]);

                if (!DefinedGenericTypes.ContainsKey(id)) {
                    var definition = new List<TypeDef>();
                    DefinedGenericTypes.Add(id, definition);

                    foreach(var genArg in type.GenericTypeArguments) {
                        definition.Add(new TypeDef(genArg));
                    }
                }
            }

            Type = type;
            Name = type.Name;
            Namespace = Type.Namespace;
            FullName = type.FullName;
        }

        public string GetXMLName() {
            if (!Type.IsGenericType) {
                return FullName;
            }
            else {
                string[] parts = Type.FullName.Split('`');
                string p1 = parts[0];
                int id = Int32.Parse(parts[1].Split('[')[0]);

                StringBuilder keyB = new StringBuilder();
                keyB.Append(p1);
                keyB.Append("{");
                bool first = true;
                foreach(var def in DefinedGenericTypes[id]) {
                    if (!first) {
                        keyB.Append(",");
                    }
                    keyB.Append(def.GetXMLName());
                }
                keyB.Append("}");
                return keyB.ToString();
            }
        }

        public string GetGenericName(bool fullNames = false) {
            string[] parts = Name.Split('`');
            string p1;
            if (fullNames)
                p1 = FullName.Split('`')[0];
            else
                p1 = parts[0];
            
            int id = Int32.Parse(parts[1]);

            var defs = DefinedGenericTypes[id];

            StringBuilder bp2 = new StringBuilder();
            bp2.Append("<");
            bool first = true;
            foreach (var def in defs) {
                if (!first) {
                    bp2.Append(", ");
                }

                if (fullNames) {
                    bp2.Append(def.GetDocFullName());
                }
                else {
                    bp2.Append(def.GetDocName());
                }

                first = false;
            }
            bp2.Append(">");

            return p1 + bp2.ToString();
        }

        public string GetDocName() {
            if (Type.IsGenericType) {
                return GetGenericName();
            }
            else {
                return Name;
            }
        }

        public string GetDocFullName() {
            if (Type.IsGenericType) {
                return GetGenericName(true);
            }
            else {
                return FullName;
            }
        }
    }
}
