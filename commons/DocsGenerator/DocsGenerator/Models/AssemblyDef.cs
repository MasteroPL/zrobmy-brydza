using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsGenerator.Models {
    public class AssemblyDef {
        public string AssemblyName = null;
        public Dictionary<Type, ClassDef> ClassDefs = new Dictionary<Type, ClassDef>();

        public AssemblyDef() {
            
        }

        public void AddClassDef(ClassDef def) {
            ClassDefs.Add(def.TypeDef.Type, def);
        }
    }
}
