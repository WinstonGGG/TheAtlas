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

    public GOManagement go;
    
    private GameObject[] imageObjects; //放置背包里的物品
    private int length; //存在的物品数量
    public GameObject backpack;
    public GameObject canvas;

    public GameObject textbox; //显示物品名称的文本框
    public Text itemName; //前一行的文本
    public float scaleAmount; //文本框的大小

    public bool backpackOpen = false; //背包是否处于打开状态

    public Dictionary<string, bool> canSpellDictionary; // 储存所有背包物品是否为技能
    public Dictionary<string, bool> canEquipDictionary; // 储存所有背包物品是否为装备
    public Dictionary<string, bool> canInteractDictionary; // 储存所有背包物品是否可以交互
    public Dictionary<string, Texture2D> textureDictionary; // 储存所有背包物品的asset
    
    public int numberOfBackpackItems;
    public List<string> itemNames = new List<string>(); //物品名
    public List<bool> canSpellList = new List<bool>(); //对应物品是否为技能
    public List<bool> canEquipList = new List<bool>(); //对应物品是否为装备
    public List<bool> canInteractList = new List<bool>(); //对应物品是否可以交互
    public List<Texture2D> textureList = new List<Texture2D>(); //对应物品是否可以交互
    
    public List<Sprite> itemCanSpellIcons = new List<Sprite>(); //物品是否为技能属性对应图标： 符、拾
    
    //服务于在Inspector中显示Dictionary
    // public void OnAfterDeserialize()
    public void DictionarySetup()
    {
        canSpellDictionary = new Dictionary<string, bool>();
        canEquipDictionary = new Dictionary<string, bool>();
        canInteractDictionary = new Dictionary<string, bool>();
        textureDictionary = new Dictionary<string, Texture2D>();

        for (int i = 0; i < numberOfBackpackItems; i++){
            canSpellDictionary.Add(itemNames[i], canSpellList[i]);
        }
        for (int i = 0; i < numberOfBackpackItems; i++){
            canInteractDictionary.Add(itemNames[i], canInteractList[i]);
        }
        for (int i = 0; i < numberOfBackpackItems; i++){
            canEquipDictionary.Add(itemNames[i], canEquipList[i]);
        }
        for (int i = 0; i < numberOfBackpackItems; i++){
            textureDictionary.Add(itemNames[i], textureList[i]);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        go = GameObject.Find("GameObjectManager").GetComponent<GOManagement>();
        
        backpack = go.backpack;
        canvas = go.mainUI;
        imageObjects = new GameObject[18];
        length = 0;

        DictionarySetup();
        // this.AddItem("TheAtlas");
        // this.AddItem("SpelltreeIcon");
        // this.AddItem("TalismanIcon");
        // this.AddItem("Taoism Wind");
        this.Show(false); //游戏开始时不显示背包
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //当背包里有18个或以上物品时，不可以再添加新物品
    public bool CanAddItem() {
        if(length >= 18) {
            // TipsDialog.PrintDialog("Backpack Full");
            return false;
        }
        return true;
    }

    public void AddItem(string name) {
        
        GameObject itemObj = new GameObject(name); //Create the GameObject
        imageObjects[length] = itemObj;
        length++;

        RawImage image = itemObj.AddComponent<RawImage>(); //Add the Image Component script
        BackpackItem handler = itemObj.AddComponent<BackpackItem>(); //Add item-drag component
        handler.textbox = textbox;
        handler.itemName = itemName;
        handler.itemScale = scaleAmount;
        handler.canSpell = canSpellDictionary[name];
        handler.canEquip = canEquipDictionary[name];
        handler.canInteract = canInteractDictionary[name];

        image.texture = textureDictionary[name]; //Set the Sprite of the Image Component on the new GameObject
        itemObj.tag = "BackpackItem";

        //物品类型角标设置
        GameObject itemCanSpellIcon = new GameObject("ItemCanSpell");
        Image itemCanSpellImg = itemCanSpellIcon.AddComponent<Image>();
        // -----------------More code needed------------------------

        RectTransform item_transform = itemObj.GetComponent<RectTransform>();
        item_transform.SetParent(go.itemHolder.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel, Canvas/Main UI.
        item_transform.SetAsFirstSibling();

        //更改物品的大小以适配背包卷轴背景的宽度
        GameObject locationObj = go.itemPositionHolder.transform.GetChild(length).gameObject;
        item_transform.anchoredPosition = locationObj.GetComponent<RectTransform>().anchoredPosition;

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
        
        ObItem item_ob = itemObj.AddComponent<ObItem>();
        item_ob.itemStateTotalNum = GameObject.Find(name).GetComponent<ObItem>().itemStateTotalNum;
        InSceneItem item_type = itemObj.AddComponent<InSceneItem>();
        // UISoundScript.PlayGetItem();
        itemObj.SetActive(this.gameObject.activeSelf);
    }

    public void RemoveItem(GameObject itemObject, int removeIndex) {
        Destroy(itemObject);
        length--;
        //移除物品后，将后面的物品依次前移
        for (int i = removeIndex; i < length; i++) {
            imageObjects[i] = imageObjects[i+1];
            // print("move obj " + imageObjects[i].name);
            RectTransform item_tranform = imageObjects[i].GetComponent<RectTransform>();
            item_tranform.anchoredPosition = item_tranform.anchoredPosition + new Vector2(-80, 0);
        }
        textbox.SetActive(false);
    }

    //显示背包，需要显示背包卷轴以及其中的所有物品
    public void Show(bool isShow) {
        backpack.SetActive(isShow);
        for (int i = 0; i < length; i++) {
            GameObject currObj = imageObjects[i];
            currObj.SetActive(isShow);
        }
    }
}
