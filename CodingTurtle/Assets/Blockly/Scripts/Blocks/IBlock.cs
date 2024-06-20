using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlock
{
    public bool isDragged { get; set; } //To check if the block is being dragged
    public bool isInMain { get; set; }  // To check in which canvas section the block is, Main or SideBar

    public float rotationValue { get; set; } //Value to rotate the turtle

    public float moveValue { get; set; } //Value to move the turtle

    public void Execute();  //block core

}
