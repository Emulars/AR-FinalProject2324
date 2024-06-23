using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlock
{
    public bool isDragged { get; set; } //To check if the block is being dragged
    public bool isInMain { get; set; }  // To check in which canvas section the block is, Main or SideBar

    public void Execute();  //block core
}
