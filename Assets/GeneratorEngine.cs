using DG.Tweening;
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
        print("Engine destroyed");
        Destroy(pointLight);
        var targetPosition = new Vector3(transform.position.x, transform.position.y - 3, transform.position.z);
        transform.DOMove(targetPosition, 0.75f);
    }
}
