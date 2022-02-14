using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Slider volumeSlider;

    [SerializeField]
    private TextMeshProUGUI highestDifficultyText;

    private string highestDifficultyKey = "HighestDifficulty";

    private void Start()
    {
        volumeSlider.value = SettingsManager.Instance.Volume;
        highestDifficultyText.text = PlayerPrefs.GetFloat(highestDifficultyKey, 0).ToString();
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
