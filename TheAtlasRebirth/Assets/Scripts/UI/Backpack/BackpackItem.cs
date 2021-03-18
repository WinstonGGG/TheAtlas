using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackpackItem : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler {   
    public GOManagement go;

    public static GameObject itemOnGround; // 从背包里被拖拽出来的物品

    public static bool holdItem; // 玩家当前是否在拖拽背包里的物品
    public static bool canPlaceItem = true; // 玩家当前知否可以将物品施放

    public static Vector3 previousPosition = new Vector3(0,0,0); // 玩家当前拖拽物品在背包里的原始位置
    public static float x; // 前一行variable的x坐标

    public static Vector2 originalSize = new Vector2(0f, 0f); // 玩家当前拖拽物品在背包里的原始大小
    public float itemScale; // 物品被鼠标经过或者拖拽时变大的比例

    public GameObject textbox; // 背包显示物品名的文本框
    public Text itemName; // 前一行的文本

    public Vector2 itemOriginalScale; // 河图专用原始大小

    public bool dialogShown = false;
    // => FindObjectOfType<TipsDialog>() != null;

    public bool canSpell; //当前物品是否为技能
    public bool canEquip; //当前物品是否可以装备
    public bool canInteract; //当前物品是否可以改变形态

    void Awake() {
        canPlaceItem = true;
        previousPosition = new Vector3(0,0,0);
        originalSize = new Vector2(0f, 0f);
    }

     // Start is called before the first frame update
    void Start()
    {
        itemOnGround = null;
        holdItem = false;
        itemOriginalScale = transform.localScale;
        go = GameObject.Find("GameObjectManager").GetComponent<GOManagement>();
    }

    //鼠标移动至背包里物品上方时物品变大
    public void OnPointerEnter(PointerEventData eventData) {
        textbox.SetActive(true);
        itemName.text = gameObject.name.ToString();
        // textbox.GetComponent<TextboxScaler>().UpdateBoxSize();
        textbox.GetComponent<RectTransform>().anchoredPosition = this.gameObject.GetComponent<RectTransform>().anchoredPosition + new Vector2(-180f, -120f);
        transform.localScale *= itemScale;
    }

    //鼠标移动至背包里物品上方时物品变小
    public void OnPointerExit(PointerEventData eventData) {
        textbox.SetActive(false);
        transform.localScale /= itemScale;
    }

    //拖拽过程中的物品大小变化，以及背包显示物品名的文本框隐藏
    public void OnDrag(PointerEventData eventData){
        if (!dialogShown && itemOnGround != null) {
            itemOnGround.transform.position = Input.mousePosition;
            textbox.SetActive(false);
        }
        if(!itemOnGround) return;
        RectTransform item_transform = itemOnGround.GetComponent<RectTransform>();
        string name = itemOnGround.name;
        BackpackItemSize variables = go.backpack.GetComponent<BackpackItemSize>();
    }

    //结束语拖拽时激活施法的相关程序Put()，如果没有释放成功物品回到背包里的原始位置
    public void OnEndDrag(PointerEventData eventData){
        if (!dialogShown && itemOnGround != null) {
            Put();
            itemOnGround.GetComponent<RectTransform>().anchoredPosition = previousPosition;
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
        if (holdItem) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity) && !dialogShown) {
                GameObject dragOnObject = hitInfo.collider.gameObject;
                int position = ((int)x + 680) / 80;

                canPlaceItem = ItemEffects.canPlace(itemOnGround.name, dragOnObject.name);
                print(itemOnGround.name + ", on to: " + dragOnObject.name);
                if (canPlaceItem) {
                    if (itemOnGround.name.CompareTo("The Atlas") == 0)
                        transform.localScale = itemOriginalScale / itemScale;
                    else 
                        GameObject.Find("Backpack_Roll").GetComponent<Backpack>().RemoveItem(itemOnGround, position);
                    
                    ItemEffects.puzzleEffect(itemOnGround.name, dragOnObject.name, hitInfo.point);
                    // if (itemOnGround.name.CompareTo("Tao-Book") != 0 && itemOnGround.name.CompareTo("Talisman") != 0 && itemOnGround.name.CompareTo("The Atlas") != 0 && SceneManager.GetActiveScene().name != "SampleScene")
                    //     GameObject.Find("pickupEffect").GetComponent<pickupEffect>().castAni(hitInfo.point);
                } else {
                    // UISoundScript.PlayWrongSpell();
                    // AIDataManager.wrongItemPlacementCount += 1;
                    itemOnGround.GetComponent<RectTransform>().sizeDelta = originalSize;
                }
                // if (SceneManager.GetActiveScene().name != "SampleScene")
                //     GameObject.Find("playerParticleEffect").GetComponent<castEffect>().stopCasting();
            }
            else {
                print("physics error");
            }
        }
    }
}
