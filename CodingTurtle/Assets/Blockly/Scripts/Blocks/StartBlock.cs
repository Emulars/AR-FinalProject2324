using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBlock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<DropPosition>().SetActive();
    }

    private void OnMouseDown()
    {
        Execute();
    }

    // Execute the block flow logic
    public void Execute()
    {
        var next = GetComponentInChildren<DropPosition>().droppedGameObject;
        if (next != null)
        {
            next.GetComponent<IBlock>().Execute();
        }
    }
}
