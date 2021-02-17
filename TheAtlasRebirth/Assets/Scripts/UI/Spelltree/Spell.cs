using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Spell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private GameObject goManager;
    private GOManagement go;

    private SpelltreeManager spellTreeDisp;

    public enum SpellState { LOCKED, KNOWN, UNLOCKED }; //每个技能的激活状态，决定玩家能读到多少技能描述
    public SpellState curState; //当前技能的激活状态

    public string spellName; 
    public TalisDrag.Elements[] recipe; //需要哪些元素合成这个技能
    public int level; //1: 1级元素；2: 2级元素；3: 技能
    public string knownResp; //？？；可能是：known激活态给玩家提示的部分技能配方
    public string knownDes, unlockedDes; //known激活态/解锁激活态 的技能描述
    public TalisDrag.Elements element; //如果这个位置是某种元素，此variable记录元素名
    public GameObject locked; //技能/元素封印
    public GameObject newIcon; //新解锁元素的标志
    private Image newDisp; //前一行的Image Component
    public Sprite glow; //闪光的这个元素/技能标志

    private Sprite ogSprite; //最初的技能/元素标志
    private Vector3 ogPos, ogScale; //技能标志原始位置、大小
    private bool isNew = true; //是否新解锁
    

    void Start() {
        goManager = GameObject.Find("GameObjectManager");
        go = goManager.GetComponent<GOManagement>();

        spellTreeDisp = go.spelltree.GetComponent<SpelltreeManager>();
    }

    //鼠标位于技能/元素上时，会出现描述框
    public void OnPointerEnter(PointerEventData eventData) {
        spellTreeDisp.UpdateTextBox(this);
        Vector2 thisPosition = this.gameObject.transform.parent.GetComponent<RectTransform>().anchoredPosition;
        // print("position: " + thisPosition);
        
        if (thisPosition.x >= 200)    { thisPosition.x -= 330f; }
        else                        { thisPosition.x += 330f; }
        if (thisPosition.y >= -200)    { thisPosition.y -= 200f; }
        else                        { thisPosition.y += 200f; }
        spellTreeDisp.textBox.GetComponent<RectTransform>().anchoredPosition = thisPosition;
        transform.parent.gameObject.transform.localScale *= spellTreeDisp.scaleAmount;
    }

    //鼠标离开技能/元素上时，描述框消失
    public void OnPointerExit(PointerEventData eventData) {
        spellTreeDisp.HideTextBox();
        transform.parent.gameObject.transform.localScale /= spellTreeDisp.scaleAmount;
    }
    

    // Start is called before the first frame update
    void Awake() {
        ogPos = GetComponent<Transform>().localPosition;
        ogScale = GetComponent<Transform>().localScale;
        ogSprite = GetComponent<Image>().sprite;

        //未解锁技能/元素，有封印
        if (curState == SpellState.LOCKED) {
            locked.SetActive(true);
        }
        else {
            locked.SetActive(false);
        }
    }
    
    // Update is called once per frame
    void Update() {
        
    }

    //从技能书里激活此技能/元素；移除封印
    public void ChangeState(SpellState state) {
        if (curState == SpellState.LOCKED && state != SpellState.LOCKED) {
            GetComponent<Transform>().localPosition = ogPos;
            GetComponent<Transform>().localScale = ogScale;
            GetComponent<Image>().sprite = ogSprite;
        }

        curState = state;
    }

    //看过技能书后，不再显示newIcon
    public void SetOld() { 
        isNew = false; 
        newDisp.enabled = false; 
    }

    //判断显示哪种技能/元素描述（根据是否解锁）
    private void OnEnable() {
        
        newDisp = newIcon.GetComponent<Image>();
        Debug.Log(newDisp);
        if (curState == SpellState.LOCKED) {
            newDisp.enabled = false;
        }
        else { 
            newDisp.enabled = isNew; 
        }
        
        locked.SetActive(curState == SpellState.LOCKED);
    }
}
