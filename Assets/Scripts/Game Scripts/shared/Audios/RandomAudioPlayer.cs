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
private bool isPaused;
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

public void setVolume(float value)
{
    _audioSource.volume = value;
}

public static void PauseResumeAudio()
{
    if (Instance == null) return;

    if (PausedOfBlocksDestroy)
    {
        if (Instance._audioSource.isPlaying)
        {
            Instance._audioSource.Pause();
        }
    }
    else
    {
        if (!Instance._audioSource.isPlaying)
        {
            Instance._audioSource.UnPause();
        }
    }
}
}