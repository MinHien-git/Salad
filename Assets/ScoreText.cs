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
        text.DOColor(Color.green, 0.2f).OnComplete(() => text.DOColor(Color.white, 0.3f)); // Return to original color
    }

    // Animation to play when the score decreases
    public void AnimateScoreTextDecrease()
    {
        // Scale the score text down slightly and then back to normal
        text.transform.DOScale(new Vector3(0.8f, 0.8f, 1f), 0.3f)
            .OnComplete(() => text.transform.DOScale(Vector3.one, 0.3f)); // Return to original size

        // Change the color of the text to red during the animation
        text.DOColor(Color.red, 0.2f).OnComplete(() => text.DOColor(Color.white, 0.3f)); // Return to original color
    }
}
