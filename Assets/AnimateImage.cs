using System.Collections;
using System.Collections.Generic;
using DG.Tweening; // Import DOTween
using UnityEngine;
using UnityEngine.UI;

public class AnimateImage : MonoBehaviour
{
    public Image headerText;
    public Color color;

    void Start()
    {
        // Start animation when the script starts
        AnimateHeaderText();
    }

    void AnimateHeaderText()
    {
        // Scale the text up and down for a bounce effect, looping infinitely
        headerText.transform.DOScale(new Vector3(1.2f, 1.2f, 1f), .5f).SetLoops(-1, LoopType.Yoyo);

        // Fade in and out the text color between white and blue, looping infinitely
        headerText.DOColor(color, .75f).SetLoops(-1, LoopType.Yoyo);

        // Optional: Loop the text change, but you can leave it out if unnecessary
    }
}
