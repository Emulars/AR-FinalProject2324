using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPosition : MonoBehaviour
{
    public GameObject droppedGameObject;
    public bool isAttached = false;

    public void SetActive()
    {
        // active the trigger
        GetComponent<Collider>().isTrigger = true;
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