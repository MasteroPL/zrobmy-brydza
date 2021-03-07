using DocsGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DocsGenerator.Models {
    public class EventDef {
        public ClassDef ParentClass;
        public TypeDef EventHandlerTypeDef;
        public AccessType AccessType;
        public string Name = null;
        public string Summary = null;
        public string Remarks = null;
        public bool IsStatic = false;

        public EventDef() { }

        public static EventDef FromEventInfo(ClassDef parentClass, EventInfo info, XMLDocs doc = null) {
            var def = new EventDef();

            def.ParentClass = parentClass;
            def.EventHandlerTypeDef = new TypeDef(info.EventHandlerType);
            def.Name = info.Name;
            var addMethod = info.GetAddMethod(true);
            def.IsStatic = addMethod.IsStatic;

            // Określanie poziomu dostępu
            if (addMethod.IsPublic) {
                def.AccessType = AccessType.PUBLIC;
            }
            else if (addMethod.IsFamily) {
                def.AccessType = AccessType.PROTECTED;
            }
            else if (addMethod.IsPrivate) {
                def.AccessType = AccessType.PRIVATE;
            }
            else {
                def.AccessType = AccessType.INNER;
            }

            if (doc != null) {
                XmlNode docsNode = doc.GetDocumentation(info);

                if(docsNode != null) {
                    XmlNode current;
                    for(int i = 0; i < docsNode.ChildNodes.Count; i++) {
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
                                Console.WriteLine("Unrecognized tag " + current.Name + " in class definition of " + def.ParentClass.TypeDef.FullName + ", event name " + def.Name);
                                break;
                        }
                    }
                }
            }

            return def;
        }
    }
}
