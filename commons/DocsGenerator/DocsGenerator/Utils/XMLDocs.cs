using DocsGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace DocsGenerator.Utils {
    public class XMLDocs {

        public AssemblyDef AssemblyDef = null;
        public Assembly SourceAssembly = null;
        public XmlDocument SourceDoc = null;
        public XmlNode SourceDocRoot = null;
        public Dictionary<string, XmlNode> DefinedMembers = new Dictionary<string, XmlNode>();

        // Helper method to format the key strings
        private static string XmlDocumentationKeyHelper(
          string typeFullNameString,
          string memberNameString
        ) {
            string key = Regex.Replace(
              typeFullNameString, @"\[.*\]",
              string.Empty).Replace('+', '.'
            );
            if (memberNameString != null) {
                key += "." + memberNameString;
            }
            return key;
        }
        public XmlNode GetDocumentation(Type type) {
            string key = "T:" + XmlDocumentationKeyHelper(type.FullName, null);
            return (DefinedMembers.ContainsKey(key)) ? DefinedMembers[key] : null;
        }
        public XmlNode GetDocumentation(MethodDef methodDef) {
            MethodInfo methodInfo = methodDef.MethodInfo;

            string methodName = methodInfo.ToString();
            methodName = methodName.Split(' ')[1];
            string methodKey = methodDef.GetXMLName();

            string key = "M:" + methodKey;
            //string key = "M:" + XmlDocumentationKeyHelper(methodInfo.DeclaringType.FullName, methodKey);

            return (DefinedMembers.ContainsKey(key)) ? DefinedMembers[key] : null;
        }
        public XmlNode GetDocumentation(PropertyInfo propertyInfo) {
            string key = "P:" + XmlDocumentationKeyHelper(
              propertyInfo.DeclaringType.FullName, propertyInfo.Name);
            return (DefinedMembers.ContainsKey(key)) ? DefinedMembers[key] : null;
        }

        protected static XmlNode GetMembersNode(XmlNode root) {
            for(int i = 0; i < root.ChildNodes.Count; i++) {
                if(root.ChildNodes[i].Name == "members") {
                    return root.ChildNodes[i];
                }
            }
            return null;
        }

        public void ProcessXML(string pathToXML) {
            DefinedMembers.Clear();
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;

            doc.Load(pathToXML);
            AssemblyDef = new AssemblyDef();

            var root = doc.ChildNodes.Item(1);
            var membersNode = GetMembersNode(root);
            this.SourceDoc = doc;
            this.SourceDocRoot = root;
            ReadAssembly();

            XmlNode memberNode;
            for(int i = 0; i < membersNode.ChildNodes.Count; i++) {
                memberNode = membersNode.ChildNodes[i];
                DefinedMembers.Add(memberNode.Attributes["name"].InnerText, memberNode);
            }

            var typesInAssembly = SourceAssembly.DefinedTypes;
            foreach(Type type in typesInAssembly) {
                ProcessType(type);
            }

            Console.WriteLine("OK");

            //XmlNode child;
            //for (int i = 0; i < root.ChildNodes.Count; i++) {
            //    child = root.ChildNodes[i];

            //    switch (child.Name) {
            //        case "assembly":
            //            ProcessAssemblyData(child);
            //            SourceAssembly = Assembly.Load(AssemblyDef.AssemblyName);
            //            break;

            //        case "members":
            //            ProcessMembersData(child);
            //            break;
            //    }
            //}
        }

        protected void ReadAssembly() {
            XmlDocument doc = this.SourceDoc;
            var nodes = doc.GetElementsByTagName("assembly");
            var node = nodes[0];

            var nameNode = node.FirstChild;
            SourceAssembly = Assembly.Load(nameNode.InnerText);
            AssemblyDef.AssemblyName = nameNode.InnerText;
        }

        protected void ProcessType(Type type) {
            ClassDef def = ClassDef.FromType(type, this);
            AssemblyDef.AddClassDef(def);
        }

        public void WriteToFiles(string targetDir = "_docs") {
            DocumentationWriter.WriteAssembly(targetDir, AssemblyDef);
        }

        protected void ProcessAssemblyData(XmlNode assemblyNode) {
            AssemblyDef.AssemblyName = assemblyNode.FirstChild.InnerText;
        }

        protected void ProcessMembersData(XmlNode membersNode) {
            XmlNode currentMember;

            for(int i = 0; i < membersNode.ChildNodes.Count; i++) {
                currentMember = membersNode.ChildNodes[i];
                ProcessMemberData(currentMember);
            }
        }

        protected void ProcessMemberData(XmlNode memberNode) {
            char memberType = memberNode.Attributes["name"].InnerText[0];

            switch (memberType) {
                case 'T':
                    ProcessMemberDataT(memberNode);
                    break;
                case 'M':
                    ProcessMemberDataM(memberNode);
                    break;
            }
        }

        protected void ProcessMemberDataT(XmlNode memberNode) {
            var fullName = memberNode.Attributes["name"].InnerText.Split(':')[1];
            Type type = Type.GetType(fullName + ", " + AssemblyDef.AssemblyName);

            var def = ClassDef.FromType(type);

            XmlNode node;
            for(int i = 0; i < memberNode.ChildNodes.Count; i++) {
                node = memberNode.ChildNodes[i];

                switch (node.Name) {
                    case "summary":
                        def.Summary = node.InnerText;
                        break;

                    case "remarks":
                        def.Remarks = node.InnerText;
                        break;
                }
            }

            AssemblyDef.ClassDefs.Add(type, def);
        }

        protected void ProcessMemberDataM(XmlNode memberNode) {
            // Szczytywanie ogólnych danych
            var name = memberNode.Attributes["name"].InnerText.Split(':')[1];
            string fullName;
            string paramsString;

            // Oddzielenie parametrów od nazwy
            string[] tmp = name.Split('(');
            fullName = tmp[0];
            paramsString = tmp[1].Substring(0, tmp[1].Length - 1);

            int lastDot = fullName.LastIndexOf('.');
            string className = fullName.Substring(0, lastDot);
            name = fullName.Substring(lastDot + 1);

            // Pobieranie klasy rodzica
            Type classType = Type.GetType(className + ", " + AssemblyDef.AssemblyName);

            // Pobieranie definicji klasy rodzica
            ClassDef parentClass;
            if (AssemblyDef.ClassDefs.ContainsKey(classType)) {
                parentClass = AssemblyDef.ClassDefs[classType];
            }
            else {
                parentClass = ClassDef.FromType(classType);
                AssemblyDef.ClassDefs.Add(classType, parentClass);
            }

            // Przetwarzanie listy parametrów
            Type parentClassType = parentClass.TypeDef.Type;
            Type[] paramsTypes = ProcessParametersString(paramsString);

            // Pobieranie informacji o metodzie
            if (name != "#ctor") {
                MethodInfo info = parentClassType.GetMethod(name,
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy,
                    null,
                    paramsTypes,
                    null
                );

                if (info == null) {
                    return;
                }

                // Pobieranie definicji metody
                MethodDef def;
                if (parentClass.Methods.ContainsKey(info)) {
                    def = parentClass.Methods[info];
                }
                else {
                    def = MethodDef.FromMethodInfo(parentClass, info);
                    parentClass.Methods.Add(info, def);
                }

                // Przetwarzanie danych XML
                XmlNode node;
                for (int i = 0; i < memberNode.ChildNodes.Count; i++) {
                    node = memberNode.ChildNodes[i];

                    switch (node.Name) {
                        case "summary":
                            def.Summary = node.InnerText;
                            break;
                        case "remarks":
                            def.Remarks = node.InnerText;
                            break;
                        case "param":
                            var paramName = node.Attributes["name"].InnerText;

                            var paramDef = def.GetParam(paramName);
                            if (paramDef != null) {
                                paramDef.Description = node.InnerText;
                            }
                            break;
                        case "returns":
                            def.Returns.Description = node.InnerText;
                            break;
                    }
                }
            }
            else {
                ConstructorInfo info = parentClassType.GetConstructor(
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                    null,
                    paramsTypes,
                    null
                );

                if (info == null) {
                    return;
                }

                // Pobieranie definicji metody
                ConstructorDef def;
                if (parentClass.Constructors.ContainsKey(info)) {
                    def = parentClass.Constructors[info];
                }
                else {
                    def = ConstructorDef.FromConstructorInfo(parentClass, info);
                    parentClass.Constructors.Add(info, def);
                }

                // Przetwarzanie danych XML
                XmlNode node;
                for (int i = 0; i < memberNode.ChildNodes.Count; i++) {
                    node = memberNode.ChildNodes[i];

                    switch (node.Name) {
                        case "summary":
                            def.Summary = node.InnerText;
                            break;
                        case "remarks":
                            def.Remarks = node.InnerText;
                            break;
                        case "param":
                            var paramName = node.Attributes["name"].InnerText;

                            var paramDef = def.GetParam(paramName);
                            if (paramDef != null) {
                                paramDef.Description = node.InnerText;
                            }
                            break;
                    }
                }
            }

            
        }

        protected Type[] ProcessParametersString(string paramsString) {
            if (paramsString.Length == 0) {
                return new Type[0];
            }

            string[] parameters = paramsString.Split(',');
            var result = new Type[parameters.Length];

            for (int i = 0; i < parameters.Length; i++) {
                Type tmp = SourceAssembly.GetType(parameters[i] + ", " + AssemblyDef.AssemblyName);

                if(tmp == null) {
                    tmp = Type.GetType(parameters[i], true);
                }
                result[i] = tmp;
            }
            return result;
        }

        public static Type ReconstructType(string assemblyQualifiedName, bool throwOnError = true, params Assembly[] referencedAssemblies) {
            foreach (Assembly asm in referencedAssemblies) {
                var fullNameWithoutAssemblyName = assemblyQualifiedName.Replace($", {asm.FullName}", "");
                var type = asm.GetType(fullNameWithoutAssemblyName, throwOnError: false);
                if (type != null) return type;
            }

            if (assemblyQualifiedName.Contains("[[")) {
                Type type = ConstructGenericType(assemblyQualifiedName, throwOnError);
                if (type != null)
                    return type;
            }
            else {
                Type type = Type.GetType(assemblyQualifiedName, false);
                if (type != null)
                    return type;
            }

            if (throwOnError)
                throw new Exception($"The type \"{assemblyQualifiedName}\" cannot be found in referenced assemblies.");
            else
                return null;
        }

        private static Type ConstructGenericType(string assemblyQualifiedName, bool throwOnError = true) {
            Regex regex = new Regex(@"^(?<name>\w+(\.\w+)*)`(?<count>\d)\[(?<subtypes>\[.*\])\](, (?<assembly>\w+(\.\w+)*)[\w\s,=\.]+)$?", RegexOptions.Singleline | RegexOptions.ExplicitCapture);
            Match match = regex.Match(assemblyQualifiedName);
            if (!match.Success)
                if (!throwOnError) return null;
                else throw new Exception($"Unable to parse the type's assembly qualified name: {assemblyQualifiedName}");

            string typeName = match.Groups["name"].Value;
            int n = int.Parse(match.Groups["count"].Value);
            string asmName = match.Groups["assembly"].Value;
            string subtypes = match.Groups["subtypes"].Value;

            typeName = typeName + $"`{n}";
            Type genericType = ReconstructType(typeName, throwOnError);
            if (genericType == null) return null;

            List<string> typeNames = new List<string>();
            int ofs = 0;
            while (ofs < subtypes.Length && subtypes[ofs] == '[') {
                int end = ofs, level = 0;
                do {
                    switch (subtypes[end++]) {
                        case '[': level++; break;
                        case ']': level--; break;
                    }
                } while (level > 0 && end < subtypes.Length);

                if (level == 0) {
                    typeNames.Add(subtypes.Substring(ofs + 1, end - ofs - 2));
                    if (end < subtypes.Length && subtypes[end] == ',')
                        end++;
                }

                ofs = end;
                n--;  // just for checking the count
            }

            if (n != 0)
                // This shouldn't ever happen!
                throw new Exception("Generic type argument count mismatch! Type name: " + assemblyQualifiedName);

            Type[] types = new Type[typeNames.Count];
            for (int i = 0; i < types.Length; i++) {
                try {
                    types[i] = ReconstructType(typeNames[i], throwOnError);
                    if (types[i] == null)  // if throwOnError, should not reach this point if couldn't create the type
                        return null;
                } catch (Exception ex) {
                    throw new Exception($"Unable to reconstruct generic type. Failed on creating the type argument {(i + 1)}: {typeNames[i]}. Error message: {ex.Message}");
                }
            }

            Type resultType = genericType.MakeGenericType(types);
            return resultType;
        }
    }
}
