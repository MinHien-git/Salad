using System.Collections;
using System.Collections.Generic;
using DG.Tweening; // Import DOTween
using TMPro;
using UnityEngine;

public class AnimateHeader : MonoBehaviour
{
    public TextMeshProUGUI headerText;

    void Start()
    {
        // Start animation when the script starts
        AnimateHeaderText();
    }

    void AnimateHeaderText()
    {
        // Scale the text up and down for a bounce effect, looping infinitely
        headerText.transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 1f).SetLoops(-1, LoopType.Yoyo);

        // Fade in and out the text color between white and blue, looping infinitely
        headerText.DOColor(Color.black, 1f).SetLoops(-1, LoopType.Yoyo);

        // Optional: Loop the text change, but you can leave it out if unnecessary
    }
}
