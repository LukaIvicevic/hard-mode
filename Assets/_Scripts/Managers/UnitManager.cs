using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    [SerializeField]
    private GameObject bossStandObject;

    [SerializeField]
    private GameObject spiderBossObject;

    [SerializeField]
    private GameObject generatorPrefab;

    [SerializeField]
    private Transform[] generatorSpawnPositions;

    private GameObject[] generators = new GameObject[4];

    private Animator bossStandAnimator;

    private float bossSpawnDelay = 4f;

    private void Start()
    {
        if (bossStandObject == null)
        {
            Debug.LogWarning("BossStandObject not set on UnitManager");
        }

        bossStandAnimator = bossStandObject.GetComponent<Animator>();
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

    public void SpawnBoss()
    {
        if (spiderBossObject == null)
        {
            Debug.LogWarning("SpiderBossObject not set on UnitManager");
            return;
        }

        if (bossStandAnimator == null)
        {
            Debug.LogWarning("BossStandAnimator not set on UnitManager");
            return;
        }


        Invoke("BossIntro", bossSpawnDelay);
    }

    private void BossIntro()
    {
        // Activate boss game object and play the intro animation
        spiderBossObject.SetActive(true);
        bossStandAnimator.SetTrigger("Intro");
    }

    private IEnumerator SpawnGenerator(int index)
    {
        var delay = index * 0.5f;
        yield return new WaitForSeconds(delay);

        var position = generatorSpawnPositions[index];
        generators[index] = Instantiate(generatorPrefab, position);
    }
}
