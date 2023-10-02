using System.Collections.Generic;
using UnityEngine;
using System.IO;

using Newtonsoft.Json;

public class GamesFinder
{
    // common drives
    static string steamDir = @"C:\Program Files (x86)\Steam\steamapps\common\";
    static string epicDir = @"C:\Program Files\Epic Games\";
    static string ubisoftDir = @"C:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\games\";
    static string D_steamDir = @"D:\SteamLibrary\steamapps\common\";
    static string D_epicDir = @"D:\My Games";
    static string E_steamDir = @"E:\SteamLibrary\steamapps\common\";
    static string E_epicDir = @"E:\My Games";
    static string F_steamDir = @"F:\SteamLibrary\steamapps\common\";
    static string F_epicDir = @"F:\My Games";

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

    static bool CheckIfGameExists(string game)
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
