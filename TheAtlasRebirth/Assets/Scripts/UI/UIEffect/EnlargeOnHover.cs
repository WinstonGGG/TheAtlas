using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnlargeOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float enlargeScale;
    //鼠标移动至背包里物品上方时物品变大
    public void OnPointerEnter(PointerEventData eventData) {
        transform.localScale *= enlargeScale;
    }

    //鼠标移动至背包里物品上方时物品变小
    public void OnPointerExit(PointerEventData eventData) {
        transform.localScale /= enlargeScale;
    }
}
