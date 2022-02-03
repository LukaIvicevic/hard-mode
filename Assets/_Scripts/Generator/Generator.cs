using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
    [SerializeField]
    private GeneratorEngine engine1;

    [SerializeField]
    private GeneratorEngine engine2;

    [SerializeField]
    private AudioClip groundImpact;

    [SerializeField]
    private AudioSource audioSource;

    [Header("Fall")]
    [SerializeField]
    private float fallDuration = 2f;

    private Slider healthbar;

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
        SetupHealthBar();
    }

    public void SetId(int value)
    {
        id = value;
    }

    public void OnEngineTakeDamage()
    {
        // Update healthbar
        healthbar.value = engine1.GetHealth() + engine2.GetHealth();

        // Handle destroy
        if (engine1.isDestroyed && engine2.isDestroyed)
        {
            Destroyed();
        }
    }

    private void SetupHealthBar()
    {
        if (healthbar == null)
        {
            Logger.Instance.LogWarning("HealthBar not set on Generator");
            return;
        }

        healthbar.maxValue = engine1.GetHealth() + engine2.GetHealth();
        healthbar.value = healthbar.maxValue;
    }

    private void OnDestroy()
    {
        tween.Kill();
    }

    private void Destroyed()
    {
        UnitManager.Instance.GeneratorDestroyed(id);
        transform.DOMove(new Vector3(transform.position.x, transform.position.y - 10, transform.position.z), 2).SetEase(Ease.InOutExpo).OnComplete(Destroy);
    }

    private void Destroy()
    {
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

        if (audioSource)
        {
            SoundManager.Instance.PlayOneShot(audioSource, groundImpact);
        }
    }
}
