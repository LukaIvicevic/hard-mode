using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    [SerializeField]
    private Transform pointLight;

    [SerializeField]
    private float launchForce = 20f;

    private void Start()
    {
        if (pointLight == null)
        {
            Logger.Instance.LogWarning("Point Light not set on Launch Pad");
        }

        pointLight.DOLocalMove(new Vector3(0, 1, 0), 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!(other.tag == "Player"))
        {
            return;
        }

        var controller = other.gameObject.GetComponent<Q3Movement.Q3PlayerController>();
        controller.Launch(transform.up, launchForce);
    }
}
