using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuToggle : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject hud;

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TogglePauseMenu();
        }
    }

    private void TogglePauseMenu()
    {
        if (GameManager.Instance.isPaused)
        {
            HidePauseMenu();
            return;
        }

        ShowPauseMenu();
    }

    private void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
        hud.SetActive(false);
        Time.timeScale = 0;
        GameManager.Instance.UnlockCursor();
        GameManager.Instance.isPaused = true;
    }

    private void HidePauseMenu()
    {
        pauseMenu.SetActive(false);
        hud.SetActive(true);
        Time.timeScale = 1;
        GameManager.Instance.LockCursor();
        GameManager.Instance.isPaused = false;
    }
}
