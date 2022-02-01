using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonPersistent<SoundManager>
{
    [SerializeField]
    private AudioClip ambience;

    private AudioSource globalSource;

    private void Start()
    {
        PlayAmbientMusic();
        SettingsManager.Instance.onVolumeChanged += VolumeChanged;
    }

    public void PlayOneShot(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.volume = SettingsManager.Instance.Volume;
        audioSource.PlayOneShot(audioClip);
    }

    public void PlayNoOverlap(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.volume = SettingsManager.Instance.Volume;
        audioSource.clip = audioClip;
        if (audioSource.isPlaying)
        {
            return;
        }

        audioSource.Play();
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
        globalSource.volume = SettingsManager.Instance.Volume;
        globalSource.Play();
    }

    private void VolumeChanged()
    {
        globalSource.volume = SettingsManager.Instance.Volume;
    }
}
