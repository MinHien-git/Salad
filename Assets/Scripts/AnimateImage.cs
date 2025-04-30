using System.Collections;
using System.Collections.Generic;
using DG.Tweening; // Import DOTween
using UnityEngine;
using UnityEngine.UI;

public class AnimateImage : MonoBehaviour
{
    public Image headerText;
    public Color color;
    public float delay;

    void Start()
    {
        // Start animation when the script starts
        AnimateHeaderText();
    }

    void AnimateHeaderText()
    {
        // Delay before starting animations

        // Scale with delay
        headerText
            .transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.5f)
            .SetDelay(delay)
            .SetLoops(-1, LoopType.Yoyo);

        // Color fade with delay
        headerText.DOColor(color, 0.75f).SetDelay(delay).SetLoops(-1, LoopType.Yoyo);
    }
}
