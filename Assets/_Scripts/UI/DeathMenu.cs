using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private GameObject hud;

    public void PlayerDied()
    {
        Pause();
    }

    public void Resume()
    {
        menu.SetActive(false);
        hud.SetActive(true);
        Time.timeScale = 1;
        GameManager.Instance.LockCursor();
        GameManager.Instance.IsPaused = false;
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
    }
}
