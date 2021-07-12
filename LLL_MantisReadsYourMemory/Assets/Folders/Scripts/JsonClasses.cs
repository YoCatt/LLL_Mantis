using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Internet Searches:

// HistorySearchTermsJson myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
public class Search
{
    public string title { get; set; }
    public List<string> keywords { get; set; }
    public string query => HistoryParser.GetQueryString(this);
}

public class HistorySearchTermsJson
{
    public List<Search> Searches { get; set; }
}


//Games
public class JsonGamesClass
{
    public List<string> games { get; set; }
}