namespace JECSU
{
    using UnityEngine;
    using System.Collections;
    using Serialization;
    using System.Collections.Generic;
    using System;
    using System.Reflection;
    using System.Linq;
    using Components;
    public partial class EntityConstructor
    {

         //array of names of existing Component types 
        static string[] componentTypes;
        static Dictionary<string, Type> components_dict = new Dictionary<string, Type>();
        public void Initialize()
        {
            var list = FindAllDerivedTypes<BaseComponent>();
             //pull all existing components
            componentTypes = list.Select(x => x.Name).ToArray();
            for (int i = 0; i < list.Count; i++)
            {
                components_dict.Add(list[i].Name, list[i]);
            }
        }

        public Entity constructEntityFromTemplate(EntityTemplate template)
        {
            Entity ent = Entity.New(template.Name);
            ent.databaseID = template.databaseID;

            foreach (var p in template.components)
            {
                if (components_dict.ContainsKey(p.Key))
                {
                    IComponent comp = ComponentFactory.MakeNew(components_dict[p.Key]);
                    AssignFromTemplate(comp, p.Value);
                    comp.owner = ent;
                    EntityManager.RegisterComponent(comp);
                }
            }
            
            return ent;
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

        static bool componentExists(string comname)
        {
            int count = componentTypes.Length;
            for (int i = 0; i < count; i++)
            {
                if (comname.Equals(componentTypes[i]))
                    return true;
            }
            return false;
        }

        static Color convertColor(string str)
        {

            if (str.Length == 0)
                return Color.black;
            if(str[0] == '#')
            {
                return HexToColor(str.Replace("#", ""));
            }
            else
            {
                var split = str.Split(',');
                float firstval = Convert.ToSingle(split[0]);
                if(firstval > 1)
                {
                    if(split.Length == 3)
                    {
                        return new Color(firstval / 255f, Convert.ToSingle(split[1]) / 255f, Convert.ToSingle(split[2]) / 255f);
                    }
                    else
                    {
                        return new Color(firstval / 255f, Convert.ToSingle(split[1]) / 255f, Convert.ToSingle(split[2]) / 255f, Convert.ToSingle(split[3]) / 255f);
                    }
                }
                else
                {
                    if(split.Length == 3)
                    {
                        return new Color(firstval, Convert.ToSingle(split[1]), Convert.ToSingle(split[2]));
                    }
                    else
                    {
                        return new Color(firstval, Convert.ToSingle(split[1]), Convert.ToSingle(split[2]), Convert.ToSingle(split[3]));
                    }
                }
            }
        }

        static Color HexToColor(string hex)
        {
        	byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
        	byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
        	byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
        	return new Color32(r,g,b, 255);
        }

        static Vector3 convertV3(string str)
        {
            Vector3 v3 = Vector3.zero;

            string[] split = str.Split(',');
            if (split.Length < 3)
                return Vector3.zero;

            v3.x = Convert.ToSingle(split[0]);
            v3.y = Convert.ToSingle(split[1]);
            v3.z = Convert.ToSingle(split[2]);

            return v3;
        }

        static Vector2 convertV2(string str)
        {
            Vector2 v2 = Vector2.zero;

            string[] split = str.Split(',');
            if (split.Length < 2)
                return Vector2.zero;

            v2.x = Convert.ToSingle(split[0]);
            v2.y = Convert.ToSingle(split[1]);
            
            return v2;
        }

        static bool convertBool(string str)
        {
            switch (str)
            {
                case "yes":
                    return true;
                case "True":
                    return true;
                case "true":
                    return true;
                case "false":
                    return false;
                case "False":
                    return false;
                case "no":
                    return false;
                default:
                    Debug.LogError("Parsing error on bool");
                    return false;
            }
        }
    }
}