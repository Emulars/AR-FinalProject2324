using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using static System.TimeZoneInfo;

public class TortoiseHandler : MonoBehaviour
{
    // Create a list of all the steps the tortoise will take in the scene
    private static List<Direction> steps = new List<Direction>() 
    {
        //new Direction() { movement = Direction.Movements.Forward, value = 1f},
        //new Direction() { movement = Direction.Movements.Right, value = 90f},
        //new Direction() { movement = Direction.Movements.Forward, value = 1f},
        //new Direction() { movement = Direction.Movements.Attack, value = 0f},
        //new Direction() { movement = Direction.Movements.Forward, value = 1f}
        
        //new Direction() { movement = Direction.Movements.Left, value = 90f},
        //new Direction() { movement = Direction.Movements.Forward, value = 1f},
        //new Direction() { movement = Direction.Movements.Forward, value = 1f},
        //new Direction() { movement = Direction.Movements.Left, value = 90f},
        //new Direction() { movement = Direction.Movements.Forward, value = 1f},
        //new Direction() { movement = Direction.Movements.Forward, value = 1f},
    };

    #region Editable Gameobject properties
    public float moveSpeed = 0.2f;          // Speed of the tortoise
    public float rotateSpeed = 360.0f;      // Rotation speed of the tortoise
    [SerializeField] private GameObject Destructable; // Container for the destructable objects
    [SerializeField] private GameObject AttackEffect;   // Attack effect
    #endregion

    #region Private variables
    private int currentMovementIndex = 0;   // Index to track current movement
    private bool isMoving = false;          // Track if currently moving
    private bool isRotating = false;        // Track if currently rotating
    private bool isAttacking = false;       // Track if currently attacking
    
    private Vector3 targetPosition;         // Target position for forward movement
    private Quaternion targetRotation;      // Target rotation for right/left rotation
    public GameObject obstacle;            // Last hitted Obstacle object

    private Animator animator;              // Animator for the tortoise
    private const int TRANSITION_IDLE_TO_ATTACK_HASH = 6363197; // Attack animation transition hash
    private const int TRANSITION_ATTACK_TO_IDLE_HASH = -1952188329; // Attack animation transition hash
    private bool waitingForTransition = false; // Flag to ensures that we only start waiting for the transition once per transition.

    private Vector3 originalPosition;       // Original position of the tortoise for the respawn
    private Quaternion originalRotation;    // Original rotation of the tortoise for the respawn
    #endregion

