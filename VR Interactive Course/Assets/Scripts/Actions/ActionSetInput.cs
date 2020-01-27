using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ActionSetInput : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean menuLeft;
    public SteamVR_Action_Boolean menuRight;
    public SteamVR_Action_Boolean grabAction;

    public GameObject sollarPanel;
    public GameObject battery;
    public GameObject lightBulb;
    public GameObject powerInverter;
    public GameObject laptop;
    public GameObject chargeController;
    public GameObject menu;

    private ArrayList listModels = new ArrayList();
    private GameObject activeModel;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate Models
        GameObject mySolarPannel = Instantiate(sollarPanel);
        mySolarPannel.transform.parent = menu.transform;

        GameObject myBattery = Instantiate(battery);
        myBattery.transform.parent = menu.transform;
        myBattery.SetActive(false);

        listModels.Add(mySolarPannel);
        listModels.Add(myBattery);

        activeModel = mySolarPannel;

        //Add listner
        menuLeft.AddOnStateDownListener(GetMenuLeft, handType);
        menuRight.AddOnStateDownListener(GetMenuRight, handType);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetMenuLeft(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Dpad left is down");

        int index = listModels.IndexOf(activeModel);
        GameObject shownModel = (GameObject)listModels[index];
        shownModel.SetActive(false);
        if (index == 0)
        {
            shownModel = (GameObject)listModels[listModels.Count - 1];
            shownModel.SetActive(true);
        } else {
            shownModel = (GameObject)listModels[index - 1];
            shownModel.SetActive(true);
        }

        activeModel = shownModel;
    }

    public void GetMenuRight(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Dpad right is down");

        int index = listModels.IndexOf(activeModel);
        GameObject shownModel = (GameObject)listModels[index];
        shownModel.SetActive(false);
        if (index == listModels.Count - 1)
        {
            shownModel = (GameObject)listModels[0];
            shownModel.SetActive(true);
        } else {
            shownModel = (GameObject)listModels[index + 1];
            shownModel.SetActive(true);
        }

        activeModel = shownModel;
    }

    public void GrabObject(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Grip is down");

    }

    public void LoseObject(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Grip is up");
    }
}
