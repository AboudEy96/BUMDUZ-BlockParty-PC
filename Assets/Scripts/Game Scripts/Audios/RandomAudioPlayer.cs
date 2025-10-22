using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomAudioPlayer : MonoBehaviour
{
public List<AudioClip> audioClips;
[SerializeField]private AudioSource _audioSource;
public static RandomAudioPlayer Instance;

private List<AudioClip> audioBackup = new List<AudioClip>();

public static bool PausedOfBlocksDestroy = false;

private void Start()
{
    
    if (Instance == null)
    {
        Instance = this;
    }
    else
    {
        Destroy(gameObject);
    }
    
    audioBackup.AddRange(audioClips);
    int randomIndex = Random.Range(0, audioClips.Count);
    _audioSource.clip = audioClips[randomIndex];
    _audioSource.Play();
    audioClips.RemoveAt(randomIndex);
}

private void Update()
{
    if (audioClips.Count == 0)
    {
        audioClips.AddRange(audioBackup);
    }
    if (!_audioSource.isPlaying && !PausedOfBlocksDestroy)
    {
        int randomIndex = Random.Range(0, audioClips.Count);
        _audioSource.clip = audioClips[randomIndex];
        _audioSource.Play();   
        audioClips.RemoveAt(randomIndex);
    }
}

public static void PauseResumeAudio()
{
    if (Instance == null) return;
    
    bool aa = Instance._audioSource.isPlaying;
    if (aa)
    {
        Instance._audioSource.Pause();
    }
    else
    {
        Instance._audioSource.UnPause();
    }
    
}
}