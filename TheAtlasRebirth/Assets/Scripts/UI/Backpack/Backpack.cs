using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        if (length <= 10) {
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
        ObItem item_ob = ob.transferToBackpackItem;
        ObItem backpack_item_ob = itemObj.AddComponent<ObItem>();
        if (item_ob != null)
            item_ob.duplicateCurrent(backpack_item_ob);
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
        Debug.Log("page number" + currPageNumber);
        IEnumerator e = imageObjects.GetEnumerator((currPageNumber*5), 10);
        while (e.MoveNext()) {
            GameObject currObj = (GameObject) e.Current;
            if (currObj != null) {
                currObj.SetActive(isShow);
            } else {
                break;
            }
        }
    }
    // 转页，isNextPage==true则翻至下一页，否则翻至上一页
    public void TurnPage(bool isNextPage) {
        if (isNextPage) { //翻至下一页
            // targetAngles = go.itemHolder.transform.eulerAngles + 180f * Vector3.up; 
            // go.itemHolder.transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetAngles, 1f * Time.deltaTime); 
            go.itemHolder.transform.localRotation *= Quaternion.Euler(0, 0, 180);
            currPageNumber++;
            Debug.Log("obj length: " + imageObjects.ToArray().Length + ", to count: " + (currPageNumber*5 + 5));
            IEnumerator e = imageObjects.GetEnumerator((currPageNumber*5), 5);
            while (e.MoveNext()) {
                GameObject currObj = (GameObject) e.Current;
                currObj.transform.localRotation *= Quaternion.Euler(0, 0, 180);
            }
        } else { //翻至上一页

        }
    }
}
