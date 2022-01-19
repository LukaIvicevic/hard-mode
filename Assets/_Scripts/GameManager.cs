using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private Camera weaponCamera;

    [SerializeField]
    private GameObject generatorPrefab;

    [SerializeField]
    private Transform[] generatorSpawnPositions;

    private GameObject[] generators = new GameObject[4];

    public void CameraShake(float shakeAmount)
    {
        playerCamera.GetComponent<StressReceiver>()?.InduceStress(shakeAmount);
        weaponCamera.GetComponent<StressReceiver>()?.InduceStress(shakeAmount * 0.5f);
    }

    public void SpawnGenerators()
    {
        if (generatorSpawnPositions == null)
        {
            Debug.LogWarning($"{nameof(SpawnGenerators)} - No generator spawn positions set");
            return;
        }

        for (int i = 0; i < generatorSpawnPositions.Length; i++)
        {
            if (generators[i] != null)
            {
                Destroy(generators[i]);
            }

            StartCoroutine(SpawnGenerator(i));
        }
    }

    private IEnumerator SpawnGenerator(int index)
    {
        var delay = index * 0.5f;
        yield return new WaitForSeconds(delay);

        var position = generatorSpawnPositions[index];
        generators[index] = Instantiate(generatorPrefab, position);
    }
}
