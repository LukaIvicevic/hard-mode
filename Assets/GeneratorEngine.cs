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

    [SerializeField]
    private GameObject sphere;

    private float health;
    private TweenerCore<Vector3, Vector3, VectorOptions> tween;
    private Color baseColor;

    public bool isDestroyed {
        get { return health <= 0; }
        set { isDestroyed = value;  }
    }

    private void Awake()
    {
        health = maxHealth;
        baseColor = sphere.GetComponent<MeshRenderer>().material.color;
    }

    public override void TakeDamage(float damage)
    {
        if (isDestroyed)
        {
            return;
        }

        health -= damage;

        HitEffect();

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

    private void HitEffect()
    {
        var healthPercentage = health / maxHealth;
        var intensity = Mathf.Lerp(10, 2, healthPercentage);
        sphere.GetComponent<MeshRenderer>().material.color = baseColor * intensity;
        sphere.GetComponent<Animator>().Play("EngineHit", -1, 0f);
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
