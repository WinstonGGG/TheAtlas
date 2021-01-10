using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GOManagement : MonoBehaviour
{
    public GameObject backpackIcon;
    public GameObject spellTreeIcon;
    public GameObject backpack;
    public GameObject mainUI;

    public GameObject itemHolder;
    // Start is called before the first frame update
    void Awake()
    {
        backpackIcon = GameObject.Find("BackpackIcon");
        spellTreeIcon = GameObject.Find("SpellTreeIcon");
        backpack = GameObject.Find("Backpack");
        mainUI = GameObject.Find("MainUI");

        itemHolder = GameObject.Find("ItemHolder");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
