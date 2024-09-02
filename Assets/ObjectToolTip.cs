using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectToolTip : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public float holdDuration = 2.0f; // Time in seconds before the tooltip appears
    public float yOffsetAbove = 10f; // Offset for when the tooltip is above
    public float yOffsetBelow = 10f; // Offset for when the tooltip is below

    private bool isPointerDown = false;
    private bool isDragging = false;
    private Coroutine holdCoroutine;
    public string tooltipContent;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        isDragging = false; // Reset dragging status when the pointer goes down
        holdCoroutine = StartCoroutine(HoldToShowTooltip());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            holdCoroutine = null;
        }
        Tooltip.Instance.HideTooltip(); // Hide the tooltip when the pointer is released
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDragging = true;
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            holdCoroutine = null;
        }
        Tooltip.Instance.HideTooltip(); // Hide the tooltip when dragging starts
    }

    private IEnumerator HoldToShowTooltip()
    {
        yield return new WaitForSeconds(holdDuration);

        if (isPointerDown && !isDragging)
        {
            Tooltip.Instance.ShowTooltip(
                GetComponent<RectTransform>(),
                yOffsetAbove,
                yOffsetBelow,
                tooltipContent
            );
        }
    }
}
