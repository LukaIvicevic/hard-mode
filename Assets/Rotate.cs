using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    void Start()
    {
        transform.DORotate(new Vector3(0, 360, 0), 8, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
    }
}
