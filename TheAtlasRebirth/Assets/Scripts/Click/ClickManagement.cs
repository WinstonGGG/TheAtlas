using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// [RequireComponent(typeof(TalismanManager))]
public class ClickManagement : MonoBehaviour
{
    private GameObject goManager;
    private GOManagement go;
    
    public bool canAct => !dialogShown && !lockGame; // 玩家是否可以操作
    public bool lockGame = false; //游戏是否锁定状态
    public bool dialogShown = false; //对话框是否显示
    //     => FindObjectOfType<TipsDialog>() != null;
    private GameObject spellTreeDisp; //技能书
    private TalismanManager talisDisp; //符箓的component
    private bool earthUnlocked; 
    public bool clickedObject = false;
    public bool brightBackpack = false;
    public bool brightTalisman = false;
    private bool brightSpell = false;
    private bool backpackUnlocked, spellTreeUnlocked, talismanUnlocked; //玩家是否能使用背包、符箓、技能书
    public bool seenSpellTree = false; //看过一次技能书
    private Sprite talismanIcon;
    private ClickInScene pick; //拾起物品的component
    GraphicRaycaster raycaster;
    PointerEventData pointerData;
    EventSystem eventSystem;

    // private LeaveIconBright light;
    private bool isTalis = false;

    private static ClickManagement showInstance;
    
