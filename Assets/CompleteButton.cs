using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CompleteButton
    : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerUpHandler
{
    public Color normalColor = Color.white; // Default color
    public Color hoverColor = Color.green; // Hover color
    public Color clickColor = Color.yellow; // Click color
    public float duration = 0.25f; // Duration of the transition
    public Vector3 normalScale = new Vector3(1, 1, 1); // Default scale
    public Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1.1f); // Scale on hover
    public Vector3 clickScale = new Vector3(0.9f, 0.9f, 0.9f); // Scale on click

    private Image buttonImage;
    private RectTransform rectTransform;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        if (buttonImage == null)
        {
            Debug.LogError("No Image component found on the button.");
        }
        if (rectTransform == null)
        {
            Debug.LogError("No RectTransform component found on the button.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Change color and scale on hover
        buttonImage.DOColor(hoverColor, duration);
        rectTransform.DOScale(hoverScale, duration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Revert color and scale when not hovering
        buttonImage.DOColor(normalColor, duration);
        rectTransform.DOScale(normalScale, duration);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Change color and scale on click
        buttonImage.DOColor(clickColor, duration);
        rectTransform.DOScale(clickScale, duration);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Return to hover state if still hovering, otherwise return to normal state
        if (eventData.pointerEnter == gameObject)
        {
            buttonImage.DOColor(hoverColor, duration);
            rectTransform.DOScale(hoverScale, duration);
        }
        else
        {
            buttonImage.DOColor(normalColor, duration);
            rectTransform.DOScale(normalScale, duration);
        }
    }

    public void Complete() { }

    private void OnDestroy()
    {
        DOTween.Kill(this); // Kills any tween associated with this object
    }
}
