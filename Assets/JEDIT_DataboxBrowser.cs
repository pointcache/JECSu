namespace JECSU.Editor
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine.UI;

    [RequireComponent(typeof(JECSU.Editor.JECSUEditor))]
    public class JEDIT_DataboxBrowser : MonoBehaviour
    {

        public GameObject browserGo;
        public RectTransform content;

        JECSU.Editor.JECSUEditor editor;
        GameObject
            rootfolder_prefab,
            folder_prefab,
            databox_prefab;

        string currentFolder;

        void Awake()
        {
            databox_prefab = Resources.Load<GameObject>("JEdit/Elements/DataboxBrowser/databox");
            folder_prefab = Resources.Load<GameObject>("JEdit/Elements/DataboxBrowser/folder");
            rootfolder_prefab = Resources.Load<GameObject>("JEdit/Elements/DataboxBrowser/root_folder");
        }

        public void Toggle(bool state)
        {
            if (state)
                Open();
            else
                Close();
        }

        public void Close()
        {
            browserGo.SetActive(false);
        }

        public void Open()
        {
            browserGo.SetActive(true);
            FillBrowser();
        }

        void FillBrowser()
        {
            editor = GetComponent<JECSU.Editor.JECSUEditor>();
            currentFolder = DataBox.GetDatabasePath();
            populate_browser_from_directory(currentFolder);
        }


        void populate_browser_from_directory(string _path)
        {
            var dir = Directory.GetFileSystemEntries(_path);
            List<string> list = new List<string>();
            content.DestroyChildren();
            foreach (var path in dir)
            {
                if (path.Length < 5)
                    continue;

                string[] split = path.Split('.');
                if (split[split.Length - 1] == "meta")
                    continue;

                list.Add(path);

            }

            GameObject rootfolder = Instantiate(rootfolder_prefab);
            rootfolder.transform.SetParent(content, false);

            string rootfoldername = "";
            if (_path.IndexOf("\\") > 0)
            {
                rootfoldername = _path.Remove(0, _path.IndexOf("\\"+1));
            }
            else
            {
                rootfoldername = "Database";
            }

            rootfolder.transform.FindDeepChild("folder_name").GetComponent<Text>().text = rootfoldername;
            foreach (var path in list)
            {
                if (Directory.Exists(path))
                {
                    string foldername = "";
                    if (path.IndexOf("\\") > 0)
                    {
                        foldername = path.Remove(0, path.IndexOf("\\") +1);
                    }
                    else
                    {
                        foldername = "Database";
                    }

                    GameObject folder = Instantiate(folder_prefab);
                    folder.transform.SetParent(content, false);
                    folder.transform.FindDeepChild("folder_name").GetComponent<Text>().text = foldername;


                }
                else
                    if (File.Exists(path))
                {
                    string[] filesplit = path.Split('.');
                    if (filesplit[filesplit.Length - 1] == "box")
                    {
                        string filename = "";
                        if (path.IndexOf("\\") > 0)
                        {
                            filename = path.Remove(0, path.IndexOf("\\")+1);
                            
                        }
                        else
                        {
                            filename = "Database";
                        }

                        GameObject databox = Instantiate(databox_prefab);
                        databox.transform.SetParent(content, false);
                        databox.transform.FindDeepChild("databox_name").GetComponent<Text>().text = filename;
                    }
                }
            }
        }
    }
}