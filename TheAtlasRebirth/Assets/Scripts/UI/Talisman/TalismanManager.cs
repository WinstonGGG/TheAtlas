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

    public bool dialogShown = false;
        // FindObjectOfType<TipsDialog>() != null && !FindObjectOfType<ClickManagement>().lockGame;

    // Main display variables
    public GameObject display;
    public float timer;
    // Element variables
    public GameObject[] elements;
    public Vector3[] elePos;

    private float curTime;
    // Cookbook variables
    private TalisDrag.Elements[] craft = new TalisDrag.Elements[3];
    public Image[] slots;
    private Spell[] recipeBook;

    private Backpack backpack;
    private ClickManagement dispManager;
    private SpelltreeManager spelltreeManager;

    private GameObject textbox;
    private Text eleName;

    private bool firstAccess = true;
    // public Animator talis;

    public bool TenSecTimer = false;
    public float countdownTime = 20.0f;
    public float timeLeft;
    // public GameObject atlas;

    void Awake() {
        ResetCraft();
        ResetCraft();
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

        display.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            OpenTalisman();
        }
        //Countdown for Dialog Pop up for idle mouse
        if (TenSecTimer && timeLeft > 0) timeLeft -= Time.deltaTime;
        if (TenSecTimer && timeLeft < 0)
        {
            // Create a Talisman Tip and call it here
            // TipsDialog.PrintDialog("TalismanTip");
            TenSecTimer = false;
        }

        // TEST MAKE BUTTON
        if (Input.GetKeyDown(KeyCode.G) && display.activeSelf) {
            if(MakeItem()) dispManager.ToggleIcons(true);
        }
        else if (Input.GetKeyDown(KeyCode.Backspace) && display.activeSelf) {
            ResetCraft();
            // talis.SetTrigger("newTalis");
        }
    }

    public void OpenTalisman() {
        if (dispManager.brightTalisman) {
            // GameObject.Find("DarkBackground").GetComponent<LeaveIconBright>().DarkBackpack();
            dispManager.brightTalisman = false;
        }
        if (!display.activeSelf && !dialogShown && DontDestroyVariables.canOpenTalisman) {
            if (SceneManager.GetActiveScene().name != "SampleScene")
                // GameObject.Find("playerParticleEffect").GetComponent<castEffect>().castAni();
            if (firstAccess) {
                firstAccess = false;
                display.transform.SetSiblingIndex(2);
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
                    elements[i].GetComponent<RectTransform>().localPosition =
                        elePos[curPos2];
                    elements[i].GetComponent<TalisDrag>().UpdateOrigin(elePos[curPos2]);
                    curPos2 += 1;
                }
                else {
                    elements[i].GetComponent<RectTransform>().localPosition =
                        elePos[curPos];
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

    // Add an element to the craft log
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

    public void DispTextBox(bool display, TalisDrag.Elements e, Vector2 position) {
        textbox.SetActive(display);
        eleName.text = e.ToString();
        textbox.GetComponent<TalisTextboxScaler>().UpdateBoxSize();
        textbox.transform.position = position;
    }

}