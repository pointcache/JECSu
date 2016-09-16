namespace JECSU.CodeGeneration
{

    using UnityEngine;
    using UnityEditor;
    using System.IO;
    using System.Reflection;
    using System.Linq;
    using System.Collections.Generic;
    using System;


    public static class ComponentFactoryGenerator
    {
        /// <summary>
        /// Resets component factory to empty state in case you have compile problem
        /// </summary>
        [MenuItem("JECSU/ComponentFactory/Reset")]
        public static void ResetComponentFactory()
        {
            string path = CodeGenerator.getfile("ComponentFactory.cs");
            File.WriteAllText(path,string.Empty);
            using (FileStream fs = new FileStream(path,
                                           FileMode.OpenOrCreate,
                                           FileAccess.ReadWrite,
                                           FileShare.None))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("/*This is generated file.");
                    sw.WriteLine("This class speeds up component creation in a safe manner.");
                    sw.WriteLine();
                    sw.WriteLine("===============================================================================================*/");
                    sw.WriteLine();

                    sw.WriteLine("namespace JECSU");
                    sw.WriteLine("{");
                    sw.WriteLine("using System;");

                    sw.WriteLine("\tpublic static class ComponentFactory");
                    sw.WriteLine("\t{");

                    sw.WriteLine("\t\tpublic static IComponent MakeNew<T>() where T : BaseComponent");
                    sw.WriteLine("\t\t{");
                    sw.WriteLine("\t\t\tType t = typeof(T);");
                    sw.WriteLine("\t\t\treturn MakeNew(t);");
                    sw.WriteLine("\t\t}");
                    

                    sw.WriteLine("\t\tpublic static IComponent MakeNew(Type t)");
                    sw.WriteLine("\t\t{");
                    sw.WriteLine("\t\t return null;");
                    sw.WriteLine("\t\t}");
                    sw.WriteLine("\t}");
                    sw.WriteLine("}");

                    sw.WriteLine("//EOF");
                }
            }
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Component factory speeds up component creation by avoiding the use of Activator by generating constructor calls.
        /// </summary>
        [MenuItem("JECSU/ComponentFactory/Compile")]
        public static void GenerateComponentFactory()
        {
            string path = CodeGenerator.getfile("ComponentFactory.cs");
            File.WriteAllText(path,string.Empty);
            var componentTypes = CodeGenerator.FindAllDerivedTypes<BaseComponent>();
            using (FileStream fs = new FileStream(path,
                                           FileMode.OpenOrCreate,
                                           FileAccess.ReadWrite,
                                           FileShare.None))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("/*This is generated file.");
                    sw.WriteLine("This class speeds up component creation in a safe manner.");
                    sw.WriteLine();
                    sw.WriteLine("===============================================================================================*/");
                    sw.WriteLine();


                    sw.WriteLine("namespace JECSU");
                    sw.WriteLine("{");
                    sw.WriteLine("using System;");
                    sw.WriteLine("using System.Collections.Generic;");
                    sw.WriteLine("\tpublic static class ComponentFactory");
                    sw.WriteLine("\t{");
                    sw.WriteLine("\t\tstatic int lookup;");

                    sw.WriteLine("\t\tpublic static IComponent MakeNew<T>() where T : BaseComponent");
                    sw.WriteLine("\t\t{");
                    sw.WriteLine("\t\t\tType t = typeof(T);");
                    sw.WriteLine("\t\t\treturn MakeNew(t);");
                    sw.WriteLine("\t\t}");


                    sw.WriteLine("\t\tpublic static IComponent MakeNew(Type t)");
                    sw.WriteLine("\t\t{");
                    sw.WriteLine("\t\t\tif (!@switch.ContainsKey(t))return null;");
                    sw.WriteLine("\t\t\t@switch[t].Invoke();");
                    sw.WriteLine("\t\t\tswitch (lookup)");
                    sw.WriteLine("\t\t\t{");

                    int count = 0;
                    foreach (var type in componentTypes)
                    {
                        output_case(sw, type, count);
                        count++;
                    }
                    sw.WriteLine("\t\t\t default:");
                    sw.WriteLine("\t\t\t\t return null;");

                    sw.WriteLine("\t\t\t}");
                    sw.WriteLine("\t\t}");

                    sw.WriteLine("\t\tstatic Dictionary<Type, Action> @switch = new Dictionary<Type, Action> {");

                    count = 0;
                    foreach (var type in componentTypes)
                    {
                        output_dict(sw, type, count);
                        count++;
                    }
                    sw.WriteLine("\t\t};");
                    sw.WriteLine("\t}");
                    sw.WriteLine("}");

                    sw.WriteLine("//EOF");
                }
            }
            AssetDatabase.Refresh();

        }

        static void output_case(StreamWriter sw, Type t, int number)
        {
            sw.WriteLine("\t\t\t case {0}:", number);
            sw.WriteLine("\t\t\t\t return new {0}();", CodeGenerator.cleanname(t));
        }

        static void output_dict(StreamWriter sw, Type t, int number)
        {
            if (number == 0)
                sw.WriteLine("\t\t{ typeof(" + CodeGenerator.cleanname(t) + "), () => lookup = " + number + " }");
            else
                sw.WriteLine("\t\t,{ typeof(" + CodeGenerator.cleanname(t) + "), () => lookup = " + number + " }");
        }
    }
}