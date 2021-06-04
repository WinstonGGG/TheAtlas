using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObManagement : MonoBehaviour
{
	private GameObject goManager;
    private GOManagement go;

	public GameObject display; 
    //clickobj的itemStateTotalNum
    public ObItem.ItemState[] stateTotalNum;
    public int currentState = 1;

	// 当前点击物品的type
    //[SerializeField]
	private InSceneItem.ItemTypes itemtype;

    [HideInInspector]
    public ObItem transferToBackpackItem;

    // Start is called before the first frame update
    void Start()
    {
        display.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenOb() {
    	display.SetActive(true);
    }

    public void CloseOb() {
    	display.SetActive(false);
    }

    public void GetItemType(GameObject clickObject) {
    	itemtype = clickObject.GetComponent<InSceneItem>().itemType;
        Debug.Log(itemtype);
        if (itemtype == InSceneItem.ItemTypes.UncollNUnin) {
	        Debug.Log("不可互动不可收集");
        }
        else {
        	display.SetActive(true);
        }
    }

    public void GetObItemData(ObItem clickObjectOb) {
        stateTotalNum = clickObjectOb.itemStateTotalNum;
        currentState = clickObjectOb.CurrentState;
    }

    public void TransferToBackpack(GameObject clickObject) {
        transferToBackpackItem = clickObject.GetComponent<ObItem>();
    }
}
