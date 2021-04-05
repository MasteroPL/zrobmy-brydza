using DocsGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DocsGenerator.Models {
    public class PropertyDef {
        public ClassDef ParentClass;
        public TypeDef TypeDef;
        public AccessType AccessType;
        public string Name = null;
        public string Summary = null;
        public string Remarks = null;
        public bool IsStatic = false;

        public PropertyDef() { }

        public static PropertyDef FromPropertyInfo(ClassDef parentClass, PropertyInfo info, XMLDocs doc = null) {
            var def = new PropertyDef();

            def.ParentClass = parentClass;
            def.TypeDef = new TypeDef(info.PropertyType);
            def.Name = info.Name;
            var accessors = info.GetAccessors(true);
            def.IsStatic = accessors.Any(x => x.IsStatic);

            // Określanie poziomu dostępu
            if (accessors.Any(x => x.IsPublic)) {
                def.AccessType = AccessType.PUBLIC;
            }
            else if (accessors.Any(x => x.IsFamily)) {
                def.AccessType = AccessType.PROTECTED;
            }
            else if (accessors.Any(x => x.IsPrivate)) {
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
                                Console.WriteLine("Unrecognized tag " + current.Name + " in class definition of " + def.ParentClass.TypeDef.FullName + ", property name " + def.Name);
                                break;
                        }
                    }
                }
            }


            return def;
        }
    }
}
