namespace JECSU.Serialization
{
    using UnityEngine;
    using System.Collections;
    using System.IO;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq;

    /// <summary>
    /// Class specifically designed to work with JECSu entities and components.
    /// Produces highly readable and editable files.
    /// </summary>
    public class Serializer : MonoBehaviour
    {
        public string path = "/entity.db";

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

        List<Type> componentTypes;
        void Start()
        {
            
            componentTypes = FindAllDerivedTypes<BaseComponent>();

            constructEntity();
        }

        void constructEntity()
        {
            string err = "";
            string[] warnings = null;
            if (!_constructEntity(out err, out warnings))
            {
                Debug.Log(err);
            }
        }

        bool _constructEntity(out string error, out string[] warnings)
        {
            Entity ent = null;
            Template template = new Template();
            warnings = new string[10];
            using (FileStream fs = new FileStream(Application.dataPath +  path,
                                           FileMode.OpenOrCreate,
                                           FileAccess.ReadWrite,
                                           FileShare.None))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    error = "";
                    string line = sr.ReadLine();
                    
                    //Try to find opening symbol, if found continue
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] == ' ')
                            continue;
                        if (line[i] == '<')
                        {
                            break;
                        }
                        else
                        {
                            error = "Deserealization of entity failed, opening symbol not found";
                            return false;
                        }
                    }

                    ent = new Entity();
                    template.entity = ent;
                    //get the entity database id and name
                    string[] splitline = line.Split(' ');

                    if (splitline[0] != "<")
                    {
                        error = "Deserealization of entity failed, opening symbol not found";
                        return false;
                    }
                    else
                    {
                        if (splitline.Length > 3)
                        {
                            warnings[0] = "Entity declaration has too much objects (must be 3 (<) (id) and (name), the rest will be ignored";
                        }
                        if (splitline.Length < 2)
                        {
                            error = "Deserealization of entity failed, entity not named";
                            return false;
                        }
                        else
                        {
                            ent.databaseID = splitline[1];
                            if (splitline.Length > 2)
                            {
                                ent.name = splitline[2];
                            }
                            else
                            {
                                warnings[1] = "Entity with ID but without a name, please add a name";
                            }
                        }
                    }

                    //start component addition loop
                    while (true)
                    {
                        Type componentType = null ;
                        
                        line = sr.ReadLine();
                        if (line.Contains("\t"))
                            line = line.Replace("\t", "");
                        if (!line.Contains("[") || !line.Contains("]"))
                        {
                            error = "unexpected member instead of component";
                            return false;
                        }
                        else
                        {
                            splitline = line.Split('[');
                            if (splitline.Length > 2)
                            {
                                error = "component declaration invalid";
                                return false;
                            }
                            splitline = splitline[1].Split(']');
                            if (splitline.Length > 2)
                            {
                                error = "component declaration invalid";
                                return false;
                            }

                            componentType = componentTypes.FirstOrDefault(x => x.Name == splitline[0]);
                            if(componentType != null)
                            {
                              //  template.components.Add(ComponentFactory.MakeNew)
                            }
                        }
                        return true;
                    }
                }
            }
        }
    }
}