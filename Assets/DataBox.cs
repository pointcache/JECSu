namespace JECSU
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    public class DataBox
    {
        public string ID;
        public List<Entity> entities = new List<Entity>();

        public static string GetDatabasePath()
        {
            string path = JECSU.Serialization.JECSUConfig.current.databoxDatabasePath;

            if (path == "default")
                return Application.dataPath + "/Database";
            else
                return path;
        }
    }

    public class DataboxManager
    {
        string databoxDirectoryPath;

        public List<DataBox> databoxes = new List<DataBox>();
        public Dictionary<string, DataBox> databoxesById = new Dictionary<string, DataBox>();

        public DataboxManager(string rootpath)
        {

        }


    }
}