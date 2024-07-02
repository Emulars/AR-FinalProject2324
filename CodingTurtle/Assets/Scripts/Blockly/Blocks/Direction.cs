using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction
{
    public enum Movements
    {
        Forward,
        Left,
        Right,
        Attack
    }
    public Movements movement { get; set; }
    public float value { get; set; }
}
