using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorEngine : Enemy
{
    [SerializeField]
    private Generator generator;

    [SerializeField]
    private float maxHealth = 300f;

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
        // TODO Change the model
        print("Engine destroyed");
    }
}
