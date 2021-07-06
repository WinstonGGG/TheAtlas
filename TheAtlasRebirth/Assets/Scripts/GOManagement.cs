using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GOManagement : MonoBehaviour
{
    public GameObject backpackIcon;
    public GameObject spelltreeIcon;
    public GameObject talismanIcon;
    public GameObject backpack;
    public GameObject mainUI;
    public GameObject talisman;
    public GameObject spelltree;
    public GameObject talisElementDesc;
    public GameObject talisElementText;
    public GameObject talisman1;
    // public GameObject spellDesc;

    public GameObject itemHolder;
    public GameObject itemPositionHolder;
    public GameObject extraItemHolder;
    public GameObject secondLevelScroll;
    
    public GameObject ob;
    public GameObject characterState;
    public GameObject equipmentState;

    public GameObject textbox; //显示物品名称的文本框
    public GameObject equipButton; //显示物品名称的文本框
    public Text itemName; //前一行的文本

    public ClickManagement clickManagement;
    public GameObject characterCamera; //在主角身上的camera
    
    private static GOManagement showInstance;
    void Awake()
    {
        DontDestroyOnLoad(this);
        if (showInstance == null) {
            showInstance = this;
        }
        else {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;


        backpackIcon = GameObject.Find("BackpackIcon");
        spelltreeIcon = GameObject.Find("SpelltreeIcon");
        talismanIcon = GameObject.Find("TalismanIcon");

        backpack = GameObject.Find("Backpack");
        itemHolder = GameObject.Find("ItemHolder");
        itemPositionHolder = GameObject.Find("ItemPositionHolder");
        extraItemHolder = GameObject.Find("ExtraItemHolder");

        mainUI = GameObject.Find("MainUI");

        talisman = GameObject.Find("Talisman");

        spelltree = GameObject.Find("Spelltree");

        talisElementDesc = GameObject.Find("TElementDesc");
        talisElementText = GameObject.Find("W_TElementDesc");
        talisman1 = GameObject.Find("B_Talisman1");
        // spellDesc = GameObject.Find("spellDesc");
        secondLevelScroll = GameObject.Find("SLevelScroll");

        ob = GameObject.Find("OB");
        characterState = GameObject.Find("CharacterState");
        clickManagement = mainUI.GetComponent<ClickManagement>();

        characterCamera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        print("GO update");
        ob = GameObject.Find("OB");
        characterCamera = GameObject.Find("Main Camera");
    }
}
