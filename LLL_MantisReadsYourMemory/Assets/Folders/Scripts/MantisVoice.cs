using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using EasyButtons;

//Gets assosiated audio clips and plays them
public class MantisVoice : MonoBehaviour
{
    [SerializeField] AudioSource audSrc;
    List<AudioClip> gameAudClips = new List<AudioClip>();
    List<AudioClip> historyAudClips = new List<AudioClip>();

    void OnEnable()
    {
        Invoke("GetAndPlayVoiceClips", 2.5f);
    }

    [Button]
    void DEBUG_MakeSureWeHaveAllAudioClips()
    {
        List<string> games = GamesFinder.GetGamesList();

        for (int i = 0; i < games.Count; ++i)
        {
            string wavPath = Application.streamingAssetsPath + "/" + games[i] + ".wav";

            if (!File.Exists(wavPath))
            {
                print("MISSING AUDIO CLIP: " + games[i]);
            }
        }

        List<Search> searches = HistoryParser.GetJSONSearchTerms();
        for (int i = 0; i < searches.Count; ++i)
        {
            string wavPath = Application.streamingAssetsPath + "/" + searches[i].title + ".wav";

            if (!File.Exists(wavPath))
            {
                print("MISSING AUDIO CLIP: " + searches[i].title);
            }
        }
    }

    [Button]
    void AbbaTestThing()
    {
        HistoryGetter.CopyHistoryFilesThatExist();

        print("Chrome Keyword Result: ");
        List<KeywordResult> chromeResult = HistoryParser.GetSearchTermsWeMeetChrome();
        foreach (var kr in chromeResult)
            kr.Print();

        print("Brave Keyword Result: ");
        List<KeywordResult> braveResult = HistoryParser.GetMatchedSearchTerms(HistoryGetter.braveHistoryCopyDir);
        foreach (var kr in braveResult)
            kr.Print();

        print("Master Keyword Result: ");
        List<KeywordResult> matchedTerms = HistoryParser.GetSearchTermsOfAllBrowsers();
        foreach (var kr in matchedTerms)
            kr.Print();
    }

    [Button]
    void GetAndPlayVoiceClips()
    {
        // play intro audio clip first, then:
        GetAllGameAudioClips();
        GetAllHistoryAudioClips();

        // Shuffle the audio so it's different every time
        historyAudClips.Shuffle();
        gameAudClips.Shuffle();

        // play all voice clips (main part of the whole "game")
        StartCoroutine(VoiceAllClips());
    }

    IEnumerator VoiceAllClips()
    {
        AudioClip introClip = GetAudioFromPath("intro");
        if (introClip)
        {
            audSrc.clip = introClip;
            audSrc.Play();
            yield return new WaitWhile(() => audSrc.isPlaying);
            yield return new WaitForSeconds(0.5f);
        }

        // if over 6, select a random 6 aud clips
        foreach (AudioClip audClip in gameAudClips)
        {
            audSrc.clip = audClip;
            audSrc.Play();
            yield return new WaitWhile(() => audSrc.isPlaying);
            yield return new WaitForSeconds(0.7f);
        }

        AudioClip middle = GetAudioFromPath("MiddleSegway");
        if (middle)
        {
            audSrc.clip = middle;
            audSrc.Play();
            yield return new WaitWhile(() => audSrc.isPlaying);
            yield return new WaitForSeconds(0.5f);
        }

        foreach (AudioClip audClip in historyAudClips)
        {
            audSrc.clip = audClip;
            audSrc.Play();
            yield return new WaitWhile(() => audSrc.isPlaying);
            yield return new WaitForSeconds(0.7f);
        }

        AudioClip outro = GetAudioFromPath("outro");
        if (outro)
        {
            audSrc.clip = outro;
            audSrc.Play();
            yield return new WaitWhile(() => audSrc.isPlaying);
            yield return new WaitForSeconds(0.5f);
        }
    }

    [Button]
    void GetInstancesOfEveryMatchedHistory()
    {
        List<KeywordResult> searchesMatched = HistoryParser.GetSearchTermsWeMeetChrome();
        foreach (var kr in searchesMatched)
        {
            print("Name: " + kr.name + "\t" + "Instances: " + kr.instances);
        }
    }

    [Button]
    AudioClip GetAudioFromPath(string audName)   //without the .wav
    {
        string wavPath = Application.streamingAssetsPath + "/" + audName + ".wav";

        if (File.Exists(wavPath))
        {
            AudioClip audClip = new WWW(wavPath).GetAudioClip(false, true, AudioType.WAV);
            return audClip;
        }
        else
        {
            print("MISSING AUDIO CLIP: " + audName);
            return null;
        }
    }
    
    [Button]
    void GetAllHistoryAudioClips()
    {
        List<KeywordResult> searchesMatched = HistoryParser.GetSearchTermsOfAllBrowsers();

        for (int i = 0; i < searchesMatched.Count; ++i)
        {
            string wavPath = Application.streamingAssetsPath + "/" + searchesMatched[i].name + ".wav";

            if (File.Exists(wavPath))
            {
                AudioClip audClip = new WWW(wavPath).GetAudioClip(false, true, AudioType.WAV);
                historyAudClips.Add(audClip);
            }
            else
            {
                print("MISSING AUDIO CLIP: " + searchesMatched[i].name);
            }
        }
    }

    [Button]
    void GetAllGameAudioClips()
    {
        List<string> gamesInstalled = GamesFinder.GetGamesInstalled();

        for (int i = 0; i < gamesInstalled.Count; ++i)
        {
            string wavPath = Application.streamingAssetsPath + "/" + gamesInstalled[i] + ".wav";

            if (File.Exists(wavPath))
            {
                AudioClip audClip = new WWW(wavPath).GetAudioClip(false, true, AudioType.WAV);
                gameAudClips.Add(audClip);
            }
            else
            {
                print("MISSING AUDIO CLIP: " + gamesInstalled[i]);
            }
        }
    }
}
