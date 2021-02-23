using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ClickManagement))]
[RequireComponent(typeof(SpelltreeManager))]
public class TalismanManager : MonoBehaviour {
    private GameObject goManager;
    private GOManagement go;

    public bool dialogShown = false; //文本框是否显示
        // => FindObjectOfType<TipsDialog>() != null && !FindObjectOfType<ClickManagement>().lockGame;

    // Main display variables
    public GameObject display; //符箓GameObject
    public float timer; //初步判定 是废的
    // Element variables
    public GameObject[] elements; //所有符箓上的元素GameObject
    public Vector3[] elePos; //前一行元素的位置

    private float curTime;
    // Cookbook variables
    private TalisDrag.Elements[] craft = new TalisDrag.Elements[3]; //当前符箓上的元素
    public Image[] slots; //符箓内放置元素的三个位置
    private Spell[] recipeBook; //Recipe（技能配方）

    private Backpack backpack;
    private ClickManagement dispManager;
    private SpelltreeManager spelltreeManager;

    private GameObject textbox; //符箓上显示元素的文本框
    private Text eleName; //前一行的文本

    private bool firstAccess = true; //第一次调出符箓界面
    // public Animator talis;

    public bool TenSecTimer = false; //十秒之后变true；十秒不动，提醒玩家符箓操作
    public float countdownTime = 20.0f;
    public float timeLeft; //这两行是辅助TenSecTimer的工具
    // public GameObject atlas;
    
    private int PickButton = 0; // 点button请神or重绘
    private GameObject sLevelScroll; //右边符箓卷轴

    void Awake() {
        ResetCraft();
        ResetCraft();//不太懂为啥有俩
        curTime = 0;

        TenSecTimer = false;
        timeLeft = countdownTime;
        // atlas.SetActive(false);
    }

    void Start() {
        goManager = GameObject.Find("GameObjectManager");
        go = goManager.GetComponent<GOManagement>();

        dispManager = go.mainUI.GetComponent<ClickManagement>();
        spelltreeManager = go.spelltree.GetComponent<SpelltreeManager>();
        display = go.talisman;
        backpack = go.backpack.GetComponent<Backpack>();
        textbox = go.talisElementDesc;
        eleName = go.talisElementText.GetComponent<Text>();
        sLevelScroll = go.secondLevelScroll;

        sLevelScroll.SetActive(false);
        display.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        //按T打开符箓
        if (Input.GetKeyDown(KeyCode.T)) {
            OpenTalisman();
        }
        //Countdown for Dialog Pop up for idle mouse
        if (TenSecTimer && timeLeft > 0) timeLeft -= Time.deltaTime;
        if (TenSecTimer && timeLeft < 0)
        {
            // Create a Talisman Tip and call it here
            TipsDialog.PrintDialog("TalismanTip");
            TenSecTimer = false;
        }

        //按G生成技能物品
        if ((Input.GetKeyDown(KeyCode.G) || PickButton == 1) && display.activeSelf) {
            if(MakeItem()) dispManager.ToggleIcons(true);
            PickButton = 0;
        }
        //按删除键重置符箓
        else if ((Input.GetKeyDown(KeyCode.Backspace) || PickButton == 2) && display.activeSelf) {
            ResetCraft();
            // talis.SetTrigger("newTalis");
            PickButton = 0;
        }
    }
    // 点击请神/重绘时，生成技能物品/重置符录（update内引用pickbutton）
    public void ClickTalismanButton(string button) {
        if (button == "QingShenButton") {
            Debug.Log("get");
            PickButton = 1;
        }
        else if (button == "ChongHuiButton") {
            Debug.Log("chonghui");
            PickButton = 2;
        }
    }

    public void OpenTalisman() {
        if (dispManager.brightTalisman) {
            // GameObject.Find("DarkBackground").GetComponent<LeaveIconBright>().DarkBackpack();
            dispManager.brightTalisman = false;
        }

        if (!display.activeSelf && !dialogShown && DontDestroyVariables.canOpenTalisman) {
            if (SceneManager.GetActiveScene().name != "SampleScene") //8块板子划划划，不知道有没有用
                // GameObject.Find("playerParticleEffect").GetComponent<castEffect>().castAni();
            if (firstAccess) {
                firstAccess = false;
                display.transform.SetSiblingIndex(2);//设置图层顺序，以后应该需要改
                // TipsDialog.PrintDialog("Talisman 2");
            } else {
                // Close any text box that is open
                // TipsDialog.HideTextBox();
            }
            recipeBook = spelltreeManager.GetSpellBook();
            display.SetActive(true);
            dispManager.ToggleIcons(false);
            dispManager.ToggleTalis(true);

            DisplaySpellList();
            curTime = timer;
            // UISoundScript.OpenTalisman();

            TenSecTimer = true;
            timeLeft = countdownTime;
            
        }
        else if (!dialogShown && DontDestroyVariables.canOpenTalisman) {
            if(CloseDisplay()) {
                // UISoundScript.OpenTalisman();
            }
        }
    }
    
