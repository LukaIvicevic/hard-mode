using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Slider volumeSlider;

    private void Start()
    {
        volumeSlider.value = SettingsManager.Instance.Volume;
    }

    public void Play()
    {
        SceneManager.LoadScene("BossFight");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ChangeVolume()
    {
        SettingsManager.Instance.ChangeVolume(volumeSlider.value);
    }
}
