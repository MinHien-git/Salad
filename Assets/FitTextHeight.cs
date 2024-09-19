using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FitTextHeight : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public RectTransform rectTransform;

    void Start()
    {
        AdjustHeight();
    }

    public void AdjustHeight()
    {
        // Force TextMeshPro to update the text layout
        textMeshPro.ForceMeshUpdate();

        // Get the preferred height of the text content
        float preferredHeight = textMeshPro.textBounds.size.y;

        // Adjust the RectTransform height to match the text content height
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, preferredHeight + 40);
    }
}
