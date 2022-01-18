using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 100;
    
    [SerializeField]
    private int damage = 20;

    [SerializeField]
    private int ignoreCollisionLayer = 3;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        var layer = other.gameObject.layer;
        if (layer == ignoreCollisionLayer)
        {
            return;
        }

        var turretBoss = other.GetComponent<SpiderBoss>();
        if (turretBoss != null)
        {
            turretBoss.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
