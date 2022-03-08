using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonPersistent<SoundManager>
{
    [SerializeField]
    private AudioClip ambience;

    [SerializeField]
    private AudioClip music;

    [SerializeField]
    private AudioSource ambienceSource;

    [SerializeField]
    private AudioSource musicSource;

    private void Start()
    {
        PlayAmbientMusic();
        SettingsManager.Instance.OnVolumeChanged += VolumeChanged;
    }

    private void OnDestroy()
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.OnVolumeChanged -= VolumeChanged;
        }
    }

    public void PlayOneShot(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.volume = SettingsManager.Instance.Volume;
        audioSource.PlayOneShot(audioClip);
    }

    public void PlayLoop(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.volume = SettingsManager.Instance.Volume;
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.Play();
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

    public void PlayBossMusic()
    {
        if (musicSource == null)
        {
            Logger.Instance.LogWarning("MusicSource not set on SoundManager");
            return;
        }

        if (!musicSource.isPlaying)
        {
            musicSource.loop = true;
            musicSource.clip = music;
            musicSource.volume = SettingsManager.Instance.Volume;
            musicSource.Play();
        }
    }

    private void PlayAmbientMusic()
    {
        if (ambienceSource == null)
        {
            Logger.Instance.LogWarning("AmbienceSource not set on SoundManager");
            return;
        }
        ambienceSource.loop = true;
        ambienceSource.clip = ambience;
        ambienceSource.volume = SettingsManager.Instance.Volume;
        ambienceSource.Play();
    }

    private void VolumeChanged()
    {
        ambienceSource.volume = SettingsManager.Instance.Volume;
        musicSource.volume = SettingsManager.Instance.Volume;
    }
}