    bool AnimatorIsPlaying(Animator animator){
        return animator.GetCurrentAnimatorStateInfo(0).length >
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    // Display the list of currently usable elements
    private void DisplaySpellList() {
        int curPos = 0;
        int curPos2 = 5;
        for (int i = 0; i < elements.Length; i++) {
            TalisDrag ele = elements[i].GetComponent<TalisDrag>();
            // Show element if it's unlocked
            if (!ele.locked && !ele.known) {
                elements[i].SetActive(true);
                if (ele.isLevelTwo()) {
                    sLevelScroll.SetActive(true); //第二卷轴开启
                    elements[i].GetComponent<RectTransform>().localPosition = elePos[curPos2];
                    elements[i].GetComponent<TalisDrag>().UpdateOrigin(elePos[curPos2]);
                    curPos2 += 1;
                }
                else {
                    elements[i].GetComponent<RectTransform>().localPosition = elePos[curPos];
                    elements[i].GetComponent<TalisDrag>().UpdateOrigin(elePos[curPos]);
                    curPos += 1;
                }
            }
            else {
                elements[i].SetActive(false);
            }
        }
    }

    public void RemoveEle(int id) {
        if (craft[id] == TalisDrag.Elements.NONE) return;
        for (int i = id; i < craft.Length-1; i++) {
            slots[i].sprite = slots[i+1].sprite;
            craft[i] = craft[i+1];
            if (craft[i] == TalisDrag.Elements.NONE) slots[i].enabled = false;
            else slots[i].enabled = true;
        }
        craft[2] = TalisDrag.Elements.NONE;
        slots[2].enabled = false;
    }

    // Reset the craft log
    private void ResetCraft() {
        for (int i = 0; i < craft.Length; i++) {
            craft[i] = TalisDrag.Elements.NONE;
            slots[i].enabled = false;
        }
    }

    // Check if recipe can be made from current craft
    private bool CheckRecipe(Spell r) {
        TalisDrag.Elements[] curCraft = new TalisDrag.Elements[craft.Length];
        System.Array.Copy(craft, curCraft, craft.Length);

        for (int i = 0; i < curCraft.Length; i++) {
            if (curCraft[i] != TalisDrag.Elements.NONE) {
                if (r.recipe.Length < i+1) return false;
            }
        }

        // Make sure each element is matched
        for (int i = 0; i < r.recipe.Length; i++) {
            bool gotEle = false;
            for (int j = 0; j < craft.Length; j++) {
                if (r.recipe[i] == curCraft[j]) {
                    gotEle = true;
                    curCraft[j] = TalisDrag.Elements.NONE;
                    break;
                }
            }
            if (!gotEle) { return false; }
        }

        // Don't make empty elements
        if (r.recipe.Length <= 0) {
            return false;
        }
        return true;
    }

    // Functions to be called by other scripts
     

    // Close the entire talisman display
    public bool CloseDisplay() {
        // if(!AnimatorIsPlaying(talis)){
        if (true) {
            // if (SceneManager.GetActiveScene().name != "SampleScene")
            //     GameObject.Find("playerParticleEffect").GetComponent<castEffect>().stopCasting();
            display.SetActive(false);
            dispManager.ToggleTalis(false);
            dispManager.ToggleIcons(true);
            // go.backpack.GetComponent<Backpack>().Show(true);
            ResetCraft();
            curTime = 0;

            TenSecTimer = false;
            timeLeft = countdownTime;
            return true;
        }
        return false;
    }

    // Add an element to talisman
    public void AddCraft(TalisDrag.Elements e, Sprite s) {
        // AIDataManager.IncrementElementAccess(e);
        for (int i = 0; i < craft.Length; i++) {
            if(craft[i] == TalisDrag.Elements.NONE){
                craft[i] = e;
                slots[i].enabled = true;
                slots[i].sprite = s;
                break;
            }
        }
    }

    public void UnlockElement(TalisDrag.Elements e) {
        for (int i = 0; i < elements.Length; i++) {
            TalisDrag ele = elements[i].GetComponent<TalisDrag>();
            if (ele.element == e) {
                ele.locked = false;
                ele.known = false;
                break;
            }
        }
    }

    private bool MakeItem() {
        // Check if item can be made
        for (int i = 0; i < recipeBook.Length; i++) {
            if (CheckRecipe(recipeBook[i])) {
                // Add to backpack if it's not an element
                if (recipeBook[i].element == TalisDrag.Elements.NONE) {

                    //更新技能至老标志
                    if (backpack.CanAddItem()) {
                        // GetComponent<FlyingSpell>().FlyTowardsIcon(recipeBook[i].glow, false, recipeBook[i].spellName);
                        if (recipeBook[i].curState == Spell.SpellState.KNOWN) {
                            recipeBook[i].ChangeState(Spell.SpellState.UNLOCKED);
                            recipeBook[i].SetOld();
                        }

                        // Update old element status
                        for (int j = 0; j < recipeBook[i].recipe.Length; j++) {
                            if (recipeBook[i].recipe[j] != TalisDrag.Elements.NONE) {
                                spelltreeManager.SetElementToOld(recipeBook[i].recipe[j]);
                            }
                        }
                    }
                    
                    // AIDataManager.IncrementSpellAccess(recipeBook[i].spellName);
                }
                else {
                    // AIDataManager.DiscoverNewSpell(Time.time - AIDataManager.previousUnlockTime);
		            // AIDataManager.previousUnlockTime = Time.time;

                    spelltreeManager.UnlockElement(recipeBook[i].element);
                }
                CloseDisplay();
                return true;
            }
        }

        // Failed to make item
        // AIDataManager.TryNonExistentRecipe();
        // UISoundScript.PlayWrongSpell();
        ResetCraft();
        // talis.SetTrigger("newTalis");
        return false;

    }

    //显示元素名文本框
    public void DispTextBox(bool display, TalisDrag.Elements e, Vector2 position) {
        textbox.SetActive(display);
        eleName.text = e.ToString();
        textbox.GetComponent<TalisTextboxScaler>().UpdateBoxSize();
        textbox.transform.position = position;
    }

}