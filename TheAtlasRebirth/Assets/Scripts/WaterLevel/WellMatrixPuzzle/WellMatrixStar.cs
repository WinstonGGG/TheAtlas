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
    private GameObject[] correctStars;
    private GameObject[] wrongStars;

    void Start() {
        correctStars = transform.parent.gameObject.GetComponent<WellMatrixAllStars>().correctStars;
        wrongStars = transform.parent.gameObject.GetComponent<WellMatrixAllStars>().wrongStars;
    }
    
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

    public void Click() {
        if (shine) {
            shine = false;
            hoveringFeedback.SetActive(false);
        } else {
            shine = true;
            hoveringFeedback.SetActive(true);
            bool correctAnswer = true;
            for (int i = 0; i < correctStars.Length; i++) {
                correctAnswer = correctAnswer && correctStars[i].GetComponent<WellMatrixStar>().shine;
            }
            for (int i = 0; i < wrongStars.Length; i++) {
                correctAnswer = correctAnswer && !wrongStars[i].GetComponent<WellMatrixStar>().shine;
            }
            if (correctAnswer) {
                WellMatrixAllStars allStarsInfo = transform.parent.gameObject.GetComponent<WellMatrixAllStars>();
                allStarsInfo.fall.transform.localScale *= 2;
                allStarsInfo.ob.UpdateState(2);
            }
        }
        
    }
}
