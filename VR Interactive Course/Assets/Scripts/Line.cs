using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line
{
    public GameObject LineObject { get; set; }
    public int Position { get; set; }

    public Line(GameObject lineObject, int position)
    {
        LineObject = lineObject;
        Position = position;
    }
}
