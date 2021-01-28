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

        public string GetDocRepresentation() {
            StringBuilder output = new StringBuilder();

            if (AccessType != AccessType.INNER) {
                output.Append(AccessTypeM.GetName(AccessType) + " ");
            }
            output.Append(TypeDef.Name);
            output.Append(" : ");
            output.Append(BaseClass.Name);
            output.Append("\n");
            output.Append("Summary: ");
            output.Append(Summary);
            output.Append("\n");
            output.Append("Remarks: ");
            output.Append(Remarks);

            foreach (var m in Methods.Values) {
                output.Append("\n");
                output.Append(m.GetDocRepresentation());
            }

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
            var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var constructors = type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            MethodDef mdef;
            foreach(var method in methods) {
                mdef = MethodDef.FromMethodInfo(def, method, doc);
                def.Methods.Add(method, mdef);
            }
            ConstructorDef cdef;
            foreach(var constr in constructors) {
                cdef = ConstructorDef.FromConstructorInfo(def, constr);
                def.Constructors.Add(constr, cdef);
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
