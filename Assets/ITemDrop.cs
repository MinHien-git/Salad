using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ITemDrop : MonoBehaviour, IDropHandler
{
    public virtual void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DragAndDrop dragAndDrop = dropped.GetComponent<PlateDragAndDrop>();
        if (transform.CompareTag("TableArea"))
        {
            if (GameManager.Instance.CanPlacePlate())
                return;

            dragAndDrop.parentAfterDrag = transform;
            GameManager.Instance.AddPlate(dragAndDrop.plate);
        }
        else
        {
            dragAndDrop.parentAfterDrag = transform;
            GameManager.Instance.RemovePlate(dragAndDrop.plate);
        }
    }
}
