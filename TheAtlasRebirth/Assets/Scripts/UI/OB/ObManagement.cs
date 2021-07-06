using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObManagement : MonoBehaviour
{
	private GameObject goManager;
    private GOManagement go;
    public GameObject correspondingOB;
    //clickobj的itemStateTotalNum
    // public ObItem.ItemState[] stateTotalNum;

	// 当前点击物品的type
    [SerializeField]
	private InSceneItem.ItemTypes itemtype;

    [HideInInspector]
    public GameObject transferToBackpackItemOB;

    public GraphicRaycaster raycaster;
    PointerEventData pointerData;
    public EventSystem eventSystem;

    // Start is called before the first frame update
    void Start()
    {
        go = GameObject.Find("GameObjectManager").GetComponent<GOManagement>();
        go.ob.SetActive(false);
        transferToBackpackItemOB = GameObject.Find("TestOB");

        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the left Mouse button is clicked
        if (raycaster == null) {

        }
        else if (Input.GetMouseButtonDown(0)) { //按鼠标左键
            //Set up the new Pointer Event
            pointerData = new PointerEventData(eventSystem);
            pointerData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            
            //Raycast using the Graphics Raycaster and mouse click position
            raycaster.Raycast(pointerData, results);

            int resultSize = 0;

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results) {
                resultSize += 1;
                string name = result.gameObject.name;
                string tag = result.gameObject.tag;
                
                if (tag.CompareTo("WellMatrixStar") == 0) {
                    result.gameObject.GetComponent<WellMatrixStar>().Activate();
                }
            }
        }
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
        Debug.Log(itemtype);
        if (itemtype == InSceneItem.ItemTypes.UncollNUnin) {
	        Debug.Log("不可互动不可收集");
        }
        else {
            GetObItemData(clickObject.GetComponent<ObItem>());
        	OpenOb();
        }
    }

    //将ObItem的信息从Item放到Ob Canvas中
    public void GetObItemData(ObItem clickObjectOb) {
        correspondingOB = clickObjectOb.correspondingOB;
    }

    public void TransferToBackpack(GameObject clickObject) {
        transferToBackpackItemOB = clickObject.GetComponent<ObItem>().correspondingOB;
    }
}
