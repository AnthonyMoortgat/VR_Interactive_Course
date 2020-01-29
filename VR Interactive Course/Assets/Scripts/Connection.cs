using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection
{
    public GameObject ModelOne { get; set; }
    public GameObject ModelTwo { get; set; }
    public bool AlreadyPresent { get; set; }

    public Connection(GameObject modelOne, GameObject modelTwo)
    {
        ModelOne = modelOne;
        ModelTwo = modelTwo;
        AlreadyPresent = false;
    }
}
