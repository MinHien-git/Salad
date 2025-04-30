using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class Bouncy : MonoBehaviour
{
    [SerializeField]
    float bouncePixels = 40f; // ↑ distance in px

    [SerializeField]
    float halfCycle = 0.25f; // ↑ time up or down

    [SerializeField]
    float scaleFactor = 1.15f; // ↑ max puff size

    RectTransform rt;
    Vector2 startAnchoredPos;
    Vector3 startScale;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        startAnchoredPos = rt.anchoredPosition;
        startScale = rt.localScale;
    }

    void Start()
    {
        // Single sequence that yo-yo’s forever
        DOTween
            .Sequence()
            // go up & grow
            .Append(
                rt.DOAnchorPosY(startAnchoredPos.y + bouncePixels, halfCycle).SetEase(Ease.OutQuad)
            )
            .Join(rt.DOScale(startScale * scaleFactor, halfCycle).SetEase(Ease.OutQuad))
            // come back & shrink
            .Append(rt.DOAnchorPosY(startAnchoredPos.y, halfCycle).SetEase(Ease.InQuad))
            .Join(rt.DOScale(startScale, halfCycle).SetEase(Ease.InQuad))
            // infinite yo-yo
            .SetLoops(-1, LoopType.Restart);
    }
}
