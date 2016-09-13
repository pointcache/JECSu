namespace JECSU.Serialization
{
    using System;
    using System.Collections.Generic;
    using JECSU.Components;

    /// <summary>
    /// This stores deserialized data for later use by Entity Constructor
    /// </summary>
    public class EntityTemplate
    {
        public string databaseID;
        public string parentID; 
        public string Name;

        public string filepath;
        public int fileLine;

        public Dictionary<string, Dictionary<string, string>> components = new Dictionary<string, Dictionary<string, string>>();

        public override string ToString()
        {
            return "EntityTemplate|Name:" + Name + "|db_id:" + databaseID + "|parentID:" + parentID + "|file:" + filepath + "|line:"+ fileLine;
        }
    }
}