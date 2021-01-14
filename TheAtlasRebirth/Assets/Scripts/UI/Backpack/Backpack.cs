using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Backpack : MonoBehaviour
{
    public GOManagement go;
    
    private GameObject[] imageObjects;
    private int length;
    public GameObject backpack;
    public GameObject canvas;

    public GameObject textbox;
    public Text itemName;
    public float scaleAmount;

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
        this.Show(false);
    }
    void OnGUI() {
        GUI.depth = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CanAddItem() {
        if(length >= 18) {
            // TipsDialog.PrintDialog("Backpack Full");
            return false;
        }
        return true;
    }
    public void AddItem(string name) {
        
        GameObject imageObj = new GameObject(name); //Create the GameObject
        imageObjects[length] = imageObj;
        length++;

        RawImage image = imageObj.AddComponent<RawImage>(); //Add the Image Component script
        ItemDragHandler handler = imageObj.AddComponent<ItemDragHandler>(); //Add item-drag component
        imageObj.GetComponent<ItemDragHandler>().textbox = textbox;
        imageObj.GetComponent<ItemDragHandler>().itemName = itemName;
        imageObj.GetComponent<ItemDragHandler>().itemScale = scaleAmount;

        image.texture = Resources.Load<Texture2D>("Image/BackpackItem/A_" + name); //Set the Sprite of the Image Component on the new GameObject
        imageObj.tag = "Item";

        RectTransform item_transform = imageObj.GetComponent<RectTransform>();
        item_transform.SetParent(go.itemHolder.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel, Canvas/Main UI.
        item_transform.SetAsFirstSibling();

        item_transform.anchoredPosition = new Vector2((length-12f)*80 + 800, 0);
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
        imageObj.SetActive(backpack.activeSelf);
    }

    public void RemoveItem(GameObject itemObject, int removeIndex) {
        Destroy(itemObject);
        length--;
        for (int i = removeIndex; i < length; i++) {
            imageObjects[i] = imageObjects[i+1];
            // print("move obj " + imageObjects[i].name);
            RectTransform item_tranform = imageObjects[i].GetComponent<RectTransform>();
            item_tranform.anchoredPosition = item_tranform.anchoredPosition + new Vector2(-80, 0);
        }
        textbox.SetActive(false);
    }

    public void Show(bool isShow) {
        backpack.SetActive(isShow);
        for (int i = 0; i < length; i++) {
            GameObject currObj = imageObjects[i];
            currObj.SetActive(isShow);
        }
    }
}
