using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlateDragAndDrop : DragAndDrop
{
    public override void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }
}
