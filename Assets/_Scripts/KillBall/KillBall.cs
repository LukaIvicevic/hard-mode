using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBall : MonoBehaviour
{
    [SerializeField]
    private float aimDuration = 1;
    [SerializeField]
    private float speed = 10;

    private SphereCollider sphereCollider;

    private enum KillBallState
    {
        Intro,
        Aim,
        Move
    }

    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        IntroAnimation();
    }

    void Update() { }

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
                HandleAimState();
                break;
            case KillBallState.Move:
                HandleMoveState();
                break;
            default:
                break;
        }
    }

    private void HandleAimState()
    {
        // Rotate in a random direction on the Y axis
        var targetRotation = Random.rotation;
        targetRotation.x = transform.rotation.x;
        targetRotation.z = transform.rotation.z;
        transform.DORotateQuaternion(targetRotation, aimDuration).SetEase(Ease.InOutExpo).OnComplete(OnAimRotateComplete);
    }

    private void OnAimRotateComplete()
    {
        ChangeState(KillBallState.Move);
    }

    private void HandleMoveState()
    {
        // Raycast in the position we are facing
        if (Physics.SphereCast(sphereCollider.transform.position, sphereCollider.radius, transform.forward, out var hit))
        {
            // Go to the target position
            var targetPosition = hit.point.normalized * (hit.point.magnitude - (sphereCollider.radius * 2));
            transform.DOMove(targetPosition, speed).SetSpeedBased(true).OnComplete(OnMoveComplete);
        }
        else
        {
            // If nothing was hit then aim again
            ChangeState(KillBallState.Aim);
        }
    }

    private void OnMoveComplete()
    {
        ChangeState(KillBallState.Aim);
    }
}
