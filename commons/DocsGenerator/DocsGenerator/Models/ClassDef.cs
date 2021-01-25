using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DocsGenerator.Models {
    public class ClassDef {
        public TypeDef BaseClass = null;
        public bool Nested = false;
        public AccessType AccessType;
        public TypeDef TypeDef;
        public string Summary = null;
        public string Remarks = null;

        public Dictionary<ConstructorInfo, ConstructorDef> Constructors = new Dictionary<ConstructorInfo, ConstructorDef>();
        public Dictionary<MethodInfo, MethodDef> Methods = new Dictionary<MethodInfo, MethodDef>();

        public static ClassDef FromType(Type type) {
            var def = new ClassDef();
            def.TypeDef = new TypeDef(type);

            if (type.BaseType != null) {
                def.BaseClass = new TypeDef(type.BaseType);
            }

            // Określenie typu dostępu
            if (type.IsPublic) {
                def.AccessType = AccessType.PUBLIC;
            }
            else if (type.IsNestedPublic) {
                def.AccessType = AccessType.PUBLIC;
                def.Nested = true;
            }
            else if (type.IsNestedFamily) {
                def.AccessType = AccessType.PROTECTED;
                def.Nested = true;
            }
            else if (type.IsNestedPrivate) {
                def.AccessType = AccessType.PRIVATE;
                def.Nested = true;
            }
            else {
                def.AccessType = AccessType.INNER;
            }

            // Sczytywanie metod klasy
            var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            var constructors = type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            MethodDef mdef;
            foreach(var method in methods) {
                mdef = MethodDef.FromMethodInfo(def, method);
                def.Methods.Add(method, mdef);
            }
            ConstructorDef cdef;
            foreach(var constr in constructors) {
                cdef = ConstructorDef.FromConstructorInfo(def, constr);
                def.Constructors.Add(constr, cdef);
            }

            return def;
        }
    }
}
