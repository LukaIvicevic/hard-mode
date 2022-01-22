using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField]
    private bool close = false;

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

        Open();
    }

    private void Open()
    {
        leftDoor.DOLocalMove(new Vector3(2.25f, 0.5f, 0), .5f).SetEase(Ease.OutQuad);
        rightDoor.DOLocalMove(new Vector3(-2.25f, 0.5f, 0), .5f).SetEase(Ease.OutQuad);

        if (close)
        {
            Invoke("Close", 5);
        }
    }

    private void Close()
    {
        leftDoor.DOLocalMove(new Vector3(.75f, 0.5f, 1f), .5f).SetEase(Ease.OutQuad);
        rightDoor.DOLocalMove(new Vector3(-.75f, 0.5f, 1f), .5f).SetEase(Ease.OutQuad);
    }
}
