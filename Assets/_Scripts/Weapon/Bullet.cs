using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private int damage = 20;

    [SerializeField]
    private int ignoreCollisionLayer = 3;


    void Start()
    {
        Destroy(gameObject, 10);
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        var layer = other.gameObject.layer;
        if (layer == ignoreCollisionLayer)
        {
            return;
        }

        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
