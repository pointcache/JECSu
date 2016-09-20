namespace JECSU.Serialization
{
    using UnityEngine;
    using System.IO;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq;

    /// <summary>
    /// Class specifically designed to work with JECSu entities and components.
    /// Reads database directory on disk into template list
    /// </summary>
    public class TemplateDatabaseParser 
    {

        public string directory { get; private set; }
        const string  fileformat = "*.jdb";
       

        JECSUConfig cfg;

        public void Initialize(JECSUConfig config)
        {
            cfg = config;

            if (cfg.templateDatabasePath == "default")
                directory = Application.dataPath + "/TemplateDatabase";
        }

        public List<EntityTemplate>  FullReadDatabase()
        {
            //collect files
            var files = Directory.GetFiles(directory , fileformat, SearchOption.AllDirectories).ToList();

            List<EntityTemplate> fulltemplatelist = new List<EntityTemplate>(1000);

            foreach (var file in files)
            {
                var templates = ParseFile(file);

                fulltemplatelist.AddRange(templates);
            }

            return fulltemplatelist;
        }

        //while parsing we check the line type ahead of time
        enum LineType
        {
            entity,
            component,
            data,
            empty
        }

        /// <summary>
        /// Parses while database director
        /// </summary>
        List<EntityTemplate> ParseFile(string path)
        {
            //create new helper object
            DBJobData errordata = new DBJobData();
            //extract all templates in that file
            var templates = _getTemplatesFromFile( errordata, path);
            //error handling
            if (errordata.hasError)
            {
                Debug.Log(errordata.error + " in file:" + errordata.path + " at line:" + errordata.line);
            }
            //print out warnings
            foreach (var war in errordata.warnings)
            {
                Debug.Log(war + " in file:" + errordata.path + " at line:" + errordata.line);
            }

            return templates;
        }

        /// <summary>
        /// Object for storing errors/warning/other data
        /// </summary>
        class DBJobData
        {
            public string path;
            public int line;
            string _error;
            public string error
            {
                get { return _error; }
                set { _error = value; hasError = true; }
            }
            public bool hasError;
            public List<string> warnings = new List<string>();
        }

        /// <summary>
        /// Main worker for parsing files
        /// </summary>
        /// <param name="jobdata"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        List<EntityTemplate> _getTemplatesFromFile(DBJobData jobdata, string filepath)
        {
            jobdata.path = filepath;
            jobdata.line = 0;
            List<EntityTemplate> templatesInFile = new List<EntityTemplate>();

            //we go through the file line by line
            using (FileStream fs = new FileStream(filepath,
                                           FileMode.OpenOrCreate,
                                           FileAccess.ReadWrite,
                                           FileShare.None))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    //objects that hold last parsed template/component
                    EntityTemplate currentTemplate = null;
                    Dictionary<string, string> currentComponent = null;
                    
                    while (!sr.EndOfStream)
                    {
                        //read line and check its type
                        string line = sr.ReadLine();
                        jobdata.line++;
                        var linetype = checkLine(line);

                        
                        switch (linetype)
                        {
                            case LineType.entity:
                                currentTemplate = parseTemplateDeclaration(jobdata, line);
                                templatesInFile.Add(currentTemplate);
                                break;
                            case LineType.component:
                                currentComponent =  parseComponent(jobdata, line, currentTemplate);
                                break;
                            case LineType.data:
                                if(currentComponent != null)
                                parseData(jobdata, currentComponent, line);
                                break;
                            case LineType.empty:
                                continue;
                            default:
                                break;
                        }
                    }
                }
            }
            return templatesInFile; 
        }

        /// <summary>
        /// Returns detected line type
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        static LineType checkLine(string line)
        {
            //for empty
            string emptycheck = line.Replace("\t", "");
            emptycheck = emptycheck.Replace(" ", "");
            if (emptycheck.Length < 3)
                return LineType.empty;

            //if comment
            if (line.Contains('/'))
            {
                int slash = line.IndexOf('/');
                if(line.Length - 1 > slash)
                {
                    if (line[slash + 1] == '/')
                        return LineType.empty;
                }
            }
            //if the line has one TAB, its a component
            //if 2 TABS its data
            if (line[0] == '\t' || checkForChar(line,0, 4, ' '))
            {
                if(line[1] == '\t' || checkForChar(line,4, 8, ' '))
                {
                    if(line.Contains('='))
                        return LineType.data;
                }
                if(line.Contains('['))
                    return LineType.component;
            }

            return LineType.entity;
        }

        static void parseData(DBJobData jobdata, Dictionary<string,string> component, string line)
        {
            if (!line.Contains("="))
            {
                jobdata.error = "Data/value pair without '=' = syntax error.";
            }
            string[] splitEq = line.Split('=');

            string left = splitEq[0].Replace("\t", string.Empty);
            left = left.Replace(" ", string.Empty);
            string right = getString(jobdata, splitEq[1]);

            if (component.ContainsKey(left))
            {
                jobdata.error = "Duplicate data found, this is not permitted.";
            }
            else
            {
                component.Add(left, right);
            }
        }

        /// <summary>
        /// tries to get out a string that is surrounded by quotes, if not removes all spaces and returns result
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        static string getString(DBJobData jobdata, string start)
        {
            string result = "";
            if (start.Contains('"')){
                int count = 0;
                int indx_start = 0;
                int indx_end = 0;
               
                for (int i = 0; i < start.Length; i++)
                {
                    if(start[i] == '"')
                    {
                        count++;
                        if(count == 1)
                            indx_start = i;
                        
                        if (count == 2)
                            indx_end = i;
                    }
                }
                if (count == 2)
                {
                    result = start.Remove(indx_end, start.Length - indx_end);
                    result = result.Remove(0, indx_start + 1);
                }
                if(count == 1 || count > 2)
                {
                    jobdata.error = "Syntax error, check quotes in data/value ";
                }
            }
            else
            {
                result = start.Replace(" ", string.Empty);
            }

            return result;
        }

        /// <summary>
        /// returns true if from start of string til charcount all chars are "tocheck" in string str
        /// </summary>
        /// <param name="str"></param>
        /// <param name="count"></param>
        /// <param name="tocheck"></param>
        /// <returns></returns>
        static bool checkForChar(string str,int start, int count, char tocheck)
        {
            if (str.Length < count)
                return false;
            for (int i = 0; i < count; i++)
            {
                if (str[i] != tocheck)
                    return false;
            }
            return true;
        }

        static Dictionary<string, string> parseComponent(DBJobData jobdata, string line, EntityTemplate template)
        {
            string component = "";
            if (line.Contains('[') && line.Contains(']'))
            {
                int start = line.IndexOf('[');
                int end = line.IndexOf(']');
                char[] name = new char[end - start - 1];

                int count = 0;
                for (int i = start + 1; i < end; i++)
                {
                    name[count] = line[i];
                    count++;
                }
                component = new string(name);
            }
            else
            {
                jobdata.error = "Syntax error, check brackets in component declaration declaration";
            }
            //if (!componentExists(component))
            //{
            //    jobdata.warnings.Add("Component [" + component + "] not found in system, make sure you spelled it right."); 
            //}

            if (template.components.ContainsKey(component))
            {
                jobdata.error = "Duplicate component found, this is not allowed.";
            }
            else
            {
                template.components.Add(component, new Dictionary<string, string>());
                return template.components[component];
            }

            return null;
        }

        

        static EntityTemplate parseTemplateDeclaration(DBJobData jobdata, string line)
        {
            EntityTemplate template = new EntityTemplate();
            template.fileLine = jobdata.line;
            template.filepath = jobdata.path;
            string name = "NoNameEntity";
            string IDandParent = "";
            bool hasName = false;
            if (line.Contains('"')){
                int count = 0;
                int indx_start = 0;
                int indx_end = 0;
               
                for (int i = 0; i < line.Length; i++)
                {
                    if(line[i] == '"')
                    {
                        count++;
                        if(count == 1)
                            indx_start = i;
                        
                        if (count == 2)
                            indx_end = i;
                    }
                }
                if (count == 2)
                {
                    name = line.Remove(indx_end, line.Length - indx_end);
                    name = name.Remove(0, indx_start + 1);
                    hasName = true;
                }
                if(count == 1 || count > 2)
                {
                    jobdata.error = "Syntax error, check quotes in entity declaration";
                }
            }
            if (hasName)
            {
                IDandParent = line.Replace("\"" + name + "\"", string.Empty);
            }
            else
                IDandParent = line;

            template.Name = name;
            string[] split = IDandParent.Split(' ');
            
            bool foundID = false;
            bool foundParentID = false;
            for (int i = 0; i < split.Length; i++)
            {
                if (split[i].Length == 0)
                    continue;
                else
                {
                    if (!foundID)
                    {
                        foundID = true;
                        template.databaseID = split[i];
                    }
                    else
                    if(!foundParentID)
                    {
                        foundParentID = true;
                        template.parentID = split[i];
                    }
                    else
                    {
                        jobdata.error = "Syntax error, too much members on entity declaration!";
                    }
                }
            }

            return template;
        }

       
    }
}