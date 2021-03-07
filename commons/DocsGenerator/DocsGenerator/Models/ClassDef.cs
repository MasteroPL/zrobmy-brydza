using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml;
using DocsGenerator.Utils;
using EasyHosting.Models.Actions;

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
        public Dictionary<PropertyInfo, PropertyDef> Properties = new Dictionary<PropertyInfo, PropertyDef>();
        public Dictionary<EventInfo, EventDef> Events = new Dictionary<EventInfo, EventDef>();
        public Dictionary<FieldInfo, FieldDef> Fields = new Dictionary<FieldInfo, FieldDef>();

        public string GetDocRepresentation() {
            StringBuilder output = new StringBuilder();

            output.Append(TypeDef.GetDocFullName());

            return output.ToString();
        }

        public static ClassDef FromType(Type type, XMLDocs doc = null) {
            if (type == typeof(ActionsManager)) {
                Console.WriteLine("OK");
            }

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
            var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Static);
            var constructors = type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            var properties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            var events = type.GetEvents(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            MethodDef mdef;
            foreach(var method in methods) {
                mdef = MethodDef.FromMethodInfo(def, method, doc);
                def.Methods.Add(method, mdef);
            }
            ConstructorDef cdef;
            foreach(var constr in constructors) {
                cdef = ConstructorDef.FromConstructorInfo(def, constr, doc);
                def.Constructors.Add(constr, cdef);
            }
            PropertyDef pdef;
            foreach(var prpt in properties) {
                pdef = PropertyDef.FromPropertyInfo(def, prpt, doc);
                def.Properties.Add(prpt, pdef);
            }
            EventDef edef;
            foreach(var evt in events) {
                edef = EventDef.FromEventInfo(def, evt, doc);
                def.Events.Add(evt, edef);
            }
            FieldDef fdef;
            foreach(var fld in fields) {
                if (!fld.Name.Contains("k__BackingField")) {
                    fdef = FieldDef.FromFieldInfo(def, fld, doc);
                    def.Fields.Add(fld, fdef);
                }
            }

            if(doc != null) {
                XmlNode docsNode = doc.GetDocumentation(type);

                if(docsNode != null) {
                    XmlNode current;
                    for (int i = 0; i < docsNode.ChildNodes.Count; i++) {
                        current = docsNode.ChildNodes[i];

                        switch (current.Name) {
                            case "summary":
                                def.Summary = current.InnerText.Replace("\n\r            ", "\n").Replace("\r\n            ", "\n").Replace("\n            ", "\n");
                                def.Summary = def.Summary.Substring(1, def.Summary.Length - 2);
                                def.Summary.Replace("\n", "\n\t");
                                break;
                            case "remarks":
                                def.Remarks = current.InnerText;
                                break;
                            default:
                                Console.WriteLine("Unrecognized tag " + current.Name + " in class definition of " + def.TypeDef.FullName);
                                break;
                        }
                    }
                }
            }

            return def;
        }
    }
}
