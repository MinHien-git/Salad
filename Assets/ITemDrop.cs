using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ITemDrop : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DragAndDrop dragAndDrop = dropped.GetComponent<DragAndDrop>();
        dragAndDrop.parentAfterDrag = transform;
    }
}
