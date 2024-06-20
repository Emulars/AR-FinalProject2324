using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropInTrashCan : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<IBlock>() == null) return; //cannot drop not IBlock obj
        Destroy(eventData.pointerDrag);
    }
}
