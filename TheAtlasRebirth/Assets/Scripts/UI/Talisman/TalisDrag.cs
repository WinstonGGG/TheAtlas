using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
// 每一个元素的属性

public class TalisDrag : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
    private GameObject goManager;
    private GOManagement go;
    
    public enum Elements { METAL, WOOD, WATER, FIRE, EARTH, THUNDER, SUN, WIND, MOON, NONE };
    public Elements element;
    public bool locked, known; //元素是否被玩家所了解的状态：locked-看不到元素，known-false-只能看到部分元素解析；

    private Vector3 origin; //元素原本的位置
    private TalismanManager talismanManager; //符箓的总信息管理

    // Start is called before the first frame update
    void Start() {
        goManager = GameObject.Find("GameObjectManager");
        go = goManager.GetComponent<GOManagement>();

        origin = gameObject.GetComponent<RectTransform>().localPosition;
        talismanManager = go.talisman.GetComponent<TalismanManager>();
    }

    //点击元素就会在符箓上添加元素，如果10秒不动符箓，改timer
    public void OnPointerDown(PointerEventData pointerEventData) {
        //Output the name of the GameObject that is being clicked
        talismanManager.AddCraft(element, GetComponentInChildren<Image>().sprite);
        if (talismanManager.TenSecTimer) talismanManager.timeLeft = talismanManager.countdownTime;
    }

    //鼠标放到元素上会显示元素名
    public void OnPointerEnter(PointerEventData eventData) {
        if(transform.localPosition == origin)
            talismanManager.DispTextBox(true, element, transform.position);
    }

    //鼠标离开元素上会不显示元素名
    public void OnPointerExit(PointerEventData eventData) {
        talismanManager.DispTextBox(false, element, eventData.position);
    }

    //暂时废弃
    public bool isLevelTwo() {
        return element == Elements.THUNDER || element == Elements.SUN ||
            element == Elements.WIND || element == Elements.MOON;
    }

    // Reset position of talisman
    private void OnDisable() {
        transform.position = origin;
    }

    // Update is called once per frame
    void Update() {

    }

    //手动强行修改元素原始位置
    public void UpdateOrigin(Vector3 newPos) {
        origin = newPos;
    }
}