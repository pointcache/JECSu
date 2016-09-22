namespace JECSU {

    using UnityEngine;
    using System.Xml.Serialization;
    using System.IO;
    using System.Xml;
    using System.Text;
    using Serialization;
    using System.Collections.Generic;
    using System;

    /// <summary>
    /// This objects represents database of entities that can be constructed later, it doesnt contain actual Entity objects or their component, only their Templates
    /// </summary>
    public class TemplateDatabase {

        JECSUConfig cfg;
        TemplateDatabaseParser parser;
        EntityConstructor contructor;

        //key = templateID
        Dictionary<string, EntityTemplate> alltemplates = new Dictionary<string, EntityTemplate>();
        Dictionary<string, List<IComponent>> constructedEntities = new Dictionary<string, List<IComponent>>();

        
        public TemplateDatabase() 
        {
            cfg = JECSUConfig.current;
            parser = new TemplateDatabaseParser();
            parser.Initialize(cfg);


            fillDatabase(parser);
            

            contructor = new EntityConstructor();
            contructor.Initialize();
            fillEntitiesDictionary();
        }

        public Entity GetById(string databaseID)
        {
            if (!alltemplates.ContainsKey(databaseID))
                return null;
            else
            {
                return contructor.constructEntityFromTemplate(alltemplates[databaseID]);
            }
        }

        void fillDatabase(TemplateDatabaseParser parser)
        {
            //Get list of all templates from directory in config
            var templatesFromParser = parser.FullReadDatabase();
            //add them to database while checking for duplicates
            int count = templatesFromParser.Count;
            for (int i = 0; i < count; i++)
            {
                if (alltemplates.ContainsKey(templatesFromParser[i].databaseID))
                {
                    Debug.LogError("Database already contains " + templatesFromParser[i].ToString());
                    return;
                }
                else
                {
                    alltemplates.Add(templatesFromParser[i].databaseID, templatesFromParser[i]);
                }
            }
        }

        void fillEntitiesDictionary()
        {
            foreach (var pair in alltemplates)
            {
                var components = new List<IComponent>();
                constructedEntities.Add(pair.Key, components);
                foreach (var comp in pair.Value.components)
                {
                    Type type = Type.GetType(comp.Key);
                    if (type == null)
                        continue;
                    IComponent icomp = ComponentFactory.MakeNew(type);
                    
                    EntityConstructor.AssignFromTemplate(icomp, comp.Value);
                    components.Add(icomp);
                }
            }
        }
    }
}