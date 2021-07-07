using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;
using System;
using EasyButtons;

public class MantisVoice : MonoBehaviour
{
    [SerializeField] AudioSource audSrc;
    List<AudioClip> audClips = new List<AudioClip>();

    [Button]
    void GetAndPlayVoiceClips()
    {
        GetAllGameAudioClips();
        StartCoroutine(VoiceAllClips());
    }

    void GetAllGameAudioClips()
    {
        List<string> gamesInstalled = MindReader.GetGamesInstalled();

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
