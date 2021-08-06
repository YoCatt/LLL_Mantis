using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class DEBUG_UI_MoreBrowserStats : MonoBehaviour
{
    [SerializeField] GameObject entryPrefab;
    [SerializeField] Transform contentTransf;

    // Start is called before the first frame update
    void OnEnable()
    {
        DisplayAllMatches();
    }

    [Button]
    void DEBUG_Spawn5()
    {
        for (int i = 0; i < 5; ++i)
        {
            SpawnEntry("abba", 999);
        }
    }

    [Button]
    void DisplayAllMatches()
    {
        var abba = HistoryParser.GetSearchTermsOfAllBrowsers();
        foreach (var i in abba)
        {
            SpawnEntry(i.wavFileName, i.instances);
        }
    }

    void SpawnEntry(string keyword, int count)
    {
        // this should be inside StatEntry... im retarded
        var spawned = Instantiate(entryPrefab, contentTransf).GetComponent<StatEntry>();
        spawned.countTxt.text = count.ToString();
        spawned.keywordTxt.text = keyword;
    }
}
