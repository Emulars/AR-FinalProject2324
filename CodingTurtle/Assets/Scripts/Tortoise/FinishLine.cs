using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private GameObject nextLevelPrefab;
    [SerializeField] private GameObject CurrentLevel;

    private Vector3 levelPosition;
    private Quaternion levelRotation;
    private Transform parentTransform;
    private GameObject tortoise;

    private void Start()
    {
        // Get the current level transform position and rotation
        levelPosition = CurrentLevel.transform.position;
        levelRotation = CurrentLevel.transform.rotation;

        // Get the parent transform of the current level
        parentTransform = CurrentLevel.transform.parent;

        // Find the sibling GameObject (assuming it's named "SiblingGameObject")
        tortoise = CurrentLevel.transform.Find("Tortoise_Boss_Anims").gameObject;
    }

    // On trigger enter enable the collider Point Light component
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Tower")
        {
            Debug.Log("Tortoise has reached the finish line");
            // Enable the Point Light component
            other.transform.GetChild(6).gameObject.SetActive(true);

            if (nextLevelPrefab != null)
            {
                // Reset the steps to avoid the previous steps' level being executed
                tortoise.BroadcastMessage("ResetSteps");
                // After 2 seconds load the next level
                Invoke("LoadNextLevel", 2f);
            }
        }
    }

    // Method to load the next level
    private void LoadNextLevel()
    {
        // Deactivate current level
        CurrentLevel.SetActive(false);

        // Instantiate and position the next level
        Instantiate(nextLevelPrefab, levelPosition, levelRotation, parentTransform);

        // Destroy the current level
        Destroy(CurrentLevel);
    }
}
