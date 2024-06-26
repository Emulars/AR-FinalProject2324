using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class FinishLine : MonoBehaviour
{

    // On trigger enter enable the collider Point Light component
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Tower")
        {
            Debug.Log("Tortoise has reached the finish line");
            // Enable the Point Light component
            other.transform.GetChild(6).gameObject.SetActive(true);
        }
    }
}
