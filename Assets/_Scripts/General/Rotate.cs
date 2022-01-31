using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField]
    private bool counterRotate = false;

    [SerializeField]
    private float duration = 8;

    [SerializeField]
    private Ease ease = Ease.Linear;

    private TweenerCore<Quaternion, Vector3, QuaternionOptions> tween;

    private void Start()
    {
        tween = transform.DORotate(new Vector3(transform.rotation.eulerAngles.x, counterRotate ? -360 : 360, transform.rotation.eulerAngles.z), duration, RotateMode.FastBeyond360).SetLoops(-1).SetEase(ease);
    }

    private void OnDestroy()
    {
        tween.Kill();
    }
}
