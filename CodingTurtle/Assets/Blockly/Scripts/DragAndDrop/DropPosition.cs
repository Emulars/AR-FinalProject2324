using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPosition : MonoBehaviour, IDropHandler
{
    public GameObject droppedGameObject;
    private bool isAttached = false;
    private bool isActive = false;

    public void SetActive()
    {
        isActive = true;
    }

    void Update()
    {
        if (!isAttached) return;

        if (droppedGameObject.GetComponent<IBlock>().isDragged)
        {
            isAttached = false;
            droppedGameObject = null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!isActive || eventData.pointerDrag.GetComponent<IBlock>() == null) return;
        
        //auto positioning when the component is dropped
        eventData.pointerDrag.GetComponent<RectTransform>().position =
            GetComponentInParent<RectTransform>().position;

        droppedGameObject = eventData.pointerDrag;
        isAttached = true;

        eventData.pointerDrag.transform.SetParent(gameObject.transform.parent);
    }
}