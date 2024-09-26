using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlateHoldAndPlaceEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Color normalColor = new Color(0.8f, 0.8f, 0.8f); // Default color
    public Color holdColor = new Color(0.6f, 0.6f, 0.6f); // Color when holding
    public float holdScale = 0.9f; // Scale when holding
    public float placeScale = 1.1f; // Scale when placing
    public float normalScale = 1.0f; // Default scale
    public float holdDuration = 0.15f; // Duration of the hold effect
    public float placeDuration = 0.25f; // Duration of the place effect

    public Image plateRenderer;
    private Transform plateTransform;

    void Start()
    {
        plateTransform = GetComponent<Transform>();

        if (plateRenderer == null)
        {
            Debug.LogError("No Renderer component found on the plate.");
        }
        if (plateTransform == null)
        {
            Debug.LogError("No Transform component found on the plate.");
        }

        // Set the initial color
        plateRenderer.color = normalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Trigger the hold effect (e.g., scale down and change color)
        plateRenderer.DOColor(holdColor, holdDuration);
        plateTransform.DOScale(holdScale, holdDuration).SetEase(Ease.OutBack);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Trigger the place effect (e.g., scale up and change color)
        plateRenderer.DOColor(normalColor, placeDuration);
        plateTransform
            .DOScale(placeScale, placeDuration)
            .OnComplete(() =>
            {
                // Once the place effect is done, return to normal scale
                plateTransform.DOScale(normalScale, placeDuration).SetEase(Ease.OutBack);
            });
    }

    private void OnDestroy()
    {
        DOTween.Kill(this); // Kills any tween associated with this object
    }
}
