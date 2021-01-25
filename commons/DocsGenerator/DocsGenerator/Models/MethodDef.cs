using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DocsGenerator.Models {
    public class MethodDef {
        public ClassDef ParentClass = null;
        public AccessType AccessType;
        public string Name = null;
        public bool IsConstructor = false;
        public bool IsStatic = false;

        public string Summary = null;
        public string Remarks = null;

        public List<ParamDef> Params = new List<ParamDef>();
        public ReturnsDef Returns;
        public List<ThrowDef> Throws = new List<ThrowDef>();

        public MethodDef(ClassDef parent) {
            ParentClass = parent;
        }

        public void AddParam(ParamDef def) {
            Params.Add(def);
        }

        public ParamDef GetParam(string paramName) {
            foreach(var param in Params) {
                if (param.Name == paramName) {
                    return param;
                }
            }
            return null;
        }

        public void AddThrow(ThrowDef def) {
            Throws.Add(def);
        }

        public ThrowDef GetThrow(Type exceptionType) {
            foreach(var t in Throws) {
                if(t.ExceptionTypeDef.Type == exceptionType) {
                    return t;
                }
            }
            return null;
        }

        public static MethodDef FromMethodInfo(ClassDef parentClass, MethodInfo info) {
            var def = new MethodDef(parentClass);

            // Sczytywanie nazwy
            def.Name = info.Name;
            def.IsConstructor = info.IsConstructor;

            // Określanie poziomu dostępu
            if (info.IsPublic) {
                def.AccessType = AccessType.PUBLIC;
            }
            else if (info.IsFamily) {
                def.AccessType = AccessType.PROTECTED;
            }
            else if (info.IsPrivate) {
                def.AccessType = AccessType.PRIVATE;
            }
            else {
                def.AccessType = AccessType.INNER;
            }

            def.IsStatic = info.IsStatic;

            // Typ zwracany
            if (!def.IsConstructor) {
                def.Returns = new ReturnsDef() {
                    TypeDef = new TypeDef(info.ReturnType),
                    Description = null
                };
            }


            // Parametry
            var parameters = info.GetParameters();
            foreach(var param in parameters){
                def.AddParam(ParamDef.FromParamInfo(param));
            }

            return def;
        }
    }
}
