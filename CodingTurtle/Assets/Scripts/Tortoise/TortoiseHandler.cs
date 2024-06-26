using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TortoiseHandler : MonoBehaviour
{
    // Create a list of all the steps the tortoise will take in the scene
    private static List<Direction> steps = new List<Direction>() 
    {
        //new Direction() { direction = Direction.Directions.Forward, value = 1f},
        //new Direction() { direction = Direction.Directions.Right, value = 90f},
        //new Direction() { direction = Direction.Directions.Forward, value = 1f},
        //new Direction() { direction = Direction.Directions.Left, value = 90f},
        //new Direction() { direction = Direction.Directions.Forward, value = 1f},
        //new Direction() { direction = Direction.Directions.Forward, value = 1f},
        //new Direction() { direction = Direction.Directions.Left, value = 90f},
        //new Direction() { direction = Direction.Directions.Forward, value = 1f},
        //new Direction() { direction = Direction.Directions.Forward, value = 1f},
    };
    // Speed of the tortoise
    public float moveSpeed = 5.0f;
    // Rotation speed of the tortoise
    public float rotateSpeed = 360.0f;

    // Index to track current movement
    private int currentMovementIndex = 0;
    // Track if currently moving or rotating
    private bool isMoving = false;
    private bool isRotating = false;
    // Target position for forward movement
    private Vector3 targetPosition;
    // Target rotation for right/left rotation
    private Quaternion targetRotation;

    // Animator for the tortoise
    private Animator animator;

    private void Start()
    {
        // Get the animator component
        animator = GetComponent<Animator>();
        // get the steps from the executor
        steps = Executor.steps;
    }

    /// <summary>
    /// Function invoked to update the steps list from the executor
    /// </summary>
    public void UpdateSteps()
    {
        // reset the step list to empty
        steps = new List<Direction>();
        // get the steps from the executor
        steps = Executor.steps;
        // reset the current movement index
        currentMovementIndex = 0;
        isMoving = false;
        isRotating = false;
    }

    private void Update()
    {
        // Check if there are any steps left
        if (currentMovementIndex >= steps.Count)
        {
            Debug.Log("No more steps left");
            return;
        }

        // Check that the tortoise is not moving or rotating
        if (!isMoving && !isRotating)
        {
            Debug.Log("Current movement index: " + currentMovementIndex);
            // Get the current step
            Direction currentStep = steps[currentMovementIndex];

            // Check if the current step is a forward movement
            if (currentStep.direction == Direction.Directions.Forward)
            {
                // Calculate the target position
                targetPosition = transform.position + transform.forward * currentStep.value;
                // Start moving
                isMoving = true;
                // Set the animator to move
                animator.SetBool("IsMoving", true);
            }
            // Check if the current step is a right rotation
            else if (currentStep.direction == Direction.Directions.Right)
            {
                // Calculate the target rotation
                targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + currentStep.value, 0);
                // targetRotation = transform.rotation * Quaternion.Euler(0, currentStep.value, 0);
                // Start rotating
                isRotating = true;
            }
            // Check if the current step is a left rotation
            else if (currentStep.direction == Direction.Directions.Left)
            {
                // Calculate the target rotation
                targetRotation = Quaternion.Euler(0, transform.eulerAngles.y - currentStep.value, 0);
                // targetRotation = transform.rotation * Quaternion.Euler(0, -currentStep.value, 0);
                // Start rotating
                isRotating = true;
            }
        }

        // Check if the tortoise is moving
        if (isMoving)
        {
            Debug.Log("Moving");
            // Move towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                // Reached the target position
                isMoving = false;
                // Set the animator to stop moving
                animator.SetBool("IsMoving", false);
                currentMovementIndex++;
            }
        }
        // Check if the tortoise is rotating
        else if (isRotating)
        {
            Debug.Log("Rotating");
            // Rotate towards the target rotation
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                // Reached the target rotation
                isRotating = false;
                currentMovementIndex++;
            }
        }
    }
}
