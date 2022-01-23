using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFloorTile : MonoBehaviour
{
    private bool isActive = false;

    public void SetActive(bool active)
    {
        isActive = active;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive)
        {
            return;
        }

        var isPlayer = other.tag == "Player";
        if (!isPlayer)
        {
            return;
        }

        var player = other.gameObject;
        GameManager.Instance.KillPlayer(player);
    }
}
