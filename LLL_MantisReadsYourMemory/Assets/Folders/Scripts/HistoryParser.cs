using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using EasyButtons;
using System;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.Video;

public class HistoryParser
{
    public static List<string> GetSearchTermsWeMeetChrome()
    {
        HistoryGetter.CopyChromeHistory();

        var searchTerms = GetSearchTerms().Searches;
        List<string> searchTermsWeMeet = new List<string>();

        // Open Database
        string connection = "URI=file:" + HistoryGetter.chromeHistoryCopyDir;
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        for (int i = 0; i < searchTerms.Count; ++i)
        {
            IDbCommand cmnd_read = dbcon.CreateCommand();
            IDataReader reader;
            
            string query = GetQueryString(searchTerms[i]);

            cmnd_read.CommandText = query;
            reader = cmnd_read.ExecuteReader();
            
            if (reader.Read())
            {
                searchTermsWeMeet.Add(searchTerms[i].title);
            }
        }
        return searchTermsWeMeet;
    }

    void GetURLSFromChromeHistory()
    {
        HistoryGetter.CopyChromeHistory();

        // Open Database
        string connection = "URI=file:" + HistoryGetter.chromeHistoryCopyDir;
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        // Read and print all values in table
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT * FROM urls WHERE url OR title LIKE '%pornhub.com%';";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        int count = 0;
        while (reader.Read())
        {
            Debug.Log("url: " + reader[1].ToString());
            Debug.Log("title: " + reader[2].ToString());
            ++count;
        }

        // Close connection
        dbcon.Close();
    }


    static string GetQueryString(Search search)
    {
        string baseQuery = "SELECT * FROM urls WHERE title LIKE ";
        var keywords = search.keywords;
        string query = baseQuery;

        query += "'%" + keywords[0] + "%'";

        for (int i = 1; i < keywords.Count; ++i)
        {
            query += " AND title like ";
            query += "'%" + keywords[i] + "%'";
        }
        return query;
    }

    static HistorySearchTermsJson GetSearchTerms()
    {
        var sr = new StreamReader(Application.streamingAssetsPath + "/" + "SearchTerms.json");
        HistorySearchTermsJson myDeserializedClass = JsonConvert.DeserializeObject<HistorySearchTermsJson>(sr.ReadToEnd());
        sr.Close();
        return myDeserializedClass;
    }
}
