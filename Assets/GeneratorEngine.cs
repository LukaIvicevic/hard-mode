using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorEngine : Enemy
{
    [SerializeField]
    private Generator generator;

    [SerializeField]
    private float maxHealth = 300f;

    [SerializeField]
    private GameObject pointLight;

    private float health;
    private TweenerCore<Vector3, Vector3, VectorOptions> tween;

    public bool isDestroyed {
        get { return health <= 0; }
        set { isDestroyed = value;  }
    }

    private void Awake()
    {
        health = maxHealth;
        print(health);
    }

    public override void TakeDamage(float damage)
    {
        if (isDestroyed)
        {
            return;
        }

        health -= damage;

        if (isDestroyed)
        {
            Destroy();
        }

        generator.OnEngineTakeDamage();
    }

    public float GetHealth()
    {
        return health;
    }

    private void Destroy()
    {
        Destroy(pointLight);
        var targetPosition = new Vector3(transform.position.x, transform.position.y - 3, transform.position.z);
        tween = transform.DOMove(targetPosition, 0.25f).SetEase(Ease.InBack);
    }

    private void OnDestroy()
    {
        tween.Kill();
    }
}
