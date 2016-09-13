namespace JECSU {

    using UnityEngine;
    using System.Xml.Serialization;
    using System.IO;
    using System.Xml;
    using System.Text;
    using JECSU.Serialization;
    using System.Collections.Generic;

    /// <summary>
    /// This objects represents database of entities that can be constructed later, it doesnt contain actual Entity objects or their component, only their Templates
    /// </summary>
    public class TemplateDatabase {

        TemplateDatabaseConfig cfg;
        TemplateDatabaseParser parser;

        //key = templateID
        Dictionary<string, EntityTemplate> alltemplates = new Dictionary<string, EntityTemplate>();

        
        public void Initialize()
        {
            cfg = getConfig();
            parser = new TemplateDatabaseParser();
            parser.Initialize(cfg);

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

        /// <summary>
        /// tries to find alread existing xml config file, if none - creates new one
        /// </summary>
        /// <returns></returns>
        TemplateDatabaseConfig getConfig()
        {
            string path = Application.dataPath + "/Database.xml";
            var serializer = new XmlSerializer(typeof(TemplateDatabaseConfig));

            if (File.Exists(path))
            {
                using (var fStream = new FileStream(path, FileMode.Open))
                    cfg = (TemplateDatabaseConfig)serializer.Deserialize(fStream);
            }

            else
            {
                cfg = new TemplateDatabaseConfig();

                using (XmlTextWriter tw = new XmlTextWriter(path, Encoding.UTF8))
                {
                    tw.Formatting = Formatting.Indented;
                    serializer.Serialize(tw, cfg);
                }
            }

            return cfg;
        }
    }
}