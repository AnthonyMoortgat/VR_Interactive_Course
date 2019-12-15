using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Valve.VR;

public class LeftControllerActions : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Action_Boolean drawAction;

    private GameObject collidingObject; // 1
    private GameObject objectInHand; // 2

    //Draw line
    // private bool drawing = false;
    private LineRenderer lineRenderer;
    private GameObject currentLine;
    private GameObject objectStart;

    // Start is called before the first frame update
    void Start()
    {
        //Add listner
        grabAction.AddOnStateUpListener(LoseObject, handType);
        grabAction.AddOnStateDownListener(GrabObject, handType);

        drawAction.AddOnStateDownListener(DrawLine, handType);
        drawAction.AddOnStateUpListener(StopDrawing, handType);
    }

    // Update is called once per frame
    void Update()
    {
        if(drawAction.GetState(handType) == true && objectStart)
        {
            // update position of line renderer
            lineRenderer.SetPosition(1, gameObject.transform.position);
        }
    }

    public void DrawLine(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        // Check model is in trigger area
        // If true => Drawing = true
        // Generate game object
        // Generete line renderer and set position to center of model and controller

        if (collidingObject && collidingObject.transform.parent == null)
        {
            
            objectStart = collidingObject;

            currentLine = new GameObject();
            lineRenderer = currentLine.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            // lineRenderer.widthMultiplier = 0.2f;
            lineRenderer.startWidth = 0.01f;
            lineRenderer.endWidth = 0.01f;

            var points = new Vector3[2];
            points[0] = objectStart.transform.position;
            points[1] = gameObject.transform.position;

            lineRenderer.SetPositions(points);
        }
    }

    public void StopDrawing(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (!collidingObject || collidingObject == objectStart)
        {
            Destroy(currentLine);
            currentLine = null;
            objectStart = null;
        } else if(collidingObject.transform.parent != null)
        {
            Destroy(currentLine);
            currentLine = null;
            objectStart = null;
        }
    }

    public void GrabObject(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
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
