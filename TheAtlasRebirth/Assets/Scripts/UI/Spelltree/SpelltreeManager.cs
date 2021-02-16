using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TalismanManager))]
public class SpelltreeManager : MonoBehaviour {
    private GameObject goManager;
    private GOManagement go;
    
    private List<Spell> spell = new List<Spell>(); //技能书上的所有激活的元素/技能
    private GameObject display;

    public float scaleAmount;

    // Textbox display
    public GameObject textBox; //技能书描述框
    public Text spellName, recipe, desc; //前一行的文本

    // Start is called before the first frame update
    void Start() {
        goManager = GameObject.Find("GameObjectManager");
        go = goManager.GetComponent<GOManagement>();

        display = go.spelltree;
        // textbox = go.spellDesc;

        UpdateSpell();
    }

    //激活技能
    private void UpdateSpell() {
        display.SetActive(true);
        spell = new List<Spell>();
        foreach (Transform child in transform) {
            Spell[] spellList = child.GetComponentsInChildren<Spell>();
            for (int i = 0; i < spellList.Length; i++)
                spell.Add(spellList[i]);
        }
        display.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        
    }

    //？？预测废了
    private void UpdateIcons() {
        for (int i = 0; i < spell.Count; i++) {
            if (spell[i].curState == Spell.SpellState.LOCKED) {
                Image img = spell[i].gameObject.GetComponent<Image>();
                var tempColor = img.color;
                tempColor.a = .5f;
                img.color = tempColor;
            }
        }
    }

    //便于屏幕上显示元素名
    private string EleToString(TalisDrag.Elements e) {
        switch (e) {
            case TalisDrag.Elements.EARTH: return "earth";
            case TalisDrag.Elements.FIRE: return "fire";
            case TalisDrag.Elements.METAL: return "metal";
            case TalisDrag.Elements.MOON: return "moon";
            case TalisDrag.Elements.SUN: return "sun";
            case TalisDrag.Elements.THUNDER: return "thunder";
            case TalisDrag.Elements.WATER: return "water";
            case TalisDrag.Elements.WIND: return "wind";
            case TalisDrag.Elements.WOOD: return "wood";
        }
        return "";
    }

    // 能否生成技能r
    private bool CanCraft(Spell r) {
        for (int i = 0; i < r.recipe.Length; i++) {
            bool gotEle = false; //技能配方里的第i个元素是否被解锁
            for (int j = 0; j < spell.Count; j++) {
                if (spell[j].curState == Spell.SpellState.UNLOCKED) {
                    if (r.recipe[i] == spell[j].element) {
                        gotEle = true;
                        break;
                    }
                }
            }
            //只要有一个元素没有解锁，这个技能就不能生成
            if (!gotEle && r.recipe[i] != TalisDrag.Elements.NONE) {
                return false;
            }
        }
        // 配方长度不对
        if (r.recipe.Length <= 0) return false;
        return true;
    }

    // Functions to be called by other scripts：更新描述
    public void UpdateTextBox(Spell s) {
        textBox.SetActive(true);
        if (s.curState == Spell.SpellState.LOCKED) {
            spellName.text = "? ? ?";
            recipe.text = "Recipe: ? ? ?";
            desc.text = "? ? ?";
        }
        else {
            spellName.text = s.spellName;

            if (s.curState == Spell.SpellState.UNLOCKED) {
                desc.text = s.unlockedDes;
                recipe.text = "Recipe: ";
                for (int i = 0; i < s.recipe.Length; i++) {
                    recipe.text += EleToString(s.recipe[i]);
                    if (i + 1 >= s.recipe.Length) break;
                    recipe.text += ", ";
                }
                if (s.recipe.Length <= 0) { recipe.text += "Fundamental Element"; }
            }
            else {
                desc.text = s.knownDes;
                recipe.text = s.knownResp;
            }
        }
    }

    public void HideTextBox() { textBox.SetActive(false); }

    public void UnlockElement(TalisDrag.Elements e) {
        UpdateSpell();
        // Unlock the element
        for (int i = 0; i < spell.Count; i++) {
            if (spell[i].element == e) {
                if (spell[i].curState != Spell.SpellState.UNLOCKED) {
                    spell[i].ChangeState(Spell.SpellState.UNLOCKED);
                    if (spell[i].element != TalisDrag.Elements.EARTH) {
                        go.mainUI.GetComponent<FlyingOnUI>().FlyTowardsIcon(spell[i].glow, true, "");
                        // UISoundScript.PlayGetElement();
                    }
                }
            }
            else if (spell[i].curState == Spell.SpellState.UNLOCKED) {
                spell[i].SetOld();
            }
        }

        // Make related recipes known if locked
        for (int i = 0; i < spell.Count; i++) {
            if (CanCraft(spell[i]) && spell[i].curState == Spell.SpellState.LOCKED){
                spell[i].ChangeState(Spell.SpellState.KNOWN);
            }
        }

        // Unlock in TalismanManager
        GetComponent<TalismanManager>().UnlockElement(e);
    }

    
    //获得已激活的元素/技能
    public Spell[] GetSpellBook() {
        UpdateSpell();
        Spell[] theList = new Spell[spell.Count];
        for (int i = 0; i < spell.Count; i++) {
            theList[i] = spell[i];
        }
        return theList;
    }

    //使用过的技能在技能书里不再显示newIcon
    public void SetElementToOld(TalisDrag.Elements e) {
        UpdateSpell();
        for (int i = 0; i < spell.Count; i++) {
            if (spell[i].element == e && spell[i].curState == Spell.SpellState.UNLOCKED) {
                spell[i].SetOld();
            }
        }
    }
    
}
