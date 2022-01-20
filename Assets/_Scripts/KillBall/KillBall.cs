using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBall : MonoBehaviour
{
    private KillBallState killBallState = KillBallState.Intro;

    private enum KillBallState
    {
        Intro,
        Aim,
        Move
    }

    // Start is called before the first frame update
    void Start()
    {
        IntroAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void IntroAnimation()
    {
        // Save spawn position
        var spawnPosition = transform.position;

        // Move below the ground
        transform.position = new Vector3(transform.position.x, transform.position.y - 10, transform.position.z);

        // Animate up
        transform.DOMove(spawnPosition, 2).SetEase(Ease.InOutExpo).OnComplete(SetStartState);
    }

    private void SetStartState()
    {
        ChangeState(KillBallState.Aim);
    }

    private void ChangeState(KillBallState state)
    {
        switch (state)
        {
            case KillBallState.Intro:
                break;
            case KillBallState.Aim:
                break;
            case KillBallState.Move:
                break;
            default:
                break;
        }
    }
}
