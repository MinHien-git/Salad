using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    public static ScoreText Instance { get; set; }
    public TextMeshProUGUI text;

    void Awake()
    {
        Instance = this;
    }

    // Animation to play when the score changes
    public void AnimateScoreTextIncrease()
    {
        // Scale the score text up and down for a pop effect
        text.transform.DOScale(new Vector3(1.25f, 1.25f, 1f), 0.3f)
            .OnComplete(() => text.transform.DOScale(Vector3.one, 0.3f)); // Return to original size

        // Change the color of the text to green during the animation
        text.DOColor(new Color(28 / 255f, 166 / 255f, 63 / 255f), 0.2f)
            .OnComplete(() => text.DOColor(new Color(242 / 255f, 80 / 255f, 110 / 255f), 0.3f)); // Return to original color
    }

    // Animation to play when the score decreases
    public void AnimateScoreTextDecrease()
    {
        // Scale the score text down slightly and then back to normal
        text.transform.DOScale(new Vector3(0.8f, 0.8f, 1f), 0.3f)
            .OnComplete(() => text.transform.DOScale(Vector3.one, 0.3f)); // Return to original size

        // Change the color of the text to red during the animation
        text.DOColor(new Color(242 / 255f, 68 / 255f, 5 / 255f), 0.2f)
            .OnComplete(() => text.DOColor(new Color(242 / 255f, 80 / 255f, 110 / 255f), 0.3f)); // Return to original color
    }

    private void OnDestroy()
    {
        DOTween.Kill(this); // Kills any tween associated with this object
    }
}
