using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour
{
    // Duplication
    [Header("Workbench")]
    [SerializeField] 
    private GameObject workingPlane;
    private GameObject duplicate;
    private Vector3 mousePosition;

    private Vector3 GetMousePos() => Camera.main.WorldToScreenPoint(transform.position);

    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition - GetMousePos();
    }

    private void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);

        GetComponent<IBlock>().isDragged = true;

        if (transform.parent == workingPlane.transform)
            return;

        // Duplicate block if in the SideBar
        if (!GetComponent<IBlock>().isInMain)
        {
            duplicate = Instantiate(gameObject, transform.parent, false);
            duplicate.name = gameObject.name;
            OnMouseUp();
        }

        // Set the parent to the working plane
        gameObject.transform.SetParent(workingPlane.transform);

        // Enalbe all drop positions of the block being dragged
        var dropPositions = gameObject.GetComponentsInChildren<DropPosition>();
        foreach (var drop in dropPositions)
        {
            drop.SetActive();
        }

    }
    
    private void OnMouseUp() => duplicate.GetComponent<IBlock>().isDragged = false;
}
