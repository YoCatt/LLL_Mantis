using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;
using System;
using EasyButtons;

//Gets assosiated audio clips and plays them
public class MantisVoice : MonoBehaviour
{
    [SerializeField] AudioSource audSrc;
    List<AudioClip> audClips = new List<AudioClip>();

    void OnEnable()
    {
        Invoke("GetAndPlayVoiceClips", 4.2f);
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
    void GetAllHistoryAudioClips()
    {
        List<string> searchesMatched = HistoryParser.GetSearchTermsWeMeetChrome();

        for (int i = 0; i < searchesMatched.Count; ++i)
        {
            string wavPath = Application.streamingAssetsPath + "/" + searchesMatched[i] + ".wav";

            if (File.Exists(wavPath))
            {
                AudioClip audClip = new WWW(wavPath).GetAudioClip(false, true, AudioType.WAV);
                audClips.Add(audClip);
            }
            else
            {
                print("MISSING AUDIO CLIP: " + searchesMatched[i]);
            }
        }
    }

    void GetAllGameAudioClips()
    {
        List<string> gamesInstalled = GamesFinder.GetGamesInstalled();

        for (int i = 0; i < gamesInstalled.Count; ++i)
        {
            string wavPath = Application.streamingAssetsPath + "/" + gamesInstalled[i] + ".wav";

            if (File.Exists(wavPath))
            {
                AudioClip audClip = new WWW(wavPath).GetAudioClip(false, true, AudioType.WAV);
                audClips.Add(audClip);
            }
        }
    }

    IEnumerator VoiceAllClips()
    {
        foreach (AudioClip audClip in audClips)
        {
            audSrc.clip = audClip;
            audSrc.Play();
            yield return new WaitWhile(() => audSrc.isPlaying);
            yield return new WaitForSeconds(0.7f);
        }
    }
}
