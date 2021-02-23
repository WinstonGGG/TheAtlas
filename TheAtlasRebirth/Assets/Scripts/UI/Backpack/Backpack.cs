using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Backpack : MonoBehaviour, ISerializationCallbackReceiver
{
    public GOManagement go;
    
    private GameObject[] imageObjects; //放置背包里的物品
    private int length; //存在的物品数量
    public GameObject backpack;
    public GameObject canvas;

    public GameObject textbox; //显示物品名称的文本框
    public Text itemName; //前一行的文本
    public float scaleAmount; //文本框的大小

    public bool backpackOpen = false; //背包是否处于打开状态

    public Dictionary<string, ItemDragHandler.ItemType> typeDictionary; // 储存所有背包物品的类型
    public Dictionary<string, Texture2D> pathDictionary; //储存所有背包物品的texture
    public List<string> itemNames = new List<string>(); //物品名
    public List<ItemDragHandler.ItemType> types = new List<ItemDragHandler.ItemType>(); //对应物品类型
    public List<Texture2D> textures = new List<Texture2D>(); //对应物品类型

    public List<Sprite> itemTypeIcons = new List<Sprite>(); //装、集、符的asset
    
    //以下两个方法均服务于在Inspector中显示Dictionary
    public void OnBeforeSerialize()
    {
        itemNames.Clear();
        types.Clear();
        textures.Clear();

        foreach (var kvp in typeDictionary)
        {
            itemNames.Add(kvp.Key);
            types.Add(kvp.Value);
        }

        foreach (var kvp in pathDictionary)
        {
            textures.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        typeDictionary = new Dictionary<string, ItemDragHandler.ItemType>();
        pathDictionary = new Dictionary<string, Texture2D>(); 

        for (int i = 0; i < itemNames.Count; i++){
            typeDictionary.Add(itemNames[i], types[i]);
            pathDictionary.Add(itemNames[i], textures[i]);
        }
    }

    void OnGUI()
    {
        GUI.depth = 0;
        foreach (var kvp in typeDictionary) 
            GUILayout.Label("Item Name: " + kvp.Key + "Type: " + kvp.Value);
        foreach (var kvp in pathDictionary) 
            GUILayout.Label("Item Name: " + kvp.Key + "Texture: " + kvp.Value);
    }

    // Start is called before the first frame update
    void Start()
    {
        go = GameObject.Find("GameObjectManager").GetComponent<GOManagement>();
        
        backpack = go.backpack;
        canvas = go.mainUI;
        imageObjects = new GameObject[18];
        length = 0;
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
        ItemDragHandler handler = itemObj.AddComponent<ItemDragHandler>(); //Add item-drag component
        handler.textbox = textbox;
        handler.itemName = itemName;
        handler.itemScale = scaleAmount;
        handler.itemType = typeDictionary[name];

        image.texture = pathDictionary[name]; //Set the Sprite of the Image Component on the new GameObject
        itemObj.tag = "Item";

        //物品类型角标设置
        GameObject itemTypeIcon = new GameObject("ItemType");
        Image itemTypeImg = itemTypeIcon.AddComponent<Image>();
        if (handler.itemType == ItemDragHandler.ItemType.EQUIPMENT) {
            itemTypeImg.color = Color.black;
            // itemTypeImg.sprite = itemTypeIcons[0];
        } else if (handler.itemType == ItemDragHandler.ItemType.COLLECTION) {
            itemTypeImg.color = Color.red;
            // itemTypeImg.sprite = itemTypeIcons[1];
        } else {
            itemTypeImg.color = Color.yellow;
            // itemTypeImg.sprite = itemTypeIcons[2];
        }

        RectTransform item_transform = itemObj.GetComponent<RectTransform>();
        item_transform.SetParent(go.itemHolder.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel, Canvas/Main UI.
        item_transform.SetAsFirstSibling();

        //更改物品的大小以适配背包卷轴背景的宽度
        item_transform.anchoredPosition = new Vector2((length-12f)*80 + 650, 0);
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
        
        // UISoundScript.PlayGetItem();
        itemObj.SetActive(backpack.activeSelf);
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
        // for (int i = 0; i < length; i++) {
        //     GameObject currObj = imageObjects[i];
        //     currObj.SetActive(isShow);
        // }
    }
}
