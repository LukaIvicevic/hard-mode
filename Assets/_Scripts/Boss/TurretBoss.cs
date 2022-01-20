using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretBoss : MonoBehaviour
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
    private GameObject turretShellPrefab;

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

    private TurretState turretState;

    private enum TurretState
    {
        Starting,
        Tracking,
        Firing
    }

    void Start()
    {
        health = maxHealth;
        healthbar.maxValue = maxHealth;
        healthbar.value = health;
        canFireTime = Time.time + fireRate;
        turretState = TurretState.Starting;
    }

    void Update()
    {
        switch (turretState)
        {
            case TurretState.Starting:
                HandleStateStarting();
                break;
            case TurretState.Tracking:
                HandleStateTracking();
                break;
            case TurretState.Firing:
                HandleStateFiring();
                break;
            default:
                break;
        }
    }

    #region State

    private void ChangeState(TurretState tracking)
    {
        switch (tracking)
        {
            case TurretState.Starting:
                break;
            case TurretState.Tracking:
                isFiring = false;
                canFireTime = Time.time + fireRate;
                turretState = TurretState.Tracking;
                break;
            case TurretState.Firing:
                turretState = TurretState.Firing;
                break;
            default:
                break;
        }
    }

    private void HandleStateStarting()
    {
        ChangeState(TurretState.Tracking);
    }

    private void HandleStateTracking()
    {
        LookAtPlayer();

        if (canFireTime <= Time.time)
        {
            ChangeState(TurretState.Firing);
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
        var turretShellInstance = Instantiate(turretShellPrefab);
        turretShellInstance.transform.position = shellSpawner.position;
        turretShellInstance.transform.rotation = shellSpawner.rotation;

        ChangeState(TurretState.Tracking);
    }

    #endregion

    public void TakeDamage(int value)
    {
        Logger.Instance.Log("Hit for " + value);
        health -= value;
        healthbar.value = health;

        if (health <= 0)
        {
            Destroy(this.gameObject);
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

        // Turn it -90s to correctly orient the turret
        targetRotation *= Quaternion.Euler(0, -90, 0);

        // Smoothly turn the turret
        horizontalRotationGroup.rotation = Quaternion.Slerp(horizontalRotationGroup.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void HandleVerticalRotation()
    {
        // Get the position to look at
        var positionToLookAt = target.position - verticalRotationGroup.transform.position;
        
        // Set the position to look at X distance in front of where the turret's current rotation so we only take the
        // player's height into account
        var distanceToTarget = Vector3.Distance(verticalRotationGroup.transform.position, target.position);
        positionToLookAt.z = (horizontalRotationGroup.transform.right * distanceToTarget).z;
        positionToLookAt.x = (horizontalRotationGroup.transform.right * distanceToTarget).x;

        // Find the target rotation
        var targetRotation = Quaternion.LookRotation(positionToLookAt);
        
        // Turn it -90s to correctly orient the turret
        targetRotation *= Quaternion.Euler(0, -90, 0);

        // Smoothly turn the turret
        verticalRotationGroup.rotation = Quaternion.Slerp(verticalRotationGroup.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
