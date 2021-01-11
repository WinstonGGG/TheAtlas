using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TalismanManager))]
public class SpelltreeManager : MonoBehaviour {
    private GameObject goManager;
    private GOManagement go;
    
    private List<Spell> spell = new List<Spell>();
    private GameObject display;

    public float scaleAmount;

    // Textbox display
    public GameObject textBox;
    public Text spellName, recipe, desc;

    // Start is called before the first frame update
    void Start() {
        goManager = GameObject.Find("GameObjectManager");
        go = goManager.GetComponent<GOManagement>();

        display = go.spelltree;
        // textbox = go.spellDesc;

        UpdateSpell();
    }

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

    private bool CanCraft(Spell r) {
        for (int i = 0; i < r.recipe.Length; i++) {
            bool gotEle = false;
            for (int j = 0; j < spell.Count; j++) {
                if (spell[j].curState == Spell.SpellState.UNLOCKED) {
                    if (r.recipe[i] == spell[j].element) {
                        gotEle = true;
                        break;
                    }
                }
            }

            if (!gotEle && r.recipe[i] != TalisDrag.Elements.NONE) {
                return false;
            }
        }
        if (r.recipe.Length <= 0) return false;
        return true;
    }

    // Functions to be called by other scripts
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
                        // GetComponent<FlyingSpell>().FlyTowardsIcon(spell[i].glow, true, "");
                        // UISoundScript.PlayGetElement();
                    }
                }
                /// break;
            }
            else if (spell[i].curState == Spell.SpellState.UNLOCKED) {
                spell[i].SetOld();
            }
        }
     //   print("spell list: " + spell);

        // Make related recipes known if locked
        for (int i = 0; i < spell.Count; i++) {
            if (CanCraft(spell[i]) && spell[i].curState == Spell.SpellState.LOCKED){
            // if (CanCraft(spell[i]) && spell[i].element == TalisDrag.Elements.NONE) {
                spell[i].ChangeState(Spell.SpellState.KNOWN);
            }
        }

        // Unlock in TalismanManager
        GetComponent<TalismanManager>().UnlockElement(e);
    }

    

    public Spell[] GetSpellBook() {
        UpdateSpell();
        Spell[] theList = new Spell[spell.Count];
        for (int i = 0; i < spell.Count; i++) {
            theList[i] = spell[i];
        }
        return theList;
    }

    public void SetElementToOld(TalisDrag.Elements e) {
        UpdateSpell();
        for (int i = 0; i < spell.Count; i++) {
            if (spell[i].element == e && spell[i].curState == Spell.SpellState.UNLOCKED) {
                spell[i].SetOld();
            }
        }
    }
    
}
