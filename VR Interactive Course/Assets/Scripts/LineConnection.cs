using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineConnection
{
    public GameObject StartObject { get; set; }
    public GameObject EndObject { get; set; }
    public GameObject Line { get; set; }

    public LineConnection(GameObject startObject, GameObject endObject, GameObject line)
    {
        StartObject = startObject;
        EndObject = endObject;
        Line = line;
    }
}
