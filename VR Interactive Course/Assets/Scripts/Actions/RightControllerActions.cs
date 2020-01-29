using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class RightControllerActions : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean menuLeft;
    public SteamVR_Action_Boolean menuRight;

    public GameObject sollarPanel;
    public GameObject battery;
    public GameObject lightBulb;
    public GameObject powerInverter;
    public GameObject laptop;
    public GameObject chargeController;
    public GameObject menu;

    // private ArrayList listModels = new ArrayList();
    private List<GameObject> listModels = new List<GameObject>();

    private GameObject activeModel;
    private GameObject shownModel;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate Models
        GameObject mySolarPannel = Instantiate(sollarPanel);
        mySolarPannel.transform.parent = menu.transform;

        GameObject myBattery = Instantiate(battery);
        myBattery.transform.parent = menu.transform;
        myBattery.SetActive(false);

        GameObject myLightBulb = Instantiate(lightBulb);
        myLightBulb.transform.parent = menu.transform;
        myLightBulb.SetActive(false);

        GameObject myLaptop = Instantiate(laptop);
        myLaptop.transform.parent = menu.transform;
        myLaptop.SetActive(false);

        GameObject myChargeController = Instantiate(chargeController);
        myChargeController.transform.parent = menu.transform;
        myChargeController.SetActive(false);

        GameObject myPowerInverter = Instantiate(powerInverter);
        myPowerInverter.transform.parent = menu.transform;
        myPowerInverter.SetActive(false);

        listModels.Add(mySolarPannel);
        listModels.Add(myBattery);
        listModels.Add(myLightBulb);
        listModels.Add(myPowerInverter);
        listModels.Add(myChargeController);
        listModels.Add(myLaptop);

        activeModel = mySolarPannel;

        // Add models connection to list
        Connection connection = new Connection(mySolarPannel, myChargeController);
        LineConnectionList.myListConnections.Add(connection);

        connection = new Connection(myChargeController, myBattery);
        LineConnectionList.myListConnections.Add(connection);

        connection = new Connection(myBattery, myLightBulb);
        LineConnectionList.myListConnections.Add(connection);

        connection = new Connection(myBattery, myPowerInverter);
        LineConnectionList.myListConnections.Add(connection);

        connection = new Connection(myPowerInverter, myLaptop);
        LineConnectionList.myListConnections.Add(connection);

        //Add listner
        menuLeft.AddOnStateDownListener(GetMenuLeft, handType);
        menuRight.AddOnStateDownListener(GetMenuRight, handType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        
    }

    public void GetMenuLeft(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if(listModels.Count > 1)
        {
            listChildren(menu);

            int index = listModels.IndexOf(activeModel);
            GameObject shownModel = listModels[index];
            shownModel.SetActive(false);
            if (index == 0)
            {
                shownModel = listModels[listModels.Count - 1];
                shownModel.SetActive(true);
            }
            else
            {
                shownModel = listModels[index - 1];
                shownModel.SetActive(true);
            }

            activeModel = shownModel;
        }
    }

    public void GetMenuRight(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (listModels.Count > 1)
        {
            listChildren(menu);

            int index = listModels.IndexOf(activeModel);
            shownModel = listModels[index];
            shownModel.SetActive(false);
            if (index == listModels.Count - 1)
            {
                shownModel = listModels[0];
                shownModel.SetActive(true);
            }
            else
            {
                shownModel = listModels[index + 1];
                shownModel.SetActive(true);
            }

            activeModel = shownModel;
        }
    }

    public void listChildren(GameObject parentObject)
    {
        List<GameObject> listCurentModels = new List<GameObject>();
        Rigidbody[] arrayCurentModelsRB;

        // Get components, even the inactive children (true)
        arrayCurentModelsRB = parentObject.GetComponentsInChildren<Rigidbody>(true);

        foreach (Rigidbody body in arrayCurentModelsRB)
        {
            listCurentModels.Add(body.gameObject);
        }

        bool isAanwezig = false;

        if (listCurentModels.Count != listModels.Count)
        {
            for (int i = 0; i < listModels.Count; i++)
            {
                for (int x = 0; x < listCurentModels.Count; x++)
                {
                    if (listModels[i].name == listCurentModels[x].name)
                    {
                        isAanwezig = true;
                    }
                }

                if (isAanwezig == false)
                {
                    listModels.RemoveAt(i);

                    shownModel = listModels[0];
                    shownModel.SetActive(true);

                    activeModel = shownModel;
                } else
                {
                    isAanwezig = false;
                }
            }
        }
    }
}
