using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private int difficulty = 1;

    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private Camera weaponCamera;

    [SerializeField]
    private Transform killBallCastStart;

    private bool started = false;

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

    public int GetDifficulty()
    {
        return difficulty;
    }

    public Transform GetKillBallCastStart()
    {
        return killBallCastStart;
    }

    public void PlayerHitKillFloorTile()
    {
        print("You died");
    }
}
