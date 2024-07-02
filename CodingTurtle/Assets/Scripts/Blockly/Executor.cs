using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Direction;

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
        steps.Add(new Direction { movement = Direction.Movements.Forward, value = value });
    }

    // Rotate the turtle to the left
    public void RotateLeft(float value)
    {
        Debug.Log($"Rotate Left - {value}");
        steps.Add(new Direction { movement = Direction.Movements.Left, value = value });
    }

    // Rotate the turtle to the right
    public void RotateRight(float value)
    {
        Debug.Log($"Rotate Right - {value}");
        steps.Add(new Direction { movement = Direction.Movements.Right, value = value });
    }

    // Attack the enemy
    public void Attack(float value)
    {
        Debug.Log($"Attack - {value}");
        steps.Add(new Direction { movement = Direction.Movements.Attack, value = value });
    }

    // Invoked when the flow is finished
    public void IsFinish(string s)
    {
        Debug.Log(s + ": Flow finished");
        string result = string.Join(", ", steps.Select(d => $"{d.movement}: {d.value}"));
        Debug.Log(result);
        // Invoke the UpdateSteps function in the TortoiseHandler
        BroadcastMessage("UpdateSteps");
    }
}
