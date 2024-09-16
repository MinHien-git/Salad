using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentAfterDrag;
    public Image image;
    public IngredientPlate plate;

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        Vector2 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        image.raycastTarget = false;

        transform.position = temp;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = temp;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }
}
