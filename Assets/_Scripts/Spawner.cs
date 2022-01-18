using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    private GameObject spawnedObject;

    private void OnTriggerEnter(Collider other)
    {
        //if (spawnedObject != null)
        //{
        //    Destroy(spawnedObject);
        //}

        //spawnedObject = Instantiate(prefab);

        GameManager.Instance.SpawnGenerators();
    }
}
