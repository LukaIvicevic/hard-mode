using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generator : Enemy
{
    [Header("Stats")]
    [SerializeField]
    private float maxHealth = 1000f;

    [Header("Fall")]
    [SerializeField]
    private float fallDuration = 2f;

    private Slider healthbar;

    private float health;

    private float targetHealthbarX = 120f;

    private int id;

    private TweenerCore<Vector3, Vector3, VectorOptions> tween;

    private void Start()
    {
        Fall();
    }

    public void SetHealthbar(Slider hb)
    {
        healthbar = hb;
        SetupHealth();
    }

    public override void TakeDamage(float damage)
    {
        // Reduce health
        health -= damage;
        healthbar.value = health;

        // Handle destroy
        if (health <= 0)
        {
            Destroyed();
        }
    }

    public void SetId(int value)
    {
        id = value;
    }

    private void SetupHealth()
    {
        health = maxHealth;

        if (healthbar == null)
        {
            Logger.Instance.LogWarning("HealthBar not set on Generator");
            return;
        }

        healthbar.maxValue = maxHealth;
        healthbar.value = health;
    }

    private void OnDestroy()
    {
        tween.Kill();
    }

    private void Destroyed()
    {
        UnitManager.Instance.GeneratorDestroyed(id);
        Destroy(gameObject);
    }

    private void Fall()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, out var hit))
        {
            // Fall to the ground
            tween = transform.DOMove(hit.point, fallDuration).SetEase(Ease.InExpo).OnComplete(FallComplete);
        }
    }

    private void FallComplete()
    {
        healthbar.transform.DOMoveX(targetHealthbarX, 1).SetEase(Ease.OutExpo);
        GameManager.Instance.CameraShake(0.4f);
    }
}
