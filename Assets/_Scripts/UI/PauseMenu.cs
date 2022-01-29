using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject hud;

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TogglePauseMenu();
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        hud.SetActive(true);
        Time.timeScale = 1;
        GameManager.Instance.LockCursor();
        GameManager.Instance.isPaused = false;
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

    private void TogglePauseMenu()
    {
        if (GameManager.Instance.isPaused)
        {
            Resume();
            return;
        }

        Pause();
    }

    private void Pause()
    {
        pauseMenu.SetActive(true);
        hud.SetActive(false);
        Time.timeScale = 0;
        GameManager.Instance.UnlockCursor();
        GameManager.Instance.isPaused = true;
    }
}
