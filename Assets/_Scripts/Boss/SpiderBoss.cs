using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SpiderBoss : Enemy
{
    [Header("Stats")]
    [SerializeField]
    private float maxHealth = 1000f;

    [SerializeField]
    private float rotationSpeed = 5f;

    [SerializeField]
    private float fireRate = 1f;

    [SerializeField]
    private float fireDelay = 1f;

    [Header("Force Field")]
    [SerializeField]
    private Vector3 forceFieldScale = new Vector3(50, 30, 65);
    [SerializeField]
    private float forceFieldAnimationDuration = 2f;
    [SerializeField]
    private Ease forceFieldAnimationEase = Ease.OutExpo;

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
    private GameObject forceField;

    [SerializeField]
    private Slider healthbar;

    [SerializeField]
    private GameObject shootParticleSystem;

    [SerializeField]
    private AudioClip groundImpact;

    [SerializeField]
    private AudioSource impactSource;

    [SerializeField]
    private AudioClip tankShot;



    private SpiderState turretState;

    private float canFireTime = 0f;

    private bool isFiring = false;

    private bool canBeDamaged = false;

    private float health;



    private enum SpiderState
    {
        Starting,
        Tracking,
        Firing
    }

    private void Start()
    {
        AdjustDifficulty();

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

    private void Update()
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
        // Play ground impact
        if (impactSource)
        {
            SoundManager.Instance.PlayOneShot(impactSource, groundImpact);
        }

        // Start tracking
        ChangeState(SpiderState.Tracking);

        // Activate force field
        ActivateForceField();

        // Ease in healthbar
        healthbar.transform.DOLocalMoveY(475, 1).SetEase(Ease.OutExpo);

    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealth()
    {
        return health;
    }

    private void AdjustDifficulty()
    {
        fireRate = StatsManager.Instance.GetDifficultyValue(StatsManager.Instance.tankFireRateD1, StatsManager.Instance.tankMaxFireRateD10);
        fireDelay = StatsManager.Instance.GetDifficultyValue(StatsManager.Instance.tankFireRateD1, StatsManager.Instance.tankMaxFireRateD10);
        maxHealth = StatsManager.Instance.GetDifficultyValue(StatsManager.Instance.bossHealthD1, StatsManager.Instance.bossHealthD10);
    }

    #region Force Field

    public void ActivateForceField()
    {
        if (forceField == null)
        {
            Logger.Instance.LogWarning("ForceField not set on SpiderBoss");
            return;
        }

        canBeDamaged = false;
        forceField.SetActive(true);
        forceField.transform.localScale = Vector3.zero;
        forceField.transform.rotation = transform.rotation;
        forceField.transform.DOScale(forceFieldScale, forceFieldAnimationDuration).SetEase(forceFieldAnimationEase);
    }

    public void DeactivateForceField()
    {
        if (forceField == null)
        {
            Logger.Instance.LogWarning("ForceField not set on SpiderBoss");
            return;
        }

        forceField.SetActive(false);
        forceField.transform.localScale = Vector3.zero;
        forceField.transform.rotation = transform.rotation;
        forceField.transform.DOScale(Vector3.zero, forceFieldAnimationDuration).SetEase(forceFieldAnimationEase).OnComplete(SetForceFieldInactive);
    }

    private void SetForceFieldInactive()
    {
        forceField.SetActive(false);
        canBeDamaged = true;
    }

    #endregion

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

        // Spawn particle system
        var explosion = Instantiate(shootParticleSystem, shellSpawner.position, Quaternion.identity);
        explosion.GetComponent<PlayOneShot>().AudioClip = tankShot;

        ChangeState(SpiderState.Tracking);
    }

    #endregion

    public override void TakeDamage(float damage)
    {
        if (!canBeDamaged)
        {
            return;
        }

        health -= damage;
        healthbar.value = health;

        if (health <= 0)
        {
            GameManager.Instance.PlayerWins();
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
