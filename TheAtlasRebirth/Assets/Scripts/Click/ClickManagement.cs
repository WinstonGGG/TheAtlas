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
    
    public bool canAct => !dialogShown && !lockGame;
    public bool lockGame = false;
    public bool dialogShown = false;
    //     => FindObjectOfType<TipsDialog>() != null;
    private GameObject spellTreeDisp;
    private TalismanManager talisDisp;
    private bool earthUnlocked;
    public bool clickedObject = false;
    public bool brightBackpack = false;
    public bool brightTalisman = false;
    private bool brightSpell = false;
    private bool backpackUnlocked, spellTreeUnlocked, talismanUnlocked;
    public bool seenSpellTree = false;
    private Sprite talismanIcon;
    private ClickInScene pick;
    GraphicRaycaster raycaster;
    PointerEventData pointerData;
    EventSystem eventSystem;

    // private LeaveIconBright light;
    private bool isTalis = false;

    private static ClickManagement showInstance;
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
        go.spelltreeIcon.GetComponent<Image>().enabled = false;
    }

    private void Update() {
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

            Debug.Log("click somewhere");

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results) {
                resultSize += 1;
                string name = result.gameObject.name;
                string tag = result.gameObject.tag;
                
                if (name.CompareTo("TalismanIcon") == 0 && canAct) {
                    Debug.Log("click talismanIcon");
                    pick.descShow = false;
                    if(!isTalis) 
                        talisDisp.OpenTalisman();
                    else 
                        talisDisp.CloseDisplay();
                }
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
            if (resultSize == 0)
                pick.ClickOnGround();
        }
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

    public void ToggleTalis(bool isShow) {
        isTalis = isShow;
        GameObject.Find("TalismanIcon").GetComponent<Image>().enabled = isShow;
    }

    public void CloseDisplays() {
        go.backpack.GetComponent<Backpack>().Show(false);
        spellTreeDisp.SetActive(false);
    }

    public void ShowBackpackIcon() { 
        GameObject.Find("BackpackIcon").GetComponent<Image>().enabled = true; 
        backpackUnlocked = true;
    }

    public void ShowTalismanIcon() { 
        GameObject.Find("TalismanIcon").GetComponent<Image>().enabled = true; 
        // GameObject.Find("DarkBackground").GetComponent<LeaveIconBright>().ShineTalisman();
        DontDestroyVariables.canOpenTalisman = true;
        brightTalisman = true;
        talismanUnlocked = true;
    }

    public void ShowSpelltreeIcon() { 
        GameObject.Find("SpellTreeIcon").GetComponent<Image>().enabled = true; 
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

    public void ToggleIcons(bool isOn) {
        if (!isOn || backpackUnlocked) go.backpackIcon.GetComponent<Image>().enabled = isOn;
        if (!isOn || spellTreeUnlocked) go.spelltreeIcon.GetComponent<Image>().enabled = isOn;
        if (!isOn || talismanUnlocked) go.talismanIcon.GetComponent<Image>().enabled = isOn;

        if (!isOn) CloseDisplays();
        else if (backpackUnlocked) 
            go.backpack.GetComponent<Backpack>().Show(true);
    }

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