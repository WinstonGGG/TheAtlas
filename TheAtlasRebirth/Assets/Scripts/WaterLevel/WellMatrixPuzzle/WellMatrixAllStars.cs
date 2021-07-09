using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellMatrixAllStars : MonoBehaviour
{
    public GameObject[] correctStars;
    public GameObject[] wrongStars;
    public GameObject fall;
    private GOManagement go;
    public ObManagement ob; //ObManagement component

    
    void Start()
    {
        go = GameObject.Find("GameObjectManager").GetComponent<GOManagement>();
        ob = go.ob.GetComponent<ObManagement>();
    }

}
