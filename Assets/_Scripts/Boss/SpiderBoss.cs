using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiderBoss : Enemy
{
    [Header("Setup")]
    [SerializeField]
    private Transform target;

    [SerializeField]
    private Transform horizontalRotationGroup;

    [SerializeField]
    private Transform verticalRotationGroup;

    [SerializeField]
    private Transform shellSpawner;

    [SerializeField]
    private GameObject shellPrefab;

    [SerializeField]
    private Slider healthbar;

    [Header("Stats")]
    [SerializeField]
    private float maxHealth = 1000f;

    [SerializeField]
    private float rotationSpeed = 5f;

    [SerializeField]
    private float fireRate = 1f;

    [SerializeField]
    private float fireDelay = 1f;

    private float canFireTime = 0f;

    private float health;

    private bool isFiring = false;

    private SpiderState turretState;

    private enum SpiderState
    {
        Starting,
        Tracking,
        Firing
    }

    void Start()
    {
        health = maxHealth;
        canFireTime = Time.time + fireRate;
        ChangeState(SpiderState.Starting);

        if (healthbar == null)
        {
            Logger.Instance.LogWarning("Healthbar not set on SpiderBoss");
            return;
        }

        healthbar.maxValue = maxHealth;
        healthbar.value = health;
        
    }

    void Update()
    {
        switch (turretState)
        {
            case SpiderState.Starting:
                HandleStateStarting();
                break;
            case SpiderState.Tracking:
                HandleStateTracking();
                break;
            case SpiderState.Firing:
                HandleStateFiring();
                break;
            default:
                break;
        }
    }

    public void IntroFinished()
    {
        ChangeState(SpiderState.Tracking);

        healthbar.transform.DOLocalMoveY(475, 1).SetEase(Ease.OutExpo);
    }

    #region State

    private void ChangeState(SpiderState tracking)
    {
        switch (tracking)
        {
            case SpiderState.Starting:
                turretState = SpiderState.Starting;
                break;
            case SpiderState.Tracking:
                isFiring = false;
                canFireTime = Time.time + fireRate;
                turretState = SpiderState.Tracking;
                break;
            case SpiderState.Firing:
                turretState = SpiderState.Firing;
                break;
            default:
                break;
        }
    }

    private void HandleStateStarting()
    {

    }

    private void HandleStateTracking()
    {
        LookAtPlayer();

        if (canFireTime <= Time.time)
        {
            ChangeState(SpiderState.Firing);
        }
    }

    private void HandleStateFiring()
    {
        if (!isFiring)
        {
            StartCoroutine(Fire());
        }
    }

    private IEnumerator Fire()
    {
        isFiring = true;

        // Wait a bunch to give the player a chance
        yield return new WaitForSeconds(fireDelay);

        // Fire
        var turretShellInstance = Instantiate(shellPrefab);
        turretShellInstance.transform.position = shellSpawner.position;
        turretShellInstance.transform.rotation = shellSpawner.rotation;

        ChangeState(SpiderState.Tracking);
    }

    #endregion

    public override void TakeDamage(float damage)
    {
        health -= damage;
        healthbar.value = health;

        if (health <= 0)
        {
            Logger.Instance.Log("Died");
            Destroy(gameObject);
        }
    }

    private void LookAtPlayer()
    {
        HandleHorizontalRotation();
        HandleVerticalRotation();
    }

    private void HandleHorizontalRotation()
    {
        // Get the position to look at
        var positionToLookAt = target.position;

        // Lock the Y position so we only rotate around the Y axis
        positionToLookAt.y = horizontalRotationGroup.position.y;

        // Find the target rotation
        var targetRotation = Quaternion.LookRotation(positionToLookAt - horizontalRotationGroup.position);

        // Smoothly turn the turret
        horizontalRotationGroup.rotation = Quaternion.Slerp(horizontalRotationGroup.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void HandleVerticalRotation()
    {
        // Get the position to look at
        var positionToLookAt = target.position - verticalRotationGroup.transform.position;

        // Find the target rotation
        var targetRotation = Quaternion.LookRotation(positionToLookAt);

        // Turn it 90 to correctly orient the turret
        targetRotation *= Quaternion.Euler(90, 0, 0);

        // Smoothly turn the turret
        verticalRotationGroup.rotation = Quaternion.Slerp(verticalRotationGroup.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
