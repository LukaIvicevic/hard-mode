using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private int damage = 20;

    [SerializeField]
    private int ignoreCollisionLayer = 3;

    [SerializeField]
    private AudioClip bulletImpact;

    [SerializeField]
    private AudioSource audioSource;

    void Start()
    {
        Destroy(gameObject, 10);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Special exception for the door trigger
        if (other.name == "Doors")
        {
            return;
        }

        var layer = other.gameObject.layer;
        if (layer == ignoreCollisionLayer)
        {
            return;
        }

        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            SoundManager.Instance.PlayHitMarker();
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
