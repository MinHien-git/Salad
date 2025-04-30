using DG.Tweening;
using UnityEngine;

public class SnappyBackAndForth : MonoBehaviour
{
    public float angle = 30f; // Maximum angle (Z-axis)
    public float rotateTime = 0.1f; // Quick rotation time
    public float waitTime = 0.1f; // Wait time at each end

    private RectTransform rectTransform;
    private bool flip = true;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // Start the loop
        StartRotationLoop();
    }

    void StartRotationLoop()
    {
        Sequence seq = DOTween.Sequence();

        // Initial move to +angle
        seq.Append(
                rectTransform.DORotate(new Vector3(0, 0, angle), rotateTime).SetEase(Ease.Linear)
            )
            .AppendInterval(waitTime);

        // Infinite flip between +angle and -angle
        seq.AppendCallback(() => FlipRotation());
    }

    void FlipRotation()
    {
        Sequence seq = DOTween.Sequence();

        // Alternate between +angle and -angle
        float targetAngle = flip ? -angle : angle;
        flip = !flip;

        seq.Append(
                rectTransform
                    .DORotate(new Vector3(0, 0, targetAngle), rotateTime)
                    .SetEase(Ease.Linear)
            )
            .AppendInterval(waitTime)
            .AppendCallback(() => FlipRotation());
    }
}
