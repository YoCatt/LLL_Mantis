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
        // Invoke("GetAndPlayVoiceClips", 2.5f);
    }

    [Button]
    void GetAndPlayVoiceClips()
    {
        GetAllGameAudioClips();
        gameAudClips.Shuffle();

        // play all voice clips (main part of the whole "game")
        StartCoroutine(VoiceAllClips());
    }

    IEnumerator VoiceAllClips()
    {
        yield return StartCoroutine(PlayAudClip(GetAudioFromPath("intro")));
        
        // if over 6, select a random 6 aud clips
        foreach (AudioClip audClip in gameAudClips) // Play Game aud Clips
            yield return StartCoroutine(PlayAudClip(audClip));

        AudioClip middle = GetAudioFromPath("MiddleSegway");
        yield return StartCoroutine(PlayAudClip(middle));

        yield return StartCoroutine(PlayHistoryClips());

        yield return StartCoroutine(PlayAudClip(GetAudioFromPath("outro")));
    }

    [Button]
    void StartHistoryClipsCourotine()
    {
        StartCoroutine(PlayHistoryClips());
    }

    IEnumerator PlayHistoryClips()
    {
        List<KeywordResult> searchesMatched = HistoryParser.GetSearchTermsOfAllBrowsers();
        searchesMatched.Shuffle();

        foreach (var kr in searchesMatched)  // Play History aud Clips
        {
            AudioClip audClip = GetAudioFromPath(kr.wavFileName);
            print(kr.caption + " " + kr.instances);
            yield return StartCoroutine(PlayAudClip(audClip));
        }
    }

    IEnumerator PlayAudClip(AudioClip audClip)
    {
        if (audClip)
        {
            audSrc.clip = audClip;
            audSrc.Play();
            yield return new WaitWhile(() => audSrc.isPlaying);
            yield return new WaitForSeconds(0.5f);
        }
    }

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
    
    void GetAllHistoryAudioClips()
    {
        List<KeywordResult> searchesMatched = HistoryParser.GetSearchTermsOfAllBrowsers();

        for (int i = 0; i < searchesMatched.Count; ++i)
        {
            string wavPath = Application.streamingAssetsPath + "/" + searchesMatched[i].wavFileName + ".wav";

            if (File.Exists(wavPath))
            {
                AudioClip audClip = new WWW(wavPath).GetAudioClip(false, true, AudioType.WAV);
                historyAudClips.Add(audClip);
            }
            else
            {
                print("MISSING AUDIO CLIP: " + searchesMatched[i].wavFileName);
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
