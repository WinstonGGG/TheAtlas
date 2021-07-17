using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackpackItem : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler {   
    public GOManagement go;

    public static GameObject draggedItem; // 从背包里被拖拽出来的物品

    public static bool holdItem; // 玩家当前是否在拖拽背包里的物品
    public static bool canPlaceItem = true; // 玩家当前知否可以将物品施放

    public static Vector3 previousPosition = new Vector3(0,0,0); // 玩家当前拖拽物品在背包里的原始位置
    public static float x; // 前一行variable的x坐标

    public static Vector2 originalSize = new Vector2(0f, 0f); // 玩家当前拖拽物品在背包里的原始大小
    public float itemScale; // 物品被鼠标经过或者拖拽时变大的比例

    public static GameObject textbox; // 背包显示物品名的文本框，在Backpack.cs里初始化
    public static Text itemName; // 前一行的文本

    public static GameObject equipButton; // 背包物品装备按钮，在Backpack.cs里初始化
    public static GameObject equipment; // 前一行的文本

    public Vector2 itemOriginalScale; // 河图专用原始大小

    public bool dialogShown = false;
    // => FindObjectOfType<TipsDialog>() != null;

    public bool canSpell; //当前物品是否为技能
    public bool canEquip; //当前物品是否可以装备
    public bool canInteract; //当前物品是否可以改变形态

    private GraphicRaycaster raycaster;
    private PointerEventData pointerData;
    private EquipmentState equipmentState;
    private ObManagement ob;

    void Awake() {
        canPlaceItem = true;
        previousPosition = new Vector3(0,0,0);
        originalSize = new Vector2(0f, 0f);
    }

     // Start is called before the first frame update
    void Start()
    {
        draggedItem = null;
        holdItem = false;
        itemOriginalScale = transform.localScale;
        go = GameObject.Find("GameObjectManager").GetComponent<GOManagement>();
        raycaster = go.clickManagement.raycaster;
        equipmentState = go.equipmentState.GetComponent<EquipmentState>();
        ob = go.ob.GetComponent<ObManagement>();
        textbox = go.textbox;
        equipButton = go.equipButton;
        itemName = go.itemName;
    }

    //鼠标移动至背包里物品上方时物品变大，显示浮框
    public void OnPointerEnter(PointerEventData eventData) {
        textbox.SetActive(true);
        itemName.text = gameObject.name.ToString();
        // textbox.GetComponent<TextboxScaler>().UpdateBoxSize();
        textbox.GetComponent<RectTransform>().anchoredPosition = this.gameObject.GetComponent<RectTransform>().anchoredPosition + new Vector2(-180f, -120f);
        
        equipButton.SetActive(true);
        go.clickManagement.equipment = gameObject;
        equipButton.GetComponent<RectTransform>().anchoredPosition = this.gameObject.GetComponent<RectTransform>().anchoredPosition + new Vector2(-120f, 40f);
        
        transform.localScale *= itemScale;
    }

    //鼠标移动至背包里物品上方时物品变小
    public void OnPointerExit(PointerEventData eventData) {
        textbox.SetActive(false);
        equipButton.SetActive(false);
        transform.localScale /= itemScale;
    }

    //拖拽过程中的物品大小变化，以及背包显示物品名的文本框隐藏
    public void OnDrag(PointerEventData eventData){
        if (!dialogShown && draggedItem != null) {
            draggedItem.transform.position = Input.mousePosition;
            textbox.SetActive(false);
            equipButton.SetActive(false);
        }
        if(!draggedItem) return;
        RectTransform item_transform = draggedItem.GetComponent<RectTransform>();
        string name = draggedItem.name;
        BackpackItemSize variables = go.backpack.GetComponent<BackpackItemSize>();
    }

    //结束语拖拽时激活施法的相关程序Put()，如果没有释放成功物品回到背包里的原始位置
    public void OnEndDrag(PointerEventData eventData){
        if (!dialogShown && draggedItem != null) {
            Put();
            draggedItem.GetComponent<RectTransform>().anchoredPosition = previousPosition;
            holdItem = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if(!dialogShown) holdItem = true;
        // if (SceneManager.GetActiveScene().name != "SampleScene")
        //     GameObject.Find("playerParticleEffect").GetComponent<castEffect>().castAni();
    }

    //施放物品时候可能会发生的效果
    public void Put()
    {
        // Debug.Log("put");
        if (holdItem && raycaster != null && !dialogShown) {
            bool placed = false;
            //Set up the new Pointer Event
            pointerData = new PointerEventData(go.clickManagement.eventSystem);
            pointerData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            
            //Raycast using the Graphics Raycaster and mouse click position
            raycaster.Raycast(pointerData, results);

            int resultSize = 0;

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results) {
                resultSize += 1;
                GameObject dragOnObject = result.gameObject;
                int position = ((int)x + 680) / 80;

                if (canEquip && dragOnObject.name.CompareTo("CurrentEquipIcon") == 0 && equipmentState.isEquiped == false) { //装备新物品于装备栏
                    placed = true;
                    equipmentState.equip(draggedItem);
                    print("make sure it exist" + draggedItem.GetComponent<ObItem>());
                    break;
                } else {
                    ObItem targetOb;
                    string targetName;
                    InSceneItem inSceneProperty = null;
                    if (dragOnObject.name.CompareTo("CurrentEquipIcon") == 0 && equipmentState.isEquiped == true) { //与装备栏里的物品交互
                        targetOb = equipmentState.equipedItemOb;
                        targetName = equipmentState.currentEquipmentName;
                        print(draggedItem.name + ", on to equipment: " + equipmentState.currentEquipmentName);

                    } else { //directly interact with another object either on UI or on the ground, UI first
                        inSceneProperty = dragOnObject.GetComponent<InSceneItem>();
                        targetOb = dragOnObject.GetComponent<ObItem>();
                        targetName = dragOnObject.name;
                        print(draggedItem.name + ", on to: " + dragOnObject.name);
                    }
                    
                    canPlaceItem = ItemEffects.canPlace(draggedItem.name, targetName);
                    if (canPlaceItem) {
                        print(targetOb);
                        ob.GetObItemData(targetOb);
                        ob.OpenOb();

                        if (draggedItem.name.CompareTo("The Atlas") == 0)
                            transform.localScale = itemOriginalScale / itemScale;
                        else 
                            go.backpack.GetComponent<Backpack>().RemoveItem(draggedItem, position);
                        
                        if (inSceneProperty == null) {
                            ItemEffects.puzzleEffect(draggedItem.name, targetName, pointerData.position);
                            // if (draggedItem.name.CompareTo("Tao-Book") != 0 && draggedItem.name.CompareTo("Talisman") != 0 && draggedItem.name.CompareTo("The Atlas") != 0 && SceneManager.GetActiveScene().name != "SampleScene")
                            //     GameObject.Find("pickupEffect").GetComponent<pickupEffect>().castAni(pointerData.position);
                            placed = true;
                        } else {
                            placed = false;
                        }
                        break;
                    }
                } 
                // if (SceneManager.GetActiveScene().name != "SampleScene")
                //     GameObject.Find("playerParticleEffect").GetComponent<castEffect>().stopCasting();
            }

            if (!placed) {
                // UISoundScript.PlayWrongSpell();
                // AIDataManager.wrongItemPlacementCount += 1;
                draggedItem.GetComponent<RectTransform>().sizeDelta = originalSize;
            }
        }
    }
}
