using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private Camera weaponCamera;

    public void StartFight()
    {
        UnitManager.Instance.SpawnGenerators();
    }

    public void CameraShake(float shakeAmount)
    {
        playerCamera.GetComponent<StressReceiver>()?.InduceStress(shakeAmount);
        weaponCamera.GetComponent<StressReceiver>()?.InduceStress(shakeAmount * 0.2f);
    }


}
