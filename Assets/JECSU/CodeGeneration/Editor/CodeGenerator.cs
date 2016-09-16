namespace JECSU.CodeGeneration
{

    using UnityEngine;
    using UnityEditor;
    using System.IO;
    using System.Reflection;
    using System.Linq;
    using System.Collections.Generic;
    using System;

    /// <summary>
    /// Various functionality that speeds up various things in system.
    /// </summary>
    public static class CodeGenerator
    {
        [MenuItem("JECSU/CompileEverything")]
        public static void CompileEverything()
        {
            ComponentFactoryGenerator.GenerateComponentFactory();
            EntityFactoryGenerator.GenerateEntityConstructor();
        }


        [MenuItem("JECSU/ResetEverything")]
        public static void ResetEverything()
        {
            ComponentFactoryGenerator.ResetComponentFactory();
            EntityFactoryGenerator.ResetEntityConstructor();
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

       

        

        public static string cleanname(Type t)
        {
            if (t.FullName.Contains("+"))
            {
                return t.FullName.Replace('+', '.');
            }
            return t.FullName;
        }



        public static string getfile(string file)
        {
            //First we try to find already existing .cs file project wide, and if we dont find it, we create a new one.
            string path = "";
            List<string> files = Directory.GetFiles(Application.dataPath + "/JECSU/", "*.cs", SearchOption.AllDirectories).ToList();
            foreach (string st in files)
            {
                if (st.Contains(file))
                {
                    string[] pathArr = st.Split('\\');
                    if (pathArr[pathArr.Length - 1].Equals(file))
                    {
                        path = st;
                    }
                    break;
                }
            }
            if (path == "")
            {
                path = Application.dataPath + "/JECSU/" + file;
            }
            return path;
        }
    }
}