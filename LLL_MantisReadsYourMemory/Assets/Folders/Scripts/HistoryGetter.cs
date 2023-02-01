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
    public static string gxDefaultHistoryDir =>  @"C:\Users\" + GetUsername() + @"\AppData\Roaming\Opera Software\Opera GX Stable\History";
    public static string operaDefaultHistoryDir =>  @"C:\Users\" + GetUsername() + @"\AppData\Roaming\Opera Software\Opera Stable\History";
    public static string edgeDefaultHistoryDir =>  @"C:\Users\" + GetUsername() + @"\AppData\Local\Microsoft\Edge\User Data\Default\History";

    public static string chromeHistoryCopyDir = Application.streamingAssetsPath + "/" + "ChromeHistory";
    public static string braveHistoryCopyDir = Application.streamingAssetsPath + "/" + "BraveHistory";
    public static string gxHistoryCopyDir = Application.streamingAssetsPath + "/" + "GxHistory";
    public static string operaHistoryCopyDir = Application.streamingAssetsPath + "/" + "OperaHistory";
    public static string edgeHistoryCopyDir = Application.streamingAssetsPath + "/" + "EdgeHistory";

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
        System.IO.File.Delete(gxHistoryCopyDir);
        System.IO.File.Delete(edgeHistoryCopyDir);
        System.IO.File.Delete(operaHistoryCopyDir);

        // copies the browser history file and adds it to the historyCopies list
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
        
        if (System.IO.File.Exists(gxDefaultHistoryDir))
        {
            CopyGxHistoryFile();
            historyCopies.Add(gxHistoryCopyDir);
        }
        
        if (System.IO.File.Exists(edgeDefaultHistoryDir))
        {
            CopyEdgeHistoryFile();
            historyCopies.Add(edgeHistoryCopyDir);
        }
        
        if (System.IO.File.Exists(operaDefaultHistoryDir))
        {
            CopyOperaHistoryFile();
            historyCopies.Add(operaHistoryCopyDir);
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
    
    public static void CopyGxHistoryFile()
    {
        try
        {
            File.Copy(gxDefaultHistoryDir, gxHistoryCopyDir, true);
        }
        catch (IOException iox)
        {
            Debug.Log(iox.Message);
        }
    }
    
    public static void CopyEdgeHistoryFile()
    {
        try
        {
            File.Copy(edgeDefaultHistoryDir, edgeHistoryCopyDir, true);
        }
        catch (IOException iox)
        {
            Debug.Log(iox.Message);
        }
    }
    
    public static void CopyOperaHistoryFile()
    {
        try
        {
            File.Copy(operaxDefaultHistoryDir, operaHistoryCopyDir, true);
        }
        catch (IOException iox)
        {
            Debug.Log(iox.Message);
        }
    }
}
