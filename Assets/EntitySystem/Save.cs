using UnityEngine;
using System;
using System.Collections.Generic;
using EntitySystem;
using EntitySystem.Components;
using FullSerializer;
using System.IO;

/// <summary>
/// About: performs save, uses FullSerializer for that
/// How to: 
///     Add "Serializeable" component on entities you need saved.
///     Set path to savefile
///     Use SaveState();
/// </summary>
public class Save : MonoBehaviour
{
    /// <summary>
    /// save file path
    /// </summary>
    static string relativeFolder = "/Save/savefile.sav";

    /// <summary>
    /// Use this to save all entities that have "Serializeable" Component on them.
    /// </summary>
    public void SaveGame()
    {
        SaveState();
    }

    public static void SaveState()
    {
        List<Entity> tosave = Matcher.Is<Serializeable>();

        string path = Application.dataPath + relativeFolder;

        Serialize(tosave, path, true);
    }

    static bool Serialize(object obj, string path, bool beautify)
    {
        fsSerializer _serializer = new fsSerializer();
        fsData data;
        _serializer.TrySerialize(obj, out data).AssertSuccessWithoutWarnings();
        StreamWriter sw = new StreamWriter(path);
        switch (beautify)
        {
            case true:
                sw.Write(fsJsonPrinter.PrettyJson(data));
                break;
            case false:
                sw.Write(fsJsonPrinter.CompressedJson(data));
                break;
        }

        sw.Close();
        return true;
    }
}
