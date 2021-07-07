using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using EasyButtons;
using System;
using UnityEngine.UI;

using Newtonsoft.Json;
using UnityEngine.Video;

public class JsonGamesClass
{
    public List<string> games { get; set; }
}

public class MindReader : MonoBehaviour
{
    static string steamDir = @"C:\Program Files (x86)\Steam\steamapps\common\";
    static string epicDir = @"C:\Program Files\Epic Games";
    static string ubisoftDir = @"C:\Program Files\Epic Games";
    
    [SerializeField] Text txt;
    List<string> gamesInstalled;

    void Start()
    {
        gamesInstalled = GetGamesInstalled();
        WriteGamesToTXTUI();
    }

    void WriteGamesToTXTUI()
    {
        for (int i = 0; i < gamesInstalled.Count; ++i)
        {
            txt.text += gamesInstalled[i];
            txt.text += ",\n";
        }
    }

    public static List<string> GetGamesInstalled()
    {
        List<string> gamesInstalled = new List<string>();
        List<string> games = GetGamesList();
        foreach (string game in games)
        {
            if (CheckIfGameExists(game))
                gamesInstalled.Add(game);
        }
        return gamesInstalled;
    }

    public static List<string> GetGamesList()
    {
        var sr = new StreamReader(Application.streamingAssetsPath + "/" + "games.json");
        JsonGamesClass myDeserializedClass = JsonConvert.DeserializeObject<JsonGamesClass>(sr.ReadToEnd());
        sr.Close();
        return myDeserializedClass.games;
    }

    public static bool CheckIfGameExists(string game)
    {
        if (Directory.Exists(steamDir + game))
            return true;

        if (Directory.Exists(ubisoftDir + game))
            return true;

        if (Directory.Exists(epicDir + game))
            return true;

        return false;
    }
}
