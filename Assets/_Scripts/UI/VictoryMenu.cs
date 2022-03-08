using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VictoryMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private GameObject hud;

    [SerializeField]
    private TextMeshProUGUI highestDifficultyText;

    private string highestDifficultyKey = "HighestDifficulty";

    public void PlayerWins()
    {
        PlayerPrefs.SetFloat(highestDifficultyKey, GameManager.Instance.GetDifficulty());
        highestDifficultyText.text = PlayerPrefs.GetFloat(highestDifficultyKey, 0).ToString();
        Pause();
    }

    public void Resume()
    {
        menu.SetActive(false);
        hud.SetActive(true);
        Time.timeScale = 1;
        GameManager.Instance.LockCursor();
        GameManager.Instance.IsPaused = false;
        GameManager.Instance.IsWinScreenOpen = false;
    }

    public void Restart()
    {
        Resume();
        GameManager.Instance.Restart();
    }

    public void Quit()
    {
        GameManager.Instance.Quit();
    }

    private void Pause()
    {
        menu.SetActive(true);
        hud.SetActive(false);
        Time.timeScale = 0;
        GameManager.Instance.UnlockCursor();
        GameManager.Instance.IsPaused = true;
        GameManager.Instance.IsWinScreenOpen = true;
    }
}
