using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public Transform parentAfterDrag;
    public Image image;
    public IngredientPlate plate;

    private Vector2 offset;

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;

        // Adjust position to account for the offset between the pointer/touch and the object center
        Vector2 pointerPosition = GetPointerPosition(eventData);
        transform.position = pointerPosition;
        Debug.Log(transform.position);
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pointerPosition = GetPointerPosition(eventData);
        transform.position = pointerPosition;
        Debug.Log(transform.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
         Vector2 pointerPosition = GetPointerPosition(eventData);
        transform.position = pointerPosition + offset;
        Debug.Log(transform.position);
    }
    
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }

    private Vector2 GetPointerPosition(PointerEventData eventData)
    {
        Vector2 pointerPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        return pointerPosition;
    }
}
