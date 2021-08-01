using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using Newtonsoft.Json;
using EasyButtons;


public class StatKeywordResult
{
    public string keyword { get; set; }
    public int instances { get; set; }

    public StatKeywordResult(string _keyword, int count)
    {
        keyword = _keyword;
        instances = count;
    }
}

public class StatsCollector
{
    [Button]
    public static List<StatKeywordResult> GetAllStatKeywordMatches()
    {
        List<StatKeywordResult> masterKeyWordMatches = new List<StatKeywordResult>();
        List<string> historyLocations = HistoryGetter.CopyHistoryFilesThatExist();

        for (int i = 0; i < historyLocations.Count; ++i)
        {
            List<StatKeywordResult> searchTermsWeMeet = GetMatchedStatKeywords(historyLocations[i]);
            AddToMasterMasterKRList(ref masterKeyWordMatches, ref searchTermsWeMeet);
        }

        // foreach (var i in masterKeyWordMatches)
        // {
        //     Debug.Log("Keyword: " + i.keyword + "\t count: " + i.instances);
        // }

        return masterKeyWordMatches;
    }

    static List<StatKeywordResult> GetMatchedStatKeywords(string path)
    {
        List<StatKeywordResult> searchTermsWeMatch = new List<StatKeywordResult>();

        var statKeywords = GetJSONStatKeywords();
        // Open Database
        string connection = "URI=file:" + path;
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        foreach (var keyword in statKeywords)
        {
            IDbCommand cmnd_read = dbcon.CreateCommand();
            IDataReader reader;

            string query = GetQueryFromKeyword(keyword);

            cmnd_read.CommandText = query;
            reader = cmnd_read.ExecuteReader();

            int count = 0;
            while (reader.Read())
                ++count;

            if (count > 0)
            {
                StatKeywordResult kr = new StatKeywordResult(keyword, count);
                searchTermsWeMatch.Add(kr);
            }
        }
        return searchTermsWeMatch;
    }

    static string GetQueryFromKeyword(string keyword)
    {
        string baseQuery = "SELECT * FROM urls WHERE title LIKE '%";
        baseQuery += keyword + "%';";
        return baseQuery;
    }

    static List<string> GetJSONStatKeywords()
    {
        var sr = new StreamReader(Application.streamingAssetsPath + "/" + "StatKeywords.json");
        JsonStatKeywordsClass myDeserializedClass = JsonConvert.DeserializeObject<JsonStatKeywordsClass>(sr.ReadToEnd());
        sr.Close();
        return myDeserializedClass.StatKeywords;
    }

    static void AddToMasterMasterKRList(ref List<StatKeywordResult> masterList, ref List<StatKeywordResult> listToAddToMasterList)
    {
        foreach (var kr in listToAddToMasterList)
        {
            bool exists = false;
            foreach (var mkr in masterList)
            {
                if (kr.keyword == mkr.keyword)    // if already exists in Master List
                {
                    mkr.instances += kr.instances;
                    exists = true;
                    break;
                }
            }
            if (!exists)
                masterList.Add(kr);
        }
    }
}
