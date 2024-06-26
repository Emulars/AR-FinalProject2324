using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Executor : MonoBehaviour
{
    // Identify the start block
    [SerializeField] private GameObject startPoint;

    // List of steps to be executed
    public static List<Direction> steps = new List<Direction>();

    // Move the turtle in a straight line
    public void MoveStraight(float value)
    {
        Debug.Log($"Move Straight - {value}");
        steps.Add(new Direction { direction = Direction.Directions.Forward, value = value });
    }

    // Rotate the turtle to the left
    public void RotateLeft(float value)
    {
        Debug.Log($"Rotate Left - {value}");
        steps.Add(new Direction { direction = Direction.Directions.Left, value = value });
    }

    // Rotate the turtle to the right
    public void RotateRight(float value)
    {
        Debug.Log($"Rotate Right - {value}");
        steps.Add(new Direction { direction = Direction.Directions.Right, value = value });
    }

    // Invoked when the flow is finished
    public void IsFinish(string s)
    {
        Debug.Log(s + ": Flow finished");
        // Invoke the UpdateSteps function in the TortoiseHandler
        BroadcastMessage("UpdateSteps");
    }
}
