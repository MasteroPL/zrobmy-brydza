using DocsGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsGenerator.Utils {
    public static class DocumentationWriter {

        public static void WriteAssembly(string outputDir, AssemblyDef assemblyDef) {
            string destDir = Path.Combine(outputDir, assemblyDef.AssemblyName);
            Directory.CreateDirectory(destDir);
            string filePath;

            filePath = Path.Combine(destDir, "_index.rst");

            using (StreamWriter wr = new StreamWriter(filePath)) {
                wr.WriteLine("#################");
                wr.WriteLine("Dokumentacja klas");
                wr.WriteLine("#################");
                wr.WriteLine();
                wr.WriteLine(".. toctree::");
                wr.WriteLine("\t:max-depth: 2");
                wr.WriteLine();

                foreach (var classDef in assemblyDef.ClassDefs.Values) {
                    if (classDef.TypeDef.Name == "<>c")
                        continue;

                    wr.WriteLine("\t" + classDef.TypeDef.Name);
                }
            }

            foreach (var classDef in assemblyDef.ClassDefs.Values) {
                if (classDef.TypeDef.Name == "<>c")
                    continue;

                filePath = Path.Combine(destDir, classDef.TypeDef.Name + ".rst");

                using(StreamWriter wr = new StreamWriter(filePath)) {
                    WriteClass(wr, classDef);
                }
            }
        }

        public static void WriteClass(StreamWriter writeTo, ClassDef classDef) {
            StringBuilder header = new StringBuilder();
            string title = classDef.TypeDef.GetDocName();

            for(int i = 0; i < title.Length; i++) {
                header.Append('*');
            }
            string headerS = header.ToString();

            writeTo.Write(
                headerS + "\n" +
                title + "\n" +
                headerS + "\n\n" +
                ".. sphinxsharp:type:: " +
                AccessTypeM.GetName(classDef.AccessType) + " class " +
                classDef.TypeDef.Name + "\n\t\n\t" +
                classDef.Summary + "\n\n"
            );

            // Konstruktory
            writeTo.Write(
                "Konstruktory\n" +
                "============\n\n"
            );

            foreach(var constructorDef in classDef.Constructors.Values) {
                WriteConstructor(writeTo, constructorDef);
            }

            // Metody
            writeTo.Write(
                "Metody\n" +
                "======\n\n"
            );

            foreach(var methodDef in classDef.Methods.Values) {
                WriteMethod(writeTo, methodDef);
            }
        }

        public static void WriteConstructor(StreamWriter writeTo, ConstructorDef constructorDef) {
            writeTo.Write(
                ".. sphinxsharp:method:: "
                + AccessTypeM.GetName(constructorDef.AccessType) + " "
                + constructorDef.ParentClass.TypeDef.GetDocName() + "("
            );

            // Opis parametrów
            bool first = true;
            foreach (var param in constructorDef.Params) {
                if (!first) {
                    writeTo.Write(", ");
                }

                writeTo.Write(param.TypeDef.GetDocName() + " " + param.Name);

                if (param.HasDefault) {
                    writeTo.Write("=" + param.DefaultValue);
                }

                first = false;
            }

            writeTo.Write(")");

            for (int i = 0; i < constructorDef.Params.Count; i++) {
                writeTo.Write(
                    "\n\t:param(" + (i + 1) + "): "
                    + constructorDef.Params[i].Description
                );
            }

            // Podsumowanie
            writeTo.Write("\n\t\n\t" + constructorDef.Summary);

            // Końcowy odstęp
            writeTo.Write("\n\n\n");
        }

        public static void WriteMethod(StreamWriter writeTo, MethodDef methodDef) {
            writeTo.Write(
                ".. sphinxsharp:method:: "
                + AccessTypeM.GetName(methodDef.AccessType) + " "
                + methodDef.Returns.TypeDef.GetDocName() + " "
                + methodDef.Name + "("
            );

            // Opis parametrów
            bool first = true;
            foreach(var param in methodDef.Params) {
                if (!first) {
                    writeTo.Write(", ");
                }

                writeTo.Write(param.TypeDef.GetDocName() + " " + param.Name);

                if (param.HasDefault) {
                    writeTo.Write("=" + param.DefaultValue);
                }

                first = false;
            }

            writeTo.Write(")");

            for(int i = 0; i < methodDef.Params.Count; i++) {
                writeTo.Write(
                    "\n\t:param(" + (i + 1) + "): "
                    + methodDef.Params[i].Description
                );
            }

            // Podsumowanie
            writeTo.Write("\n\t\n\t" + methodDef.Summary);

            // Końcowy odstęp
            writeTo.Write("\n\n\n");
        }
    }
}
