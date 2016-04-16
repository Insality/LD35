using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SoundController for easy sound and music controls for 2d game
/// Author: Insality
/// </summary>

public class SoundController: MonoBehaviour
{
    public SoundData[] Sounds;
    public MusicData[] Musics;
    public AudioSource Audio;
    private static Dictionary<SoundType, AudioClip> _soundsDict = new Dictionary<SoundType, AudioClip>();
    private static Dictionary<MusicType, AudioClip> _musicDict = new Dictionary<MusicType, AudioClip>();
    private static AudioSource _audio;

    private static bool _isSoundOn;
    private static bool _isMusicOn;
    private static MusicType _lastMusic;
    private static bool _canPlayMusic;

    void Start()
    {
        LoadSoundsToStatic();
        LoadMusicsToStatic();
        _audio = Audio;
    }

    public static void SetAudioSettings(bool isSoundOn, bool isMusicOn)
    {
        _isSoundOn = isSoundOn;
        _isMusicOn = isMusicOn;
        if (_canPlayMusic && _isMusicOn)
        {
            PlayMusic(_lastMusic);
        }
        else
        {
            StopMusic();
        }
    }

    private void LoadSoundsToStatic()
    {
        if (Sounds != null)
        {
            for (int i = 0; i < Sounds.Length; i++)
            {
                _soundsDict.Add(Sounds[i].Type, Sounds[i].Clip);
            }
        }
        else
        {
            Debug.Log("[Error]: No Sounds Initialized");
        }
    }
    

    private void LoadMusicsToStatic()
    {
        if (Musics != null)
        {
            for (int i = 0; i < Musics.Length; i++)
            {
                _musicDict.Add(Musics[i].Type, Musics[i].Clip);
            }
        }
        else
        {
            Debug.Log("[Error]: No Musics Initialized");
        }
    }


    public static void PlaySound(SoundType type)
    {
        if (_isSoundOn)
        {
            if (_soundsDict[type] != null)
            {
                AudioSource.PlayClipAtPoint(_soundsDict[type], Vector3.zero, 1f);
            }
            else
            {
                Debug.Log("The SoundType " + type + " is null");
            }
        }
    }


    public static void PlayMusic(MusicType type) {
        _lastMusic = type;
        if (_isMusicOn)
        {
            if (_musicDict[type] != null)
            {
                _audio.Stop();
                _audio.clip = _musicDict[type];
                _audio.loop = true;
                _audio.spatialBlend = 0;
                _audio.Play();
                _canPlayMusic = true;
            }
            else
            {
                Debug.Log("The MusicType " + type + " is null");
            }
        }
    }


    public static void StopMusic()
    {
        _audio.Stop();
    }


    public static void SetPitch(float pitch)
    {
        _audio.pitch = pitch;
    }

}

[Serializable]
public class SoundData
{
    public SoundType Type;
    public AudioClip Clip;
}

[Serializable]
public class MusicData {
    public MusicType Type;
    public AudioClip Clip;
}



public enum SoundType
{
    TestKick = 0,
    TestKick2 = 1,
}


public enum MusicType
{
    GameMusic = 0,
}
