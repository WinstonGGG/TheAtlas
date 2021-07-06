using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WellMatrixStar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{   
    // 闪亮效果GameObject Reference
    public GameObject hoveringFeedback;
    // 当前星辰是否被点亮
    private bool shine = false;
    
    public void OnPointerEnter(PointerEventData eventData) {
        print("Enter star");
        if (!shine)
            hoveringFeedback.SetActive(true);
    }

    //鼠标移动至背包里物品上方时物品变小
    public void OnPointerExit(PointerEventData eventData) {
        if (!shine)
            hoveringFeedback.SetActive(false);
    }

    public void Activate() {
        print("Shine star");
        shine = true;
        hoveringFeedback.SetActive(true);
    }
}
