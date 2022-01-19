using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStand : MonoBehaviour
{
    public void IntroFinished()
    {
        GameManager.Instance.CameraShake(0.5f);
        var spiderBoss = GetComponentInChildren<SpiderBoss>();

        if (spiderBoss == null)
        {
            return;
        }

        spiderBoss.IntroFinished();
    }
}
