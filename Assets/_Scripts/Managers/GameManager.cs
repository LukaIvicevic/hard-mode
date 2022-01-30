using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool isPaused = false;

    [SerializeField]
    private DeathMenu deathMenu;

    [SerializeField]
    private int difficulty = 1;

    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private Camera weaponCamera;

    [SerializeField]
    private Transform killBallCastStart;

    private bool started = false;

    private float playerSensitivity = 1f;

    private void Start()
    {
        LockCursor();
    }

    public void StartFight()
    {
        if (started)
        {
            return;
        }

        UnitManager.Instance.SpawnGenerators();
        UnitManager.Instance.SpawnBoss();
        started = true;
    }

    public void Restart()
    {
        SceneManager.LoadScene("BossFight");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void CameraShake(float shakeAmount)
    {
        playerCamera.GetComponent<StressReceiver>()?.InduceStress(shakeAmount);
        weaponCamera.GetComponent<StressReceiver>()?.InduceStress(shakeAmount * 0.2f);
    }

    public void SelectDifficulty(int d)
    {
        Logger.Instance.Log("Difficulty changed to: " + d);
        difficulty = d;
    }

    public float GetDifficulty()
    {
        return difficulty;
    }

    public Transform GetKillBallCastStart()
    {
        return killBallCastStart;
    }

    public void KillPlayer(GameObject player)
    {
        // Temp
        //player.transform.position = killBallCastStart.position;
        deathMenu.PlayerDied();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public float GetSensitivity()
    {
        return playerSensitivity;
    }

    public void SetSensitivity(float value)
    {
        playerSensitivity = value;
    }
}
