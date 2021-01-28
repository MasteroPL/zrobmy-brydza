using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DocsGenerator.Models {
    public class ParamDef {
        public TypeDef TypeDef;
        public string Name = null;
        public string Description = null;
        public bool HasDefault = false;
        public string DefaultValue = null;

        public ParamDef() {
        }

        public static ParamDef FromParamInfo(ParameterInfo info) {
            var def = new ParamDef();

            def.TypeDef = new TypeDef(info.ParameterType);
            def.Name = info.Name;
            def.HasDefault = info.HasDefaultValue;
            def.DefaultValue = (info.DefaultValue == null) ? "null" : info.DefaultValue.ToString();

            return def;
        }
    }
}
