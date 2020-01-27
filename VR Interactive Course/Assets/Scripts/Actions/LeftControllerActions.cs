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
    private List<Line> listLines = new List<Line>();
    private GameObject objectStart;
    private List<LineConnection> listConnections = new List<LineConnection>();

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

        if(grabAction.GetState(handType) == true && listLines.Count > 0)
        {
            //Update lines position
            foreach(Line line in listLines)
            {
                lineRenderer = line.LineObject.GetComponent<LineRenderer>();
                lineRenderer.SetPosition(line.Position, gameObject.transform.position);
            }
        }
    }

    public void DrawLine(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
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
        // 1) Line must connect to a Object
        // 2) Object can't be the same object from where the line starts
        // 3) Object can't be in the menu
        if (!collidingObject || collidingObject == objectStart || collidingObject.transform.parent != null)
        {
            Destroy(currentLine);
            currentLine = null;
            objectStart = null;
        } else
        {
            bool lineDrawn = false;

            // Check if line is a correct connection
            foreach(Connection connection in LineConnectionList.myListConnections)
            {
                // Is this line possible
                if((objectStart == connection.ModelOne && collidingObject == connection.ModelTwo) || (objectStart == connection.ModelTwo && collidingObject == connection.ModelOne)){
                    Debug.Log("This line is possible");
                    
                    // Does this line already exist
                    if (!connection.AlreadyPresent)
                    {
                        Debug.Log("This line doesn't exist");
                        lineDrawn = true;

                        // Line is possible and doesn't exist
                        LineConnection lineConnection = new LineConnection(objectStart, collidingObject, currentLine);
                        listConnections.Add(lineConnection);

                        connection.AlreadyPresent = true;

                        // Change collor of linerenderer
                        currentLine.GetComponent<LineRenderer>().material.color = Color.green;
                    }
                }
            }

            if (!lineDrawn)
            {
                // Line does not exist or is already drawn
                // Launch error sound
                AudioManager.instance.Play("Error");

                Destroy(currentLine);
                currentLine = null;
                objectStart = null;
            }
        }
    }

    public void GrabObject(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (collidingObject)
        {
            // Does object have a line ?
            if (listConnections.Count > 0)
            {
                Debug.Log("Has already line in scene");

                foreach (LineConnection lineConnection in listConnections)
                {
                    if (collidingObject == lineConnection.StartObject)
                    {
                        Debug.Log("Add Line Connection Start");

                        // update line pos[0]
                        GameObject lineObject = lineConnection.Line;
                        Line line = new Line(lineObject, 0);

                        listLines.Add(line);

                    }
                    else if (collidingObject == lineConnection.EndObject)
                    {
                        Debug.Log("Add Line Connection End");

                        // update line pos[0]
                        GameObject lineObject = lineConnection.Line;
                        Line line = new Line(lineObject, 1);

                        listLines.Add(line);
                    }
                }

                Debug.Log(listLines);
            }

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

                objectInHand.GetComponent<Rigidbody>().isKinematic = true;
            }
            // 4
            objectInHand = null;
            listLines.Clear();
            // listLines = null;
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
