using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockDirection : MonoBehaviour, IBlock
{
    public bool isDragged { get; set; }
    public bool isInMain { get; set; }

    [SerializeField] private float value;
    [SerializeField] private Direction.Directions direction; // Straight, Left, Right
    private int multiplier = 1;

    private void Start()
    {
        isDragged = false;
        isInMain = false;
    }

    public void Execute()
    {
        Transform[] children = GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child.name == "Multiplier")
            {
                Debug.Log("InputField found");
                string inputText = child.GetComponent<Text>().text;
                
                // Check if the inputText is an integer
                if (int.TryParse(inputText, out int result))
                {
                    Debug.Log("InputText is an integer");
                    multiplier = result;
                }
                Debug.Log("Multiplier: " + multiplier);
                // after finding the first input field, break the loop to avoid checking following blocks' input fields
                break;
            }
        }

        // Straight
        if (direction.ToString().ToLower() == "forward") SendMessageUpwards("MoveStraight", value * multiplier);
        // Left
        else if (direction.ToString().ToLower() == "left") SendMessageUpwards("RotateLeft", value);
        // Right
        else if (direction.ToString().ToLower() == "right") SendMessageUpwards("RotateRight", value);

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the block enter a trigger and the trigger is MainScript, then the block is in the main canvas
        if (other.isTrigger && other.CompareTag("Main"))
        {
            Debug.Log("An object entered a Main trigger.");
            isInMain = true;
        }

        // If the block enter a trigger and the trigger is Trash, then the block will be destroyed
        if (other.isTrigger && other.CompareTag("Trash"))
        {
            Debug.Log("An object entered a Trash trigger.");
            Destroy(gameObject);
        }
    }
}