    // 此Component将会在每一个Scene出现且永不被删除/切换Scene不会消失，所有variable不会被初始化
    void Awake() {
        DontDestroyOnLoad(this);

        if (showInstance == null) {
            showInstance = this;
        }
        else {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Destroy(GameObject.Find("Theme Song"));
        // light = FindObjectOfType<LeaveIconBright>();
    }

    private void Start() {
        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();

        goManager = GameObject.Find("GameObjectManager");
        go = goManager.GetComponent<GOManagement>();

        pick = goManager.GetComponent<ClickInScene>();
        talisDisp = go.talisman.GetComponent<TalismanManager>();
        spellTreeDisp = go.spelltree;

        go.backpackIcon.GetComponent<Image>().enabled = false;
        go.talismanIcon.GetComponent<Image>().enabled = false;
        go.spelltreeIcon.GetComponent<Image>().enabled = false;
    }

    private void Update() {
        // 符箓或者技能书在屏幕上显示的时候不能捡起物品
        if (talisDisp.display.activeSelf || spellTreeDisp.activeSelf) {
            pick.descShow = false;
        } else {
            pick.descShow = true;
        }

        //Check if the left Mouse button is clicked
        if (raycaster == null) {

        }
        else if (Input.GetMouseButtonDown(0)) {
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
                
                if (name.CompareTo("TalismanIcon") == 0 && canAct) {
                    pick.descShow = false;
                    if(!isTalis) 
                        talisDisp.OpenTalisman();
                    else 
                        talisDisp.CloseDisplay();
                }
                //从背包里拖拽物品出来
                else if (tag.CompareTo("Item") == 0 && canAct) {
                    pick.descShow = false;
                    if (!ItemDragHandler.holdItem) {
                        ItemDragHandler.itemOnGround = result.gameObject;
                        ItemDragHandler.previousPosition = result.gameObject.GetComponent<RectTransform>().anchoredPosition;
                        ItemDragHandler.holdItem = true;
                        ItemDragHandler.x = ItemDragHandler.previousPosition.x;
                        RectTransform item_transform = ItemDragHandler.itemOnGround.GetComponent<RectTransform>();
                        ItemDragHandler.originalSize = item_transform.sizeDelta;
                    }
                }
                else if (name.CompareTo("SpelltreeIcon") == 0 && canAct) {
                    pick.descShow = false;
                    // UISoundScript.OpenSpellTree();
                    if (brightSpell) {
                        // GameObject.Find("DarkBackground").GetComponent<LeaveIconBright>().DarkBackpack();
                        // TipsDialog.PrintDialog("Spelltree 2");
                        brightSpell = false;
                        GameObject spellTree = GameObject.Find("SpelltreeIcon");
                        spellTree.GetComponent<Image>().sprite = Resources.Load<Sprite>("ChangeAsset/All elements");
                        seenSpellTree = true;
                    }
                    spellTreeDisp.SetActive(!spellTreeDisp.activeSelf);

                    // Close other canvas
                    talisDisp.CloseDisplay();
                    go.backpack.GetComponent<Backpack>().Show(!spellTreeDisp.activeSelf);
                }
                else if (name.CompareTo("Next Button") == 0) {
                    pick.descShow = false;
                    // TipsDialog.nextButton.GetComponent<NextButtonEffect>().ChangeNextButton();
                    // if (TipsDialog.isTyping){ // type full text
                    //     TipsDialog.PrintFullDialog();
                    // } else {
                    //     bool textActive = TipsDialog.NextPage();
                    //     // print("text act" + textActive);
                    //     GameObject.Find("Dialog Box").SetActive(textActive);
                    //     // check for water boss-->credits scene
                    //     if (!textActive) {
                    //         TipsDialog.CheckCurrentTipForNextMove();
                    //     }
                    // }
                } 
                // else if (tag.CompareTo("OptionButton") == 0) {
                //     TipsDialog.PlayOption(name);
                // }
                // else if (name.CompareTo("OptionButton") == 0) {
                //     InGameMenu.Option();
                // }
                // else if (name.CompareTo("ControlButton") == 0) {
                //     InGameMenu.Control();
                // }
                // else if (name.CompareTo("MainMenuButton") == 0) {
                //     InGameMenu.MainMenu();
                // }
                // else if (name.CompareTo("QuitButton") == 0) {
                //     InGameMenu.QuitGame();
                // }
            }
            //如果没有物品在UI layer且在当前鼠标下，玩家试图在捡起物品
            if (resultSize == 0) {
                pick.ClickOnGround();
            }
        }
        //按右键于背包物品上时，删除此物品
        else if (Input.GetMouseButtonDown(1)) {
            //Set up the new Pointer Event
            pointerData = new PointerEventData(eventSystem);
            pointerData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            int resultSize = 0;

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results) {
                resultSize += 1;
                string name = result.gameObject.name;
                string tag = result.gameObject.tag;

                if (tag.CompareTo("Item") == 0 && canAct) {
                    pick.descShow = false;
                    if (!ItemDragHandler.holdItem) {
                        int position = ((int)result.gameObject.GetComponent<RectTransform>().anchoredPosition.x + 680) / 80;
                        go.backpack.GetComponent<Backpack>().RemoveItem(result.gameObject, position);
                        break;
                    }
                }
            }
        }
        //点Q开技能书
        else if (Input.GetKeyDown(KeyCode.Q) && canAct && spellTreeUnlocked) {
            pick.descShow = false;
            // UISoundScript.OpenSpellTree();
            if (brightSpell) {
                // GameObject.Find("DarkBackground").GetComponent<LeaveIconBright>().DarkBackpack();
                // TipsDialog.PrintDialog("Spelltree 2");
                brightSpell = false;
                // go.spelltreeIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>("ChangeAsset/All elements");
                seenSpellTree = true;
            }
            spellTreeDisp.SetActive(!spellTreeDisp.activeSelf);
            go.backpack.GetComponent<Backpack>().Show(!spellTreeDisp.activeSelf);

            // Close other canvas
            talisDisp.CloseDisplay();
        }
    }

    //如果你打开一个功能（如符箓、技能书等），对应的Icon会消失；关闭此功能，Icon会重新显示；配合ToggleIcons()使用
    public void ToggleTalis(bool isShow) {
        isTalis = isShow;
        go.talismanIcon.GetComponent<Image>().enabled = isShow;
    }

    //关闭所有UI；只在Talisman时候需要（涉及图层问题）
    public void CloseDisplays() {
        go.backpack.GetComponent<Backpack>().Show(false);
        spellTreeDisp.SetActive(false);
    }

    //以下三个方法会同时激活对应的功能/能被玩家使用
    public void ShowBackpackIcon() { 
        go.backpackIcon.GetComponent<Image>().enabled = true; 
        backpackUnlocked = true;
    }

    public void ShowTalismanIcon() { 
        go.talismanIcon.GetComponent<Image>().enabled = true; 
        // GameObject.Find("DarkBackground").GetComponent<LeaveIconBright>().ShineTalisman();
        brightTalisman = true;
    }

    public void EnableTalisman() {
        talismanUnlocked = true;
        DontDestroyVariables.canOpenTalisman = true;
    }

    public void ShowSpelltreeIcon() { 
        go.spelltreeIcon.GetComponent<Image>().enabled = true; 
        // GameObject.Find("DarkBackground").GetComponent<LeaveIconBright>().ShineSpellIcon();
        brightSpell = true;
        spellTreeUnlocked = true;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "scene0" && !earthUnlocked
            && go.spelltreeIcon.GetComponent<Image>().enabled) {
            earthUnlocked = true;
            GetComponent<SpelltreeManager>().UnlockElement(TalisDrag.Elements.EARTH);
        }

        if (SceneManager.GetActiveScene().name == "EarthRoom" && !earthUnlocked) {
            earthUnlocked = true;
        }
    }

    //在你打开一个功能（如符箓、技能书等）时，隐藏图标
    public void ToggleIcons(bool isOn) {
        if (!isOn || backpackUnlocked) go.backpackIcon.GetComponent<Image>().enabled = isOn;
        if (!isOn || spellTreeUnlocked) go.spelltreeIcon.GetComponent<Image>().enabled = isOn;
        if (!isOn || talismanUnlocked) go.talismanIcon.GetComponent<Image>().enabled = isOn;

        if (!isOn) CloseDisplays();
    }

    //如果传入true，处于游戏锁死状态，隐藏所有UI图标，关闭符箓界面（UI功能暂时无法使用）
    public void ToggleLock(bool isLock) {
        ToggleIcons(!isLock);
        if(isLock){
            go.talisman.GetComponent<TalismanManager>().CloseDisplay();
        }
        // TipsDialog.ToggleTextBox(!isLock);
        lockGame = isLock;
        
        // if(light != null) 
        //     light.gameObject.SetActive(!isLock);
    }
}