using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPosition : MonoBehaviour, IDropHandler
{
    public GameObject droppedGameObject;
    public bool isAttached = false;
    private bool isActive = false;

    public void OnDrop(PointerEventData eventData)
    {
        if (!isActive || eventData.pointerDrag.GetComponent<IBlock>() == null) return;

        RectTransform dropTransform = GetComponent<RectTransform>();
        RectTransform draggedTransform = eventData.pointerDrag.GetComponent<RectTransform>();

        // Align position
        draggedTransform.position = dropTransform.position;

        // Optionally align rotation and scale
        draggedTransform.rotation = dropTransform.rotation;

        droppedGameObject = eventData.pointerDrag;
        isAttached = true;

        eventData.pointerDrag.transform.SetParent(gameObject.transform.parent);
    }

    public void SetActive()
    {
        // Activate the block drop position
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
}
