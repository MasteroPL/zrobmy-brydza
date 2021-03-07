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
                wr.Write("#################\n");
                wr.Write("Dokumentacja klas\n");
                wr.Write("#################\n");
                wr.Write("\n");
                wr.Write(".. toctree::\n");
                wr.Write("    :maxdepth: 2\n");
                wr.Write("    \n");

                foreach (var classDef in assemblyDef.ClassDefs.Values) {
                    if (classDef.TypeDef.Name == "<>c")
                        continue;

                    wr.Write("    " + classDef.TypeDef.Name + "\n");
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
                ".. csharpdocsclass:: " +
                //AccessTypeM.GetName(classDef.AccessType) + " class " +
                classDef.TypeDef.GetDocFullName() + "\n" +
                "    :access: " + AccessTypeM.GetName(classDef.AccessType)
            );

            if(classDef.BaseClass != null) {
                writeTo.Write(
                    "\n    :baseclass: " + classDef.BaseClass.GetDocFullName()
                );
            }

            writeTo.Write("\n\t\n\t" + classDef.Summary + "\n\n");

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

            // Własności
            writeTo.Write(
                "Własności\n" +
                "=========\n\n"
            );

            foreach(var propertyDef in classDef.Properties.Values) {
                WriteProperty(writeTo, propertyDef);
            }

            // Pola
            writeTo.Write(
                "Pola\n" +
                "====\n\n"
            );

            foreach (var fieldDef in classDef.Fields.Values) {
                WriteField(writeTo, fieldDef);
            }

            // Wydarzenia
            writeTo.Write(
                "Wydarzenia\n" +
                "==========\n\n"
            );

            foreach(var eventDef in classDef.Events.Values) {
                WriteEvent(writeTo, eventDef);
            }
        }

        public static void WriteConstructor(StreamWriter writeTo, ConstructorDef constructorDef) {
            writeTo.Write(
                ".. csharpdocsconstructor:: "
                + constructorDef.ParentClass.TypeDef.GetDocName() + "("
            );

            // Opis parametrów
            bool first = true;
            foreach (var param in constructorDef.Params) {
                if (!first) {
                    writeTo.Write(", ");
                }

                writeTo.Write(param.TypeDef.GetDocFullName() + " " + param.Name);

                if (param.HasDefault) {
                    writeTo.Write("=" + param.DefaultValue);
                }

                first = false;
            }

            writeTo.Write(")");

            writeTo.Write(
                "\n    :access: " + AccessTypeM.GetName(constructorDef.AccessType)
            );

            for (int i = 0; i < constructorDef.Params.Count; i++) {
                writeTo.Write(
                    "\n    :param(" + (i + 1) + "): "
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
                ".. csharpdocsmethod:: "
                + methodDef.Returns.TypeDef.GetDocFullName() + " "
                + methodDef.Name + "("
            );

            // Opis parametrów
            bool first = true;
            foreach(var param in methodDef.Params) {
                if (!first) {
                    writeTo.Write(", ");
                }

                writeTo.Write(param.TypeDef.GetDocFullName() + " " + param.Name);

                if (param.HasDefault) {
                    writeTo.Write("=" + param.DefaultValue);
                }

                first = false;
            }

            writeTo.Write(")");

            writeTo.Write("\n    :access: " + AccessTypeM.GetName(methodDef.AccessType));
            if (methodDef.IsStatic) {
                writeTo.Write(" static");
            }

            for(int i = 0; i < methodDef.Params.Count; i++) {
                writeTo.Write(
                    "\n    :param(" + (i + 1) + "): "
                    + methodDef.Params[i].Description
                );
            }

            // Podsumowanie
            writeTo.Write("\n\t\n\t" + methodDef.Summary);

            // Końcowy odstęp
            writeTo.Write("\n\n\n");
        }

        public static void WriteProperty(StreamWriter writeTo, PropertyDef propertyDef) {
            writeTo.Write(
                ".. csharpdocsproperty:: "
                + propertyDef.TypeDef.GetDocFullName() + " "
                + propertyDef.Name
            );

            writeTo.Write("\n    :access: " + AccessTypeM.GetName(propertyDef.AccessType));
            if (propertyDef.IsStatic) {
                writeTo.Write(" static");
            }

            // Podsumowanie
            writeTo.Write("\n\t\n\t" + propertyDef.Summary);

            // Końcowy odstęp
            writeTo.Write("\n\n\n");
        }

        public static void WriteField(StreamWriter writeTo, FieldDef fieldDef) {
            writeTo.Write(
                ".. csharpdocsproperty:: "
                + fieldDef.TypeDef.GetDocFullName() + " "
                + fieldDef.Name
            );

            writeTo.Write("\n    :access: " + AccessTypeM.GetName(fieldDef.AccessType));
            if (fieldDef.IsStatic) {
                writeTo.Write(" static");
            }

            // Podsumowanie
            writeTo.Write("\n\t\n\t" + fieldDef.Summary);

            // Końcowy odstęp
            writeTo.Write("\n\n\n");
        }

        public static void WriteEvent(StreamWriter writeTo, EventDef eventDef) {
            writeTo.Write(
                ".. csharpdocsproperty:: "
                + eventDef.EventHandlerTypeDef.GetDocFullName() + " "
                + eventDef.Name
            );

            writeTo.Write("\n    :access: " + AccessTypeM.GetName(eventDef.AccessType));
            if (eventDef.IsStatic) {
                writeTo.Write(" static");
            }
            writeTo.Write(" event");

            // Podsumowanie
            writeTo.Write("\n\t\n\t" + eventDef.Summary);

            // Końcowy odstęp
            writeTo.Write("\n\n\n");
        }
    }
}
