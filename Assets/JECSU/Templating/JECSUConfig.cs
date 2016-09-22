namespace JECSU.Serialization
{

    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using UnityEngine;
    /// <summary>
    /// This class represents the config of the database itself and is loaded from disc in root folder of the project in Database.xml
    /// </summary>
    [Serializable]
    public class JECSUConfig
    {
        public string templateDatabasePath = "default";
        public string databoxDatabasePath = "default";


        private static JECSUConfig _current;
        public static JECSUConfig current
        {
            get
            {
                if (_current != null)
                    return _current;
                string path = Application.dataPath + "/JecsuConfig.xml";
                var serializer = new XmlSerializer(typeof(JECSUConfig));

                if (File.Exists(path))
                {
                    using (var fStream = new FileStream(path, FileMode.Open))
                        _current = (JECSUConfig)serializer.Deserialize(fStream);
                }

                else
                {
                    _current = new JECSUConfig();

                    using (XmlTextWriter tw = new XmlTextWriter(path, Encoding.UTF8))
                    {
                        tw.Formatting = Formatting.Indented;
                        serializer.Serialize(tw, current);
                    }
                }

                return _current;
            }
        }
    }
}