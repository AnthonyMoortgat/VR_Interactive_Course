using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject sollarPanel;
    public GameObject battery;

    private Dictionary<GameObject, bool> modelList;

    // Start is called before the first frame update
    void Start()
    {
        modelList.Add(sollarPanel, true);
        modelList.Add(battery, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
