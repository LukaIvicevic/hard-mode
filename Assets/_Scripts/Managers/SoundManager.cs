using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{

    [SerializeField]
    private AudioClip ambience;

    private AudioSource globalSource;

    private float volume = 1;

    private void Start()
    {
        PlayAmbientMusic();
    }

    private void PlayAmbientMusic()
    {
        globalSource = GetComponent<AudioSource>();
        if (globalSource == null)
        {
            Logger.Instance.LogWarning("GlobalSource not set on SoundManager");
            return;
        }
        globalSource.loop = true;
        globalSource.clip = ambience;
        globalSource.Play();
    }

    public void PlayOneShot(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.volume = volume;
        audioSource.PlayOneShot(audioClip);
    }
}
