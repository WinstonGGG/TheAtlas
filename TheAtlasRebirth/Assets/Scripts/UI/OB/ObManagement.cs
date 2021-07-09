using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObManagement : MonoBehaviour
{
	private GameObject goManager;
    private GOManagement go;

    public GameObject correspondingOB; //当前物品的Ob的reference
    // public GameObject correspondingObject; //当前物品的reference

    [SerializeField]
	private InSceneItem.ItemTypes itemtype; // 当前点击物品的type

    [HideInInspector]
    public GameObject transferToBackpackItemOB; //将背包物品捡起时过渡ObItem信息的媒介
    void Start()
    {
        go = GameObject.Find("GameObjectManager").GetComponent<GOManagement>();
        go.ob.SetActive(false);
        transferToBackpackItemOB = GameObject.Find("TestOB");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OpenOb() {
    	go.ob.SetActive(true);
        correspondingOB.SetActive(true);
    }

    public void CloseOb() {
        correspondingOB.SetActive(false);
    	go.ob.SetActive(false);
    }

    //点击物品时判断是否可以打开此物品的OB，如果可以则打开
    public void GetItemType(GameObject clickObject) {
    	itemtype = clickObject.GetComponent<InSceneItem>().itemType;
        if (itemtype == InSceneItem.ItemTypes.UncollNUnin) {
	        Debug.Log("不可互动不可收集");
        }
        else {
            GetObItemData(clickObject.GetComponent<ObItem>());
            // correspondingObject = clickObject;
        	OpenOb();
        }
    }

    //将ObItem的信息从Item放到Ob Canvas中
    public void GetObItemData(ObItem clickObjectOb) {
        correspondingOB = clickObjectOb.correspondingOB;
    }

    //将背包物品捡起时过渡ObItem信息的媒介
    public void TransferToBackpack(GameObject clickObject) {
        transferToBackpackItemOB = clickObject.GetComponent<ObItem>().correspondingOB;
    }
    //更新Ob State
    public void UpdateState(int state) {
        correspondingOB.GetComponent<ObDisplay>().currentState = state;
    }
}