    private void Start()
    {
        // Get the animator component
        animator = GetComponent<Animator>();
        // Save the tortoise's initial position and rotation
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void Update()
    {
        // Check if there are any steps left
        if (currentMovementIndex >= steps.Count) return;
        
        // Check that the tortoise is not moving or rotating
        if (!isMoving && !isRotating)
        {
            Debug.Log("Current movement index: " + currentMovementIndex);
            // Get the current step
            Direction currentStep = steps[currentMovementIndex];

            // Check if the current step is a forward movement
            if (currentStep.movement == Direction.Movements.Forward)
            {
                // Calculate the target position
                targetPosition = transform.position + transform.forward * currentStep.value;
                // Start moving and  Set the animator to move
                SetIsMoving(true);
            }
            // Check if the current step is a right rotation
            else if (currentStep.movement == Direction.Movements.Right)
            {
                // Calculate the target rotation
                targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + currentStep.value, 0);
                // targetRotation = transform.rotation * Quaternion.Euler(0, currentStep.value, 0);
                // Start rotating
                isRotating = true;
            }
            // Check if the current step is a left rotation
            else if (currentStep.movement == Direction.Movements.Left)
            {
                // Calculate the target rotation
                targetRotation = Quaternion.Euler(0, transform.eulerAngles.y - currentStep.value, 0);
                // targetRotation = transform.rotation * Quaternion.Euler(0, -currentStep.value, 0);
                // Start rotating
                isRotating = true;
            }
        
            // Checl if the current step is an attack
            else if (currentStep.movement == Direction.Movements.Attack)
            {
                SetIsAttaccking(true);
            }
        }

        // Check if the tortoise is moving
        if (isMoving)
        {
            // Debug.Log("Moving");
            // Move towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                // Reached the target position
                SetIsMoving(false);
                currentMovementIndex++;
            }
        }
        // Check if the tortoise is rotating
        else if (isRotating)
        {
            // Debug.Log("Rotating");
            // Rotate towards the target rotation
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                // Reached the target rotation
                isRotating = false;
                currentMovementIndex++;
            }
        }
        // Check if the tortoise is attacking
        else if (isAttacking)
        {
            if (!waitingForTransition)
            {
                StartCoroutine(WaitForTransition());
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the tortoise has entered a collider tagged "DeathZone"
        if (other.CompareTag("DeathZone")) DeathZone();
        // Check if the tortoise has entered a collider tagged "Destructable"
        // if (other.CompareTag("Destructable")) DestructableZone(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        // Check if the tortoise has entered a collider tagged "Destructable"
        if (other.CompareTag("Destructable")) DestructableZone(other.gameObject);
    }


    /// <summary>
    /// Coroutine to wait for the attack animation to finish
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForTransition()
    {
        Debug.Log("Waiting for transition");
        waitingForTransition = true;
        yield return new WaitUntil(() => IsInTransition(TRANSITION_ATTACK_TO_IDLE_HASH));
        Debug.Log("Animator is in the desired transition: " + TRANSITION_ATTACK_TO_IDLE_HASH);
        // Do something after the transition
        OnAttackAnimationFinish();
        waitingForTransition = false;
    }

    /// <summary>
    /// Check if the animator is in the desired transition
    /// </summary>
    /// <param name="transitionHash"></param>
    /// <returns></returns>
    private bool IsInTransition(int transitionHash)
    {
        animator.Update(0);
        if (animator.IsInTransition(0))
        {
            AnimatorTransitionInfo transitionInfo = animator.GetAnimatorTransitionInfo(0);
            //Debug.Log("Transition hash name: " + transitionInfo.nameHash);

            if (transitionInfo.nameHash == TRANSITION_IDLE_TO_ATTACK_HASH)
            {
                // SetActive = true to the attack effect object
                AttackEffect.SetActive(true);
            }

            return transitionInfo.nameHash == transitionHash;
            //return transitionInfo.IsName(transitionName);
        }
        return false;
    }

    /// <summary>
    /// Function to handle the attack animation finish
    /// </summary>
    private void OnAttackAnimationFinish()
    {
        Debug.Log("Animation has finished.");
        // Attack animation has finished
        SetIsAttaccking(false);
        // SetActive = false to the last hitted obstacle object
        obstacle.SetActive(false);
        // Go to the next step
        currentMovementIndex++;
        // SetActive = false to the attack effect object
        AttackEffect.SetActive(false);
    }

    /// <summary>
    /// Function to handle the death zone
    /// </summary>
    private void DeathZone()
    {
        // Reset the steps list
        ResetSteps();
        // Respawn the tortoise at its initial position and rotation
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        // Reset the obstacles
        ResetObstacles();
    }

    /// <summary>
    /// Function to handle the destructable zone
    /// </summary>
    /// <param name="gameObject"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void DestructableZone(GameObject gameObject)
    {
        Debug.Log("Destructable Zone");
        // When the tortoise enters the destructable zone, stop moving
        SetIsMoving(false);

        // If the tortoise is not attacking
        if (!isAttacking)
        {
            // Go to the next step
            currentMovementIndex++;

            // Set the obstacle as the hitted object
            obstacle = gameObject;
        }
    }

    /// <summary>
    /// Function to set the IsMoving parameter in the animator
    /// </summary>
    /// <param name="value"></param>
    void SetIsMoving(bool value)
    {
        isMoving = value;
        animator.SetBool("IsMoving", value);
    }

    void SetIsAttaccking(bool value)
    {
        isAttacking = value;
        animator.SetBool("IsAttacking", value);
    }

    /// <summary>
    /// Function invoked to update the steps list from the executor
    /// </summary>
    public void UpdateSteps()
    {
        // reset the step list to empty
        ResetSteps();
        // get the steps from the executor
        steps = Executor.steps;
    }

    /// <summary>
    /// Function to reset the steps list
    /// </summary>
    private void ResetSteps()
    {
        steps = new List<Direction>();
        currentMovementIndex = 0;
        SetIsMoving(false);
        isRotating = false;
        SetIsAttaccking(false);
    }
    
    /// <summary>
    /// Function to reset the obstacles
    /// </summary>
    private void ResetObstacles()
    {
        if (Destructable != null)
        {
            // SetActive = True for all the destructable object children
            foreach (Transform child in Destructable.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
