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
        Invoke("GetAndPlayVoiceClips", 4.2f);
    }

    [Button]
    void Abba()
    {
        List<KeywordResult> matchedTerms = HistoryParser.GetSearchTermsOfAllBrowsers();
        foreach (var kr in matchedTerms)
        {
            print("Matched Keyword Name: " + kr.name + "\t" + "Instances: " + kr.instances);
        }
    }

    [Button]
    void GetAndPlayVoiceClips()
    {
        // play intro audio clip first, then:
        GetAllGameAudioClips();
        GetAllHistoryAudioClips();
        StartCoroutine(VoiceAllClips());
        // play all internet voice clips
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
    void GetAllHistoryAudioClips()
    {
        List<KeywordResult> searchesMatched = HistoryParser.GetSearchTermsWeMeetChrome();

        for (int i = 0; i < searchesMatched.Count; ++i)
        {
            string wavPath = Application.streamingAssetsPath + "/" + searchesMatched[i] + ".wav";

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

    IEnumerator VoiceAllClips()
    {
        // if over 6, select a random 6 aud clips
        foreach (AudioClip audClip in gameAudClips)
        {
            audSrc.clip = audClip;
            audSrc.Play();
            yield return new WaitWhile(() => audSrc.isPlaying);
            yield return new WaitForSeconds(0.7f);
        }

        foreach (AudioClip audClip in historyAudClips)
        {
            audSrc.clip = audClip;
            audSrc.Play();
            yield return new WaitWhile(() => audSrc.isPlaying);
            yield return new WaitForSeconds(0.7f);
        }
    }
}
