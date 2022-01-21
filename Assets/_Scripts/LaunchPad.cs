using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!(other.tag == "Player"))
        {
            return;
        }

        print("player");

        var controller = other.gameObject.GetComponent<Q3Movement.Q3PlayerController>();
        controller.launch = 20;
    }
}
