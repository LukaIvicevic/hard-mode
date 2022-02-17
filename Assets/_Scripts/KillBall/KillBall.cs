using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBall : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip audioClip;
    [SerializeField]
    private float aimDuration = 1;
    [SerializeField]
    private float speed = 10;

    private SphereCollider sphereCollider;
    private Transform castStart;
    private Vector3 castDirection;
    private Vector3 nextMovePosition;
    private bool isMoving = false;

    [SerializeField]
    private LineRenderer lineRenderer;

    private enum KillBallState
    {
        Intro,
        Aim,
        Move
    }

    private void Start()
    {
        AdjustDifficulty();

        sphereCollider = GetComponent<SphereCollider>();
        lineRenderer = GetComponent<LineRenderer>();
        castStart = GameManager.Instance.GetKillBallCastStart();
        SoundManager.Instance.PlayLoop(audioSource, audioClip);
        IntroAnimation();
    }

    private void AdjustDifficulty()
    {
        speed = StatsManager.Instance.GetDifficultyValue(StatsManager.Instance.killBallSpeedD1, StatsManager.Instance.killBallSpeedD10);
    }

    private void Update() {
        if (isMoving)
        {
            DrawTrail();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var isPlayer = other.tag == "Player";
        if (!isPlayer)
        {
            return;
        }

        var player = other.gameObject;
        GameManager.Instance.PlayerDied();
    }

    private void DrawTrail()
    {
        // Draw the path
        var direction = nextMovePosition - transform.position;
        direction = transform.position + (direction.normalized * 30);
        var linePositions = new Vector3[] { transform.position, direction };
        lineRenderer.SetPositions(linePositions);
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
                lineRenderer.positionCount = 0;
                isMoving = false;
                HandleAimState();
                break;
            case KillBallState.Move:
                lineRenderer.positionCount = 2;
                isMoving = true;
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
        if (Physics.Raycast(castStart.position, castDirection, out var hit))
        {
            // Go to the target position
            var targetPosition = hit.point.normalized * (hit.point.magnitude - (sphereCollider.radius * 2));

            // Debug
            Logger.Instance.DrawRay(castStart.position, castDirection * 50, Color.red, 5);

            nextMovePosition = targetPosition;
        }
        else
        {
            // If nothing was hit then aim again
            ChangeState(KillBallState.Aim);
        }


        // Look at next position
        var lookAtDirection = nextMovePosition - transform.position;
        var lookAtRotation = Quaternion.LookRotation(lookAtDirection);
        transform.DORotateQuaternion(lookAtRotation, aimDuration).SetEase(Ease.InOutExpo).OnComplete(OnAimRotateComplete);
    }

    private void OnAimRotateComplete()
    {
        ChangeState(KillBallState.Move);
    }

    private void HandleMoveState()
    {
        transform.DOMove(nextMovePosition, speed).SetSpeedBased(true).OnComplete(OnMoveComplete);
    }

    private void OnMoveComplete()
    {
        ChangeState(KillBallState.Aim);
    }
}
