using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FlyingOnUI : MonoBehaviour {

    public float iconSpeed;
    public GameObject spellIcon;
    public Transform spellTreeIcon, backpackIcon;

    bool isSpell, isBackpack;
    Vector3 origin;
    private float timer;
    private string itemBuffer;
    public Backpack backpack;

    // Start is called before the first frame update
    void Start() {
        spellIcon.GetComponent<Image>().enabled = false;
        origin = spellIcon.transform.localPosition;
        timer = 2;
        itemBuffer = "";
    }

    // Update is called once per frame
    void Update() {
        if (isSpell || isBackpack) {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                Vector3 targetPos = spellTreeIcon.transform.localPosition;
                if (isBackpack) targetPos = backpackIcon.transform.localPosition;

                spellIcon.transform.localPosition = Vector3.MoveTowards(spellIcon.transform.localPosition, targetPos, iconSpeed * Time.deltaTime);
                if (Vector3.Distance(spellIcon.transform.localPosition, targetPos) < 1) {
                    if (isSpell) { isSpell = false; spellTreeIcon.GetComponent<ShakingIcon>().ShakeMe(); }
                    else { 
                        isBackpack = false; backpackIcon.GetComponent<ShakingIcon>().ShakeMe(); 
                        backpack.AddItem(itemBuffer);
                        itemBuffer = "";
                    }
                    ResetFlyingSpell();
                }

                if (isSpell) { spellIcon.transform.localScale = new Vector3(4, 4, 1); }
                else { spellIcon.transform.localScale = new Vector3(2, 2, 1); }
            }
        }
    }

    public void ResetFlyingSpell() {
        spellIcon.GetComponent<Image>().enabled = false;
        spellIcon.transform.localPosition = origin;
        isSpell = false;
        isBackpack = false;
        if(itemBuffer != "") {
            backpack.AddItem(itemBuffer);
            itemBuffer = "";
        }
    }

    public void FlyTowardsIcon(Sprite s, bool isSpell, string spell) {
        // if (SceneManager.GetActiveScene().name != "SampleScene")
        //     GameObject.Find("playerParticleEffect").GetComponent<castEffect>().stopCasting();
        if(itemBuffer != "") {
            backpack.AddItem(itemBuffer);
            itemBuffer = "";
        }
        
        spellIcon.transform.localPosition = origin;
        // if(GameObject.FindGameObjectWithTag("Player"))
        //     FindObjectOfType<pickupEffect>().castAni(GameObject.FindGameObjectWithTag("Player").transform.position);
        this.isSpell = isSpell;
        isBackpack = !isSpell;
        spellIcon.GetComponent<Image>().enabled = true;
        spellIcon.GetComponent<Image>().sprite = s;
        if (isSpell)    { spellIcon.transform.localScale = new Vector3(8, 8, 1); }
        else            { spellIcon.transform.localScale = new Vector3(4, 4, 1); }
        timer = 2;
        itemBuffer = spell;
    }
}