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

        if (audioSource != null)
        {
            SoundManager.Instance.PlayOneShot(audioSource, bulletImpact);
        }

        GetComponentInChildren<MeshRenderer>().enabled = false;
        Invoke("DestroyBullet", bulletImpact.length);
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
