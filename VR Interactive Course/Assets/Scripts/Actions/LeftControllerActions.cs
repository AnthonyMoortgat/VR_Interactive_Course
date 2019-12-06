using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class LeftControllerActions : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;

    private GameObject collidingObject; // 1
    private GameObject objectInHand; // 2

    // Start is called before the first frame update
    void Start()
    {
        //Add listner
        grabAction.AddOnStateUpListener(LoseObject, handType);
        grabAction.AddOnStateDownListener(GrabObject, handType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GrabObject(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Grip is down");

        if (collidingObject)
        {
            objectInHand = collidingObject;
            collidingObject = null;

            //Test
            objectInHand.GetComponent<Rigidbody>().isKinematic = false;

            objectInHand.transform.parent = null;

            // 2
            var joint = AddFixedJoint();
            joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
        }
    }

    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    public void LoseObject(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Grip is up");

        if (objectInHand)
        {

            if (GetComponent<FixedJoint>())
            {
                // 2
                GetComponent<FixedJoint>().connectedBody = null;
                Destroy(GetComponent<FixedJoint>());

                //Test
                objectInHand.GetComponent<Rigidbody>().isKinematic = true;

                // 3
                /*
                objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
                objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();
                */
            }
            // 4
            objectInHand = null;
        }
    }

    // Change the interactableObject
    private void SetCollidingObject(Collider col)
    {
        // 1
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }
        // 2
        collidingObject = col.gameObject;
    }

    // 1
    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    // 2
    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    // 3
    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }

        collidingObject = null;
    }
}
