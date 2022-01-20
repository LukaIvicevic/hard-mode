using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStand : MonoBehaviour
{
    [SerializeField]
    private SpiderBoss spiderBoss;


    public void IntroFinished()
    {
        GameManager.Instance.CameraShake(0.5f);
        if (spiderBoss == null)
        {
            Logger.Instance.LogWarning("SpiderBoss not set on BossStand");
            return;
        }

        spiderBoss.IntroFinished();
    }
}
