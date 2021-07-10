using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Video;
using System;
using EasyButtons;

public class HistoryGetter
{
    public static string chromeHistoryCopyDir = Application.streamingAssetsPath + "/" + "ChromeHistory";
    static string GetUsername()
    {
        return Environment.UserName;
    }

    [Button]
    public static void CopyChromeHistory()
    {
        string sourceFile = @"C:\Users\" + GetUsername() + @"\AppData\Local\Google\Chrome\User Data\Default\History";

        try
        {
            File.Copy(sourceFile, chromeHistoryCopyDir, true);
        }
        catch (IOException iox)
        {
            Debug.Log(iox.Message);
        }
    }
}
