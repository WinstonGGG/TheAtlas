using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Backpack : MonoBehaviour
{
    // public class BackpackItemProperty { //当前物品的所有属性
    //     public Texture2D texture; //当前物品的asset
    //     public bool canSpell; //当前物品是否为技能
    //     public bool canEquip; //当前物品是否可以装备
    //     public bool canInteract; //当前物品是否可以改变形态
    // }
    [HideInInspector]
    public GOManagement go;
    [HideInInspector]
    private ArrayList imageObjects; //放置背包里的物品
    [HideInInspector]
    private int currPageNumber; // 当前背包页数
    [HideInInspector]
    private int length; //存在的物品数量
    [HideInInspector]
    public GameObject backpack;
    [HideInInspector]
    public GameObject canvas;
    private ObManagement ob;

    [HideInInspector]
    public GameObject textbox; //显示物品名称的文本框
    [HideInInspector]
    public float scaleAmount; //文本框的大小

    [HideInInspector]
    public bool backpackOpen = false; //背包是否处于打开状态

    public Dictionary<string, bool> canSpellDictionary; // 储存所有背包物品是否为技能
    public Dictionary<string, bool> canEquipDictionary; // 储存所有背包物品是否为装备
    public Dictionary<string, bool> canInteractDictionary; // 储存所有背包物品是否可以交互
    public Dictionary<string, Texture2D> textureDictionary; // 储存所有背包物品的asset
    
    public int numberOfBackpackItems;
    [System.Serializable]
    public struct ItemInfo
    {
    	public string itemName;
    	public bool canSpell;
    	public bool canEquip;
    	public bool canInteract;
        public Texture2D texture;
    }
    public ItemInfo[] backpackItemInfos;
    
    public List<Sprite> itemCanSpellIcons = new List<Sprite>(); //物品是否为技能属性对应图标： 符、拾

    public Sprite equipmentIcon;
    public Sprite spellIcon;
    public Sprite collectIcon;
    [HideInInspector]
    private Vector3 targetAngles; // 背包旋转参数
    //服务于在Inspector中显示Dictionary
    // public void OnAfterDeserialize()
    public void DictionarySetup()
    {
        canSpellDictionary = new Dictionary<string, bool>();
        canEquipDictionary = new Dictionary<string, bool>();
        canInteractDictionary = new Dictionary<string, bool>();
        textureDictionary = new Dictionary<string, Texture2D>();

        for (int i = 0; i < numberOfBackpackItems; i++){
            canSpellDictionary.Add(backpackItemInfos[i].itemName, backpackItemInfos[i].canSpell);
        }
        for (int i = 0; i < numberOfBackpackItems; i++){
            canInteractDictionary.Add(backpackItemInfos[i].itemName, backpackItemInfos[i].canInteract);
        }
        for (int i = 0; i < numberOfBackpackItems; i++){
            canEquipDictionary.Add(backpackItemInfos[i].itemName, backpackItemInfos[i].canEquip);
        }
        for (int i = 0; i < numberOfBackpackItems; i++){
            textureDictionary.Add(backpackItemInfos[i].itemName, backpackItemInfos[i].texture);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        go = GameObject.Find("GameObjectManager").GetComponent<GOManagement>();
        ob = go.ob.GetComponent<ObManagement>();

        backpack = go.backpack;
        canvas = go.mainUI;
        textbox = go.textbox;
        imageObjects = new ArrayList();
        length = 0;
        currPageNumber = 0;

        DictionarySetup();
        this.AddItem("Test");
        this.AddItem("Test");
        this.AddItem("Test");
        this.AddItem("Test");
        this.AddItem("Test");
        this.AddItem("RealPick");
        this.AddItem("Test");
        this.AddItem("Test");
        this.AddItem("Test");
        this.AddItem("Test");
        this.AddItem("Test");
        this.AddItem("RealPick");
        this.AddItem("Test");
        this.AddItem("Test");
        this.Show(false); //游戏开始时不显示背包
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //当背包里有18个或以上物品时，不可以再添加新物品
    public bool CanAddItem() {
        // if(length >= 18) {
        //     return false;
        // }
        return true;
    }

    public void AddItem(string name) {
        GameObject itemObj = new GameObject(name); //Create the GameObject
        imageObjects.Add(itemObj);
        length++;

        RawImage image = itemObj.AddComponent<RawImage>(); //Add the Image Component script
        BackpackItem handler = itemObj.AddComponent<BackpackItem>(); //Add item-drag component
        handler.itemScale = scaleAmount;
            
        //物品判定
        handler.canSpell = canSpellDictionary[name];
        handler.canEquip = canEquipDictionary[name];
        handler.canInteract = canInteractDictionary[name];
        image.texture = textureDictionary[name]; //Set the Sprite of the Image Component on the new GameObject

        itemObj.tag = "BackpackItem";

        RectTransform item_transform = itemObj.GetComponent<RectTransform>();
        item_transform.SetParent(go.itemHolder.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel, Canvas/Main UI.
        item_transform.SetAsFirstSibling();

        //更改物品的位置于背包UI内
        if (length <= (currPageNumber+1)*5 && length > currPageNumber*5) {
            GameObject locationObj = go.itemPositionHolder.transform.GetChild(length-1).gameObject;
            item_transform.anchoredPosition = locationObj.GetComponent<RectTransform>().anchoredPosition;
        } else {
            GameObject locationObj = go.extraItemHolder;
            item_transform.anchoredPosition = locationObj.GetComponent<RectTransform>().anchoredPosition;
        }

        //物品类型角标设置
        GameObject itemIcon = new GameObject("ItemCanSpell");
        Image itemIconImg = itemIcon.AddComponent<Image>();
        if (handler.canSpell) {
            itemIconImg.sprite = spellIcon;
        } else {
            itemIconImg.sprite = collectIcon;
        }

        RectTransform itemIconTransform = itemIcon.GetComponent<RectTransform>();
        itemIconTransform.SetParent(itemObj.transform);
        itemIconTransform.sizeDelta = new Vector2(20, 20);
        itemIconTransform.anchoredPosition = new Vector2(20, -20);

        if (name.CompareTo("Heavenly Water") == 0) {
            item_transform.sizeDelta = new Vector2(60, 35);
        } else if (name.CompareTo("Changable Soil") == 0) {
            item_transform.anchoredPosition = item_transform.anchoredPosition + new Vector2(0, 5);
            item_transform.sizeDelta = new Vector2(45, 40);
        } else if (name.CompareTo("Taiji Key") == 0) {
            item_transform.sizeDelta = new Vector2(80, 80);
        } else if (name.CompareTo("Earth Key") == 0) {
            item_transform.sizeDelta = new Vector2(40f, 60f);
        } else if (name.CompareTo("Yin-Yang Portal") == 0) {
            item_transform.sizeDelta = new Vector2(48f, 32f);
        } else {
            item_transform.sizeDelta = new Vector2(60f, 60f);
        }
        
        //继承Ob相关属性
        // ObItem item_ob = ob.transferToBackpackItem;
        ObItem backpackItemOb = itemObj.AddComponent<ObItem>();
        backpackItemOb.DuplicateToBackpackItem();
        itemObj.SetActive(this.gameObject.activeSelf);
    }

    public void RemoveItem(GameObject itemObject, int removeIndex) {
        Destroy(itemObject);
        length--;
        imageObjects.RemoveAt(removeIndex);
        //移除物品后，将后面的物品依次前移
        // for (int i = removeIndex; i < length; i++) {
        //     imageObjects[i] = imageObjects[i+1];
        //     // print("move obj " + imageObjects[i].name);
        //     RectTransform item_tranform = imageObjects[i].GetComponent<RectTransform>();
        //     item_tranform.anchoredPosition = item_tranform.anchoredPosition + new Vector2(-80, 0);
        // }
        textbox.SetActive(false);
    }

    //显示背包，需要显示背包卷轴以及其中的所有物品
    public void Show(bool isShow) {
        backpack.SetActive(isShow);
        IEnumerator e = GetBackpackEnumerator(currPageNumber*5, 10);
        
        while (e.MoveNext()) {
            GameObject currObj = (GameObject) e.Current;
            if (currObj != null) {
                currObj.SetActive(isShow);
            } 
            else {
                break;
            }
        }
    }
    // 转页，isNextPage==true则翻至下一页，否则翻至上一页
    public void TurnPage(bool isNextPage) {
        int length = imageObjects.ToArray().Length;
        int total_page_num = (int) Math.Ceiling(length / 5.0);
        IEnumerator e;
        // Vector3 currentRatation = go.itemHolder.transform.localRotation.eulerAngles;
        if (isNextPage) { //翻至下一页
            if (currPageNumber+1 < total_page_num) {
                currPageNumber++;
                // preload Backpack Items
                e = GetBackpackEnumerator(currPageNumber*5, 5);
                int i;
                if (currPageNumber % 2 == 1) {
                    i = 5;
                } else {
                    i = 0;
                }
                while (e.MoveNext()) {
                    GameObject currObj = (GameObject) e.Current;
                    if (currObj != null) {
                        RectTransform item_transform = currObj.GetComponent<RectTransform>();
                        GameObject locationObj = go.itemPositionHolder.transform.GetChild(i).gameObject;
                        item_transform.anchoredPosition = locationObj.GetComponent<RectTransform>().anchoredPosition;
                        i++;
                    } 
                    else {
                        break;
                    }
                }
                // rotate individual Backpack Items to be shown to correct angle
                e = GetBackpackEnumerator(currPageNumber*5, 5);
                while (e.MoveNext()) {
                    GameObject currObj = (GameObject) e.Current;
                    if (currObj != null) {
                        currObj.transform.localRotation *= Quaternion.Euler(0, 0, 180);
                    } 
                    else {
                        break;
                    }
                }

                if (currPageNumber % 2 == 1) {
                    LeanTween.rotate(go.itemHolder, new Vector3(0, 0, -180), 0.5f);
                }
                else {
                    LeanTween.rotate(go.itemHolder, new Vector3(0, 0, 0), 0.5f);
                }

                // Hide Backpack Items in the previous page
                if (currPageNumber > 0) {
                    e = GetBackpackEnumerator((currPageNumber-1)*5, 5);
                    while (e.MoveNext()) {
                        GameObject currObj = (GameObject) e.Current;
                        if (currObj != null) {
                            RectTransform item_transform = currObj.GetComponent<RectTransform>();
                            GameObject locationObj = go.extraItemHolder;
                            item_transform.anchoredPosition = locationObj.GetComponent<RectTransform>().anchoredPosition;
                        } 
                        else {
                            break;
                        }
                    }
                }
            }
        } 
        else { //翻至上一页
            if (currPageNumber > 0) {
                currPageNumber--;
                // preload Backpack Items
                e = imageObjects.GetEnumerator(currPageNumber*5, 5);
                int i;
                if (currPageNumber % 2 == 1) {
                    i = 5;
                } else {
                    i = 0;
                }
                while (e.MoveNext()) {
                    GameObject currObj = (GameObject) e.Current;
                    if (currObj != null) {
                        RectTransform item_transform = currObj.GetComponent<RectTransform>();
                        GameObject locationObj = go.itemPositionHolder.transform.GetChild(i).gameObject;
                        item_transform.anchoredPosition = locationObj.GetComponent<RectTransform>().anchoredPosition;
                        i++;
                    } 
                    else {
                        break;
                    }
                }

                if (currPageNumber % 2 == 1) {
                    LeanTween.rotate(go.itemHolder, new Vector3(0, 0, 180), 0.5f);
                }
                else {
                    LeanTween.rotate(go.itemHolder, new Vector3(0, 0, 360), 0.5f);
                }

                // Hide Backpack Items in the next page
                if (currPageNumber+1 < total_page_num) {
                    e = GetBackpackEnumerator(currPageNumber*5 + 5, 5);
                    while (e.MoveNext()) {
                        GameObject currObj = (GameObject) e.Current;
                        if (currObj != null) {
                            RectTransform item_transform = currObj.GetComponent<RectTransform>();
                            GameObject locationObj = go.extraItemHolder;
                            item_transform.anchoredPosition = locationObj.GetComponent<RectTransform>().anchoredPosition;
                        } 
                        else {
                            break;
                        }
                    }
                }
            }
        }
    }

    private IEnumerator GetBackpackEnumerator(int start, int length) {
        IEnumerator e;
        try {
            e = imageObjects.GetEnumerator(start, length);
        }
        catch (ArgumentException ex) {
            e = imageObjects.GetEnumerator(start, imageObjects.ToArray().Length - start);
        }
        return e;
    }
}
