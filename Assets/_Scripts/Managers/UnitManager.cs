using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitManager : Singleton<UnitManager>
{
    [SerializeField]
    private GameObject bossStandObject;

    [SerializeField]
    private GameObject spiderBossObject;

    [SerializeField]
    private float forceFieldDestroyedDuration = 10f;

    [SerializeField]
    private GameObject generatorPrefab;

    [SerializeField]
    private GameObject killBallPrefab;

    [SerializeField]
    private Transform[] generatorSpawnPositions;

    [SerializeField]
    private Slider[] generatorHealthbars = new Slider[4];

    private GameObject[] generators = new GameObject[4];

    private Animator bossStandAnimator;

    private float bossSpawnDelay = 4f;

    private void Start()
    {
        if (bossStandObject == null)
        {
            Logger.Instance.LogWarning("BossStandObject not set on UnitManager");
        }

        bossStandAnimator = bossStandObject.GetComponent<Animator>();
    }

    public void SpawnGenerators()
    {
        if (generatorSpawnPositions == null)
        {
            Logger.Instance.LogWarning($"{nameof(SpawnGenerators)} - No generator spawn positions set");
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
            Logger.Instance.LogWarning("SpiderBossObject not set on UnitManager");
            return;
        }

        if (bossStandAnimator == null)
        {
            Logger.Instance.LogWarning("BossStandAnimator not set on UnitManager");
            return;
        }


        Invoke("BossIntro", bossSpawnDelay);
    }

    public void GeneratorDestroyed(int id)
    {
        SpawnKillBall(generators[id].transform.position);

        generators[id] = null;

        CheckGenerators();
    }

    private void SpawnKillBall(Vector3 position)
    {
        if (killBallPrefab == null)
        {
            Logger.Instance.LogWarning("KillBallPrefab is not set on UnitManager");
            return;
        }

        position = new Vector3(position.x, position.y + 1, position.z);
        Instantiate(killBallPrefab, position, Quaternion.identity);
    }

    private void CheckGenerators()
    {
        // If all generators are dead then bring down the force field
        foreach (var item in generators)
        {
            if (item != null) return;
        }

        spiderBossObject.GetComponent<SpiderBoss>().DeactivateForceField();
        Invoke("ActivateForceField", forceFieldDestroyedDuration);
    }

    private void ActivateForceField()
    {
        // The boss might have already been defeated so we can just return to avoid an exception
        if (spiderBossObject == null) return;

        SpawnGenerators();
        spiderBossObject.GetComponent<SpiderBoss>().ActivateForceField();
    }

    private void BossIntro()
    {
        // Activate boss game object and play the intro animation
        spiderBossObject.SetActive(true);
        bossStandAnimator.SetTrigger("Intro");
    }

    private IEnumerator SpawnGenerator(int index)
    {
        if (generatorSpawnPositions[index] == null)
        {
            Logger.Instance.LogWarning($"GeneratorSpawnPosition {index} not set on UnitManager");
        }

        // Wait before spawning
        var delay = index;
        yield return new WaitForSeconds(delay);

        // Spawn generator
        var position = generatorSpawnPositions[index];
        generators[index] = Instantiate(generatorPrefab, position);
        
        // Set the id
        var generator = generators[index].GetComponent<Generator>();
        generator.SetId(index);

        // Setup healthbar
        if (generatorHealthbars[index] == null)
        {
            Logger.Instance.LogWarning($"GeneratorHealthbar {index} not set on UnitManager");
        }
        generator.SetHealthbar(generatorHealthbars[index]);
    }
}
