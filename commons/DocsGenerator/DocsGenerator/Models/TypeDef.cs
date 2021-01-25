using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsGenerator.Models {
    public class TypeDef {
        public Type Type;
        public string FullName;
        public string Name;
        public string Namespace;

        public TypeDef(Type type) {
            Type = type;
            Name = type.Name;
            Namespace = Type.Namespace;
            FullName = type.FullName;
        }
    }
}
