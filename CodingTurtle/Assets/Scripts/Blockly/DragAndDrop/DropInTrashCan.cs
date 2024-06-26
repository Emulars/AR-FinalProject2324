using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropInTrashCan : MonoBehaviour
{
    // When a block enter the trigger of the trash can, it will be destroyed
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IBlock>() == null) return;
        Destroy(other.gameObject);
    }


}
