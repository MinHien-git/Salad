using DG.Tweening;
using UnityEngine;

public class SpoonAnimator : MonoBehaviour
{
    public RectTransform spoon;
    public float initAngle = 30f;
    public float radiusX = 30f;
    public float radiusY = 20f;
    public float ellipseDuration = 1.2f;
    public float scaleAmount = 0.1f; // Scale ±10% để tạo cảm giác gần xa

    private Vector2 originalPos;
    private Vector3 originalScale;

    public void StartStirring()
    {
        originalPos = spoon.anchoredPosition;
        originalScale = spoon.localScale;
        spoon.localRotation = Quaternion.Euler(0, 0, initAngle);

        StirSpoon();
        MoveInEllipse();
        ScaleInOut();
    }

    void StirSpoon()
    {
        Sequence stirSequence = DOTween.Sequence();

        stirSequence.Append(
            spoon.DORotate(new Vector3(0, 0, 20f), 0.3f).SetRelative().SetEase(Ease.InOutSine)
        );
        stirSequence.Append(
            spoon.DORotate(new Vector3(0, 0, -40f), 0.6f).SetRelative().SetEase(Ease.InOutSine)
        );
        stirSequence.Append(
            spoon.DORotate(new Vector3(0, 0, 20f), 0.3f).SetRelative().SetEase(Ease.InOutSine)
        );

        stirSequence.SetLoops(-1, LoopType.Restart);
    }

    void MoveInEllipse()
    {
        DOTween
            .To(
                () => 0f,
                angle =>
                {
                    float rad = angle * Mathf.Deg2Rad;
                    float x = Mathf.Cos(rad) * radiusX;
                    float y = Mathf.Sin(rad) * radiusY;
                    spoon.anchoredPosition = originalPos + new Vector2(x, y);
                },
                360f,
                ellipseDuration
            )
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    void ScaleInOut()
    {
        // Scale lên rồi scale về - loop liên tục theo chu kỳ ellipse
        Sequence scaleSeq = DOTween.Sequence();

        Vector3 scaleUp = originalScale * (1f + scaleAmount);
        Vector3 scaleDown = originalScale;

        scaleSeq.Append(spoon.DOScale(scaleUp, ellipseDuration / 2f).SetEase(Ease.InOutSine));
        scaleSeq.Append(spoon.DOScale(scaleDown, ellipseDuration / 2f).SetEase(Ease.InOutSine));

        scaleSeq.SetLoops(-1, LoopType.Restart);
    }
}
