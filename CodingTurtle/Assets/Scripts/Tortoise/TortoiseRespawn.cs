using UnityEngine;

public class TortoiseRespawn : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private GameObject tortoise;

    void Start()
    {
        // Save the tortoise's initial position and rotation
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        // Save a reference to the tortoise gameObject
        tortoise = gameObject;
    }

    void Update()
    {
        // Nothing to do in Update for now
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the tortoise has entered a collider tagged "DeathZone"
        if (other.CompareTag("DeathZone"))
        {
            // Destroy the current tortoise gameObject
            Destroy(tortoise);

            // Recreate the tortoise at the original position
            GameObject newTortoise = Instantiate(tortoise, originalPosition, originalRotation);
            newTortoise.name = tortoise.name; // Ensure the new tortoise keeps the original name
        }
    }
}
