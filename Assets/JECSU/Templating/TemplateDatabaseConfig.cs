namespace JECSU.Serialization
{

    using System;

    /// <summary>
    /// This class represents the config of the database itself and is loaded from disc in root folder of the project in Database.xml
    /// </summary>
    [Serializable]
    public class TemplateDatabaseConfig
    {
        public string rootPath = "default";
    }
}