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
    private Transform castStart;
    private Vector3 castDirection;
    private Vector3 nextMovePosition;

    private enum KillBallState
    {
        Intro,
        Aim,
        Move
    }

    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        castStart = GameManager.Instance.GetKillBallCastStart();
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
        targetRotation.x = castStart.rotation.x;
        targetRotation.z = castStart.rotation.z;
        castDirection = targetRotation * Vector3.forward;

        // Raycast in the position we are facing
        if (Physics.SphereCast(castStart.position, sphereCollider.radius, castDirection, out var hit))
        {
            // Go to the target position
            var targetPosition = hit.point.normalized * (hit.point.magnitude - (sphereCollider.radius * 2));

            // Debug
            hitPosition = hit.point;
            castRadius = sphereCollider.radius;
            Debug.DrawRay(castStart.position, castDirection * 50, Color.red, 5);

            nextMovePosition = targetPosition;
            //transform.DOMove(targetPosition, speed).SetSpeedBased(true).OnComplete(OnMoveComplete);
        }
        else
        {
            // If nothing was hit then aim again
            ChangeState(KillBallState.Aim);
        }


        // Look at next position
        transform.LookAt(nextMovePosition);
        OnAimRotateComplete();
        // transform.DORotateQuaternion(targetRotation, aimDuration).SetEase(Ease.InOutExpo).OnComplete(OnAimRotateComplete);
    }

    private void OnAimRotateComplete()
    {
        ChangeState(KillBallState.Move);
    }

    private Vector3 hitPosition = Vector3.zero;
    private float castRadius = 1;

    private void HandleMoveState()
    {
        transform.DOMove(nextMovePosition, speed).SetSpeedBased(true).OnComplete(OnMoveComplete);

        //// Raycast in the position we are facing
        //if (Physics.SphereCast(castStart, sphereCollider.radius, sphereCollider.transform.forward, out var hit))
        //{
        //    // Go to the target position
        //    var targetPosition = hit.point.normalized * (hit.point.magnitude - (sphereCollider.radius * 2));

        //    // Debug
        //    hitPosition = hit.point;
        //    castRadius = sphereCollider.radius;
        //    Debug.DrawRay(castStart, sphereCollider.transform.forward * 50, Color.red, 5);

        //    transform.DOMove(targetPosition, speed).SetSpeedBased(true).OnComplete(OnMoveComplete);
        //}
        //else
        //{
        //    // If nothing was hit then aim again
        //    ChangeState(KillBallState.Aim);
        //}
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.DrawSphere(hitPosition, castRadius);
    }

    private void OnMoveComplete()
    {
        ChangeState(KillBallState.Aim);
    }
}
