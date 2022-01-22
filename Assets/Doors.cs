using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField]
    private Transform leftDoor;

    [SerializeField]
    private Transform rightDoor;

    private void OnTriggerEnter(Collider other)
    {
        var isPlayer = other.gameObject.tag == "Player";
        if (!isPlayer)
        {
            return;
        }

        leftDoor.DOLocalMove(new Vector3(2.25f, 0.5f), .5f).SetEase(Ease.OutQuad);
        rightDoor.DOLocalMove(new Vector3(-2.25f, 0.5f), .5f).SetEase(Ease.OutQuad);
    }
}
