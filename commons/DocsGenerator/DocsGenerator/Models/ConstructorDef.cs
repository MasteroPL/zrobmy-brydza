using DocsGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DocsGenerator.Models {
    public class ConstructorDef {
        public ClassDef ParentClass = null;
        public ConstructorInfo ConstructorInfo;
        public AccessType AccessType;

        public string Name = null;
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

        public string GetXMLName() {
            string ns = ParentClass.TypeDef.GetDocFullName();
            string name = Name;
            StringBuilder prs = new StringBuilder();

            bool first = true;
            foreach (var param in Params) {
                if (!first) {
                    prs.Append(",");
                }
                prs.Append(param.TypeDef.GetDocFullName());
                first = false;
            }
            return ns + "." + name + "#ctor(" + prs.ToString() + ")";
        }

        public static ConstructorDef FromConstructorInfo(ClassDef parentClass, ConstructorInfo info, XMLDocs doc = null) {
            var def = new ConstructorDef(parentClass);
            def.ConstructorInfo = info;

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

            // Dopisywanie dokumentacji
            if (doc != null) {
                XmlNode docsNode = doc.GetDocumentation(def);

                if (docsNode != null) {
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
                            case "param":
                                string name = current.Attributes["name"].InnerText;
                                var param = def.GetParam(name);
                                if (param != null) {
                                    param.Description = current.InnerText;
                                }
                                else {
                                    Console.WriteLine("Parameter not found: " + name);
                                }
                                break;

                            default:
                                Console.WriteLine("Unrecognized tag " + current.Name + " in class definition of " + def.ParentClass.TypeDef.FullName + ", method name " + def.Name);
                                break;
                        }
                    }
                }
            }

            return def;
        }
    }
}
