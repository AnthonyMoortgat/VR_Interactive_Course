using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ActionSetInput : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean menuLeft;
    public SteamVR_Action_Boolean menuRight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetMenuLeft())
        {
            print("Menu Left " + handType);
        }

        if (GetMenuRight())
        {
            print("Menu Right " + handType);
        }
    }

    public bool GetMenuLeft()
    {
        return menuLeft.GetState(handType);
    }

    public bool GetMenuRight()
    {
        return menuRight.GetState(handType);
    }
}
