using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TalisSet : MonoBehaviour, IPointerDownHandler {

    public int id;
    //点击元素会从符箓上移除已经选中的元素
    public void OnPointerDown(PointerEventData pointerEventData) {
        if (pointerEventData.button == PointerEventData.InputButton.Right) {
            FindObjectOfType<TalismanManager>().RemoveEle(id);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
