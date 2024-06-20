using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;

    // Duplication
    [Header("Workbench")]
    [SerializeField] private GameObject workingPlane;
    [SerializeField] private float scaleMultiplayer = 1f;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var itemBeingDragged = eventData.pointerDrag;
        itemBeingDragged.GetComponent<IBlock>().isDragged = true;

        if (transform.parent == workingPlane.transform)
            return;

        // Duplicate block if in the SideBar
        if (!itemBeingDragged.GetComponent<IBlock>().isInMain)
        {
            GameObject duplicate = Instantiate(itemBeingDragged, itemBeingDragged.transform.parent, false);
            duplicate.name = itemBeingDragged.name;
            OnEndDrag(duplicate);
        }
        // Set the parent to the working plane
        itemBeingDragged.transform.SetParent(workingPlane.transform);
        
        // Enalbe all drop positions of the block being dragged
        var dropPositions = itemBeingDragged.GetComponentsInChildren<DropPosition>();
        foreach (var drop in dropPositions)
        {
            drop.SetActive();
        }
    }

    public void OnDrag(PointerEventData eventData) => rectTransform.anchoredPosition += eventData.delta / (scaleMultiplayer);

    public void OnEndDrag(PointerEventData eventData) => eventData.pointerDrag.GetComponent<IBlock>().isDragged = false;

    public void OnEndDrag(GameObject gameObject) => gameObject.GetComponent<IBlock>().isDragged = false;

}
