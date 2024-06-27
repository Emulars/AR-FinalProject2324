using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    public CanvasGroup canvasGroup;

    [Header("Canvas")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private float scaleMultiplier = 1f;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    // Begin drag the block
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        var itemBeingDragged = eventData.pointerDrag;
        itemBeingDragged.GetComponent<IBlock>().isDragged = true;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        // Set the parent before drag
        if (transform.parent == canvas.transform)
            return;

        // Duplicate block if not in the Main
        if (!itemBeingDragged.GetComponent<IBlock>().isInMain)
        {
            GameObject duplicate = Instantiate(itemBeingDragged, itemBeingDragged.transform.parent, false);
            duplicate.name = itemBeingDragged.name;
            OnEndDragDuplicate(duplicate);
        }

        // Set the parent after drag
        itemBeingDragged.transform.SetParent(canvas.transform);

        // Enable the drop position
        var dropPositions = itemBeingDragged.GetComponentsInChildren<DropPosition>();
        foreach (var drop in dropPositions)
        {
            drop.SetActive();
        }
    }

    // Drag the block
    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("OnDrag");
        gameObject.GetComponent<IBlock>().isDragged = true;
        rectTransform.anchoredPosition += eventData.delta / (canvas.scaleFactor * scaleMultiplier);
    }

    // End drag the block
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        eventData.pointerDrag.GetComponent<IBlock>().isDragged = false;
    }

    private void OnEndDragDuplicate(GameObject gameObject)
    {
        gameObject.GetComponent<DragDrop>().canvasGroup.alpha = 1f;
        gameObject.GetComponent<DragDrop>().canvasGroup.blocksRaycasts = true;
        gameObject.GetComponent<IBlock>().isDragged = false;
    }
}
