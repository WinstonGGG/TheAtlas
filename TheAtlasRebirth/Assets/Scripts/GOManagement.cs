using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject secondLevelScroll;
    // Start is called before the first frame update
    void Awake()
    {
        backpackIcon = GameObject.Find("BackpackIcon");
        spelltreeIcon = GameObject.Find("SpelltreeIcon");
        talismanIcon = GameObject.Find("TalismanIcon");

        backpack = GameObject.Find("Backpack");
        itemHolder = GameObject.Find("ItemHolder");
        itemPositionHolder = GameObject.Find("ItemPositionHolder");

        mainUI = GameObject.Find("MainUI");

        talisman = GameObject.Find("Talisman");

        spelltree = GameObject.Find("Spelltree");

        talisElementDesc = GameObject.Find("TElementDesc");
        talisElementText = GameObject.Find("W_TElementDesc");
        talisman1 = GameObject.Find("B_Talisman1");
        // spellDesc = GameObject.Find("spellDesc");

        
        secondLevelScroll = GameObject.Find("SLevelScroll");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
