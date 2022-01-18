using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
    [SerializeField]
    private float duration;
    [SerializeField]
    private float X1 = -30;
    [SerializeField]
    private float X2 = 30;
    [SerializeField]
    private float Z1 = -30;
    [SerializeField]
    private float Z2 = 30;


    // Start is called before the first frame update
    void Start()
    {
        var sequence = DOTween.Sequence();

        sequence.SetLoops(-1);
        sequence.Append(transform.DOLocalMoveX(X1, duration).SetEase(Ease.InOutSine));
        sequence.Append(transform.DOLocalMoveZ(Z1, duration).SetEase(Ease.InOutSine));
        sequence.Append(transform.DOLocalMoveX(X2, duration).SetEase(Ease.InOutSine));
        sequence.Append(transform.DOLocalMoveZ(Z2, duration).SetEase(Ease.InOutSine));

    }
}
