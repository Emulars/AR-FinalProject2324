using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction
{
    public enum Directions
    {
        Forward,
        Left,
        Right
    }
    public Directions direction { get; set; }
    public float value { get; set; }
}
