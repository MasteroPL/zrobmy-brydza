using DocsGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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

        public string GetDocRepresentation() {
            StringBuilder output = new StringBuilder();

            if (AccessType != AccessType.INNER) {
                output.Append(AccessTypeM.GetName(AccessType) + " ");
            }

            output.Append(
                this.Returns.TypeDef.Name
                + " " + this.Name
                + "("
            );

            bool first = true;
            foreach (var param in Params) {
                output.Append(param.TypeDef.Name + " " + param.Name);

                if (param.HasDefault) {
                    output.Append("=" + param.DefaultValue);
                }

                if (!first) {
                    output.Append(",");
                }

                first = false;
            }
            output.Append(")");
            output.Append("\n");
            output.Append("Summary: " + Summary + "\n");
            output.Append("Remarks: " + Remarks + "\n");
            output.Append("Params:");
            
            foreach(var param in Params) {
                output.Append("\n\t" + param.Name + ": " + param.Description);
            }

            return output.ToString();
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

        public static MethodDef FromMethodInfo(ClassDef parentClass, MethodInfo info, XMLDocs doc = null) {
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

            // Dopisywanie dokumentacji
            if(doc != null) {
                XmlNode docsNode = doc.GetDocumentation(info);

                if (docsNode != null) {
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
