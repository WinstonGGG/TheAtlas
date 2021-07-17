using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterState : MonoBehaviour
{
    private GameObject goManager;
    private GOManagement go;
	//public GameObject XinMo;
	//public GameObject QiYun;
	public GameObject charStateDisplay;

    // Start is called before the first frame update
    void Start()
    {
        goManager = GameObject.Find("GameObjectManager");
        go = goManager.GetComponent<GOManagement>();

        charStateDisplay = go.characterState;
        go.equipmentState.GetComponent<EquipmentState>().currentImage = go.equipmentState.GetComponentInChildren<RawImage>();
        charStateDisplay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
