using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DocsGenerator.Models {
    public class ConstructorDef {
        public ClassDef ParentClass = null;
        public AccessType AccessType;

        public string Summary = null;
        public string Remarks = null;

        public List<ParamDef> Params = new List<ParamDef>();
        public List<ThrowDef> Throws = new List<ThrowDef>();

        public ConstructorDef(ClassDef parent) {
            ParentClass = parent;
        }

        public void AddParam(ParamDef def) {
            Params.Add(def);
        }

        public ParamDef GetParam(string paramName) {
            foreach (var param in Params) {
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
            foreach (var t in Throws) {
                if (t.ExceptionTypeDef.Type == exceptionType) {
                    return t;
                }
            }
            return null;
        }

        public static ConstructorDef FromConstructorInfo(ClassDef parentClass, ConstructorInfo info) {
            var def = new ConstructorDef(parentClass);

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


            // Parametry
            var parameters = info.GetParameters();
            foreach (var param in parameters) {
                def.AddParam(ParamDef.FromParamInfo(param));
            }

            return def;
        }
    }
}
