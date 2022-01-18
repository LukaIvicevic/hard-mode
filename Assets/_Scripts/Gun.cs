using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    private Transform bulletSpawner;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private new ParticleSystem particleSystem;

    [Header("Stats")]
    [SerializeField]
    private float fireRateMultiplier = 1f;

    [SerializeField]
    private float gracePeriod = 0.2f;

    private float fireRate;
    private float canFireTime = 0;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("FireRateMultiplier", fireRateMultiplier);
        var animations = animator.runtimeAnimatorController.animationClips;
        foreach (var animation in animations)
        {
            if (animation.name == "Recoil")
            {
                fireRate = animation.length;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        var canFire = Time.time >= canFireTime;
        if (canFire && Input.GetMouseButton(0))
        {
            // Spawn bullet
            var bulletInstance = Instantiate(bulletPrefab);
            bulletInstance.transform.position = bulletSpawner.position;
            bulletInstance.transform.rotation = bulletSpawner.rotation;

            // Handle animation
            particleSystem.Play();
            animator.Play("Recoil", 0, 0f);

            // Handle fire rate
            canFireTime = Time.time + fireRate - gracePeriod;
        }
    }
}
