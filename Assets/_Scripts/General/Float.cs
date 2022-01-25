using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class Float : MonoBehaviour
{
    [SerializeField]
    private bool startDown = false;

    [SerializeField]
    private float amount = 50;

    [SerializeField]
    private float duration = 5;

    [SerializeField]
    private Ease ease = Ease.InOutQuad;

    private TweenerCore<Vector3, Vector3, VectorOptions> tween;

    private void Start()
    {
        tween = transform.DOLocalMove(new Vector3(transform.position.x, transform.position.y + (startDown ? -amount : amount), transform.position.z), duration).SetLoops(-1, LoopType.Yoyo).SetEase(ease);
    }

    private void OnDestroy()
    {
        tween.Kill();
    }
}
