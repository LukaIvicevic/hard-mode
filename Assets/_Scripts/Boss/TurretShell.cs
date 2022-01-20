using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShell : MonoBehaviour
{
    [SerializeField]
    private float speed = 100;

    //[SerializeField]
    //private int damage = 20;

    [SerializeField]
    private GameObject explosionEffect;

    [SerializeField]
    private int ignoreCollisionLayer = 3;

    void Update()
    {
            transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        var isIgnoreCollisionLayer = other.gameObject.layer == ignoreCollisionLayer;
        var isTurretBoss = other.gameObject.tag == "TurretBoss";
        var isForceField = other.gameObject.tag == "ForceField";
        var ignoreCollision = isIgnoreCollisionLayer || isTurretBoss || isForceField;
        if (ignoreCollision)
        {
            return;
        }

        Explode();
    }

    private void Explode()
    {
        // Play explosion
        var explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        explosion.GetComponent<ParticleSystem>()?.Play();

        // Camera shake
        GameManager.Instance.CameraShake(0.4f);

        // Apply damage

        // Destroy shell
        Destroy(gameObject);
    }
}
