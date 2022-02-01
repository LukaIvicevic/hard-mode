using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : SingletonPersistent<SettingsManager>
{
    public float Sensitivity { get; private set; }

    public float Volume { get; private set; }

    public event Action onVolumeChanged;

    private readonly string sensKey = "Sensitivity";
    private readonly string volKey = "Volume";

    protected override void Awake()
    {
        base.Awake();
        Sensitivity = PlayerPrefs.GetFloat(sensKey, 1);
        Volume = PlayerPrefs.GetFloat(volKey, 0.7f);
    }

    public void ChangeVolume(float value)
    {
        Volume = value;
        PlayerPrefs.SetFloat(volKey, value);
        onVolumeChanged?.Invoke();
    }

    public void ChangeSensitivity(float value)
    {
        Sensitivity = value;
        PlayerPrefs.SetFloat(sensKey, value);
    }
}
