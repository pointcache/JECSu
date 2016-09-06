using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System;
using EntitySystem;

/// <summary>
/// Various functionality that speeds up various things in system.
/// </summary>
public static class CodeGenerator
{


    /// <summary>
    /// Component factory speeds up component creation by avoiding the use of Activator by generating constructor calls.
    /// </summary>
    [MenuItem("EntitySystem/Compile")]
    public static void GenerateComponentFactory()
    {

        string filename = "ComponentFactory.cs";
        var componentTypes = FindAllDerivedTypes<BaseComponent>();
        //First we try to find already existing .cs file project wide, and if we dont find it, we create a new one.
        string path = "";
        List<string> files = Directory.GetFiles(Application.dataPath + "/EntitySystem/", "*.cs", SearchOption.AllDirectories).ToList();
        foreach (string st in files)
        {
            if (st.Contains(filename))
            {
                string[] pathArr = st.Split('\\');
                if (pathArr[pathArr.Length - 1].Equals(filename))
                {
                    path = st;
                }
                break;
            }
        }
        if (path == "")
        {
            path = Application.dataPath + "/EntitySystem/" + filename;
        }




        //
        //We start generating the actual file
        //

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

                sw.WriteLine("using UnityEngine;");
                sw.WriteLine("using System;");
                sw.WriteLine("using System.Collections.Generic;");
                sw.WriteLine("using EntitySystem.Components;");
                sw.WriteLine("namespace EntitySystem");
                sw.WriteLine("{");
                sw.WriteLine("\tpublic static class ComponentFactory");
                sw.WriteLine("\t{");
                sw.WriteLine("\t\tstatic int lookup;");
                sw.WriteLine("\t\tpublic static iComponent MakeNew<T>() where T : BaseComponent");
                sw.WriteLine("\t\t{");
                sw.WriteLine("\t\t\tType t = typeof(T);");
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


    }

    public static List<Type> FindAllDerivedTypes<T>()
    {
        return FindAllDerivedTypes<T>(Assembly.GetAssembly(typeof(T)));
    }

    public static List<Type> FindAllDerivedTypes<T>(Assembly assembly)
    {
        var derivedType = typeof(T);
        return assembly
            .GetTypes()
            .Where(t =>
                t != derivedType &&
                derivedType.IsAssignableFrom(t)
                ).ToList();

    } 

    static void output_case(StreamWriter sw, Type t, int number)
    {
        sw.WriteLine("\t\t\t case {0}:", number);
        sw.WriteLine("\t\t\t\t return new {0}();", cleanname(t));
    }

    static string cleanname(Type t)
    {
        if (t.FullName.Contains("+"))
        {
            return t.FullName.Replace('+', '.');
        }
        return t.FullName;
    }

    static void output_dict(StreamWriter sw, Type t, int number)
    {
        if(number == 0)
            sw.WriteLine("\t\t{ typeof(" + cleanname(t) + "), () => lookup = " + number + " }");
        else
            sw.WriteLine("\t\t,{ typeof(" + cleanname(t) + "), () => lookup = " + number + " }");
     }
}
