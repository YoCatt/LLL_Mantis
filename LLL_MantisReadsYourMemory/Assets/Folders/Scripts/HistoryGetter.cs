using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Video;
using System;
using EasyButtons;

public class HistoryGetter
{
    public static string chromeDefaultHistoryDir =>  @"C:\Users\" + GetUsername() + @"\AppData\Local\Google\Chrome\User Data\Default\History";
    public static string braveDefaultHistoryDir =>  @"C:\Users\" + GetUsername() + @"\AppData\Local\BraveSoftware\Brave-Browser\User Data\Default\History";
    public static string foxDefaultHistoryDir =>  @"C:\Users\" + GetUsername() + @"C:\AppData\Roaming\Mozilla\Firefox\Profiles\1pkr17pg.default-release";

    public static string chromeHistoryCopyDir = Application.streamingAssetsPath + "/" + "ChromeHistory";
    public static string braveHistoryCopyDir = Application.streamingAssetsPath + "/" + "BraveHistory";
    public static string foxHistoryCopyDir = Application.streamingAssetsPath + "/" + "FoxHistory";

    public static List<string> historyCopies;

    static string GetUsername()
    {
        return Environment.UserName;
    }

    public static List<string> CopyHistoryFilesThatExist()  // copies history files that exists and returns the string of dirs
    {
        historyCopies = new List<string>();

        //deletes history files if they exist in Streaming Assets:
        System.IO.File.Delete(braveHistoryCopyDir);
        System.IO.File.Delete(chromeHistoryCopyDir);

        if (System.IO.File.Exists(chromeDefaultHistoryDir))
        {
            CopyChromeHistoryFile();
            historyCopies.Add(chromeHistoryCopyDir);
        }

        if (System.IO.File.Exists(braveDefaultHistoryDir))  // DRY VIOLATION 😭😭😭😭
        {
            CopyBraveHistoryFile();
            historyCopies.Add(braveHistoryCopyDir);
        }
        
        if (System.IO.File.Exists(chromeDefaultHistoryDir))
        {
            CopyChromeHistoryFile();
            historyCopies.Add(chromeHistoryCopyDir);
        }

        return historyCopies;
    }

    public static void CopyChromeHistoryFile()  // DRY VIOLATION 😭😭😭😭😭
    {
        try
        {
            File.Copy(chromeDefaultHistoryDir, chromeHistoryCopyDir, true);
        }
        catch (IOException iox)
        {
            Debug.Log(iox.Message);
        }
    }

    public static void CopyBraveHistoryFile()
    {
        try
        {
            File.Copy(braveDefaultHistoryDir, braveHistoryCopyDir, true);
        }
        catch (IOException iox)
        {
            Debug.Log(iox.Message);
        }
    }
}
