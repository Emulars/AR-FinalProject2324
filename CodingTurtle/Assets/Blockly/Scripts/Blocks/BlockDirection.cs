using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDirection : MonoBehaviour, IBlock
{
    public bool isDragged { get; set; }
    public bool isInMain { get; set; }

    [SerializeField] private float value;
    [SerializeField] private string direction; // Straight, Left, Right

    private void Start()
    {
        isDragged = false;
        isInMain = false;
    }

    public void Execute()
    {
        // Straight
        if (direction.ToLower() == "straight") SendMessageUpwards("MoveStraight", value);
        // Left
        else if (direction.ToLower() == "left") SendMessageUpwards("RotateLeft", value);
        // Right
        else if (direction.ToLower() == "right") SendMessageUpwards("RotateRight", value);

        // Check if the block is the last one
        var next = GetComponentInChildren<DropPosition>().droppedGameObject;
        if (next != null)
        {
            next.GetComponent<IBlock>().Execute();
        }
        else
        {
            SendMessageUpwards("IsFinish", gameObject.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the block enter a trigger and the trigger is MainScript, then the block is in the main canvas
        if (other.isTrigger && other.CompareTag("MainScript"))
        {
            Debug.Log("An object entered a MainScript trigger.");
            isInMain = true;
        }

        // If the block enter a trigger and the trigger is Trash, then the block will be destroyed
        if (other.isTrigger && other.CompareTag("Trash"))
        {
            Debug.Log("An object entered a Trash trigger.");
            Destroy(gameObject);
        }

        // If the block enter a trigger and the trigger is DropPosition, then the block will be attached to the DropPosition
        if (other.isTrigger && other.GetComponent<DropPosition>() != null)
        {
            Debug.Log("An object entered a DropPosition trigger.");
            other.GetComponent<DropPosition>().droppedGameObject = gameObject;
            // Auto positioning when the component enters the trigger
            transform.position = other.transform.position;
            // Set the parent of the object to the DropPosition
            transform.SetParent(other.transform.parent);
            // Set the object to be attached
            other.GetComponent<DropPosition>().isAttached = true;
        }
    }
}
