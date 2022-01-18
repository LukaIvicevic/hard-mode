using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField]
    private float duration = 2f;

    private TweenerCore<Vector3, Vector3, VectorOptions> tween;

    void Start()
    {
        Fall();
    }

    private void OnDestroy()
    {
        tween.Kill();
    }


    private void Fall()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, out var hit))
        {
            // Fall to the ground
            tween = transform.DOMove(hit.point, duration).SetEase(Ease.InExpo).OnComplete(CameraShake);
        }
    }

    private void CameraShake()
    {
        GameManager.Instance.CameraShake(0.4f);
    }
}
