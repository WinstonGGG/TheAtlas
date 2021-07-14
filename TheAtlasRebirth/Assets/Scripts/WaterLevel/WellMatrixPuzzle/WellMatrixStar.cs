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
    private WellMatrixAllStars allStarsInfo;

    void Start() {
        allStarsInfo = transform.parent.gameObject.GetComponent<WellMatrixAllStars>();
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
            allStarsInfo.shineState[this.gameObject.name] = false;
            hoveringFeedback.SetActive(false);
        } else {
            shine = true;
            allStarsInfo.shineState[this.gameObject.name] = true;
            hoveringFeedback.SetActive(true);
        }

        for (int i = 1; i <= 4; i++) { //i represent season, （1 spring, 2 summer, 3 winter, 4 fall）
            if (judgeState(i)) {
                allStarsInfo.seasonLogos[i-1].GetComponent<WellMatrixSeason>().Enlarge();
                if (i == 4) {
                    print("in Start");
                    print(allStarsInfo.ob.GetComponent<ObManagement>().correspondingOB);
                    // allStarsInfo.ob.GetComponent<ObManagement>().UpdateState(2);
                    allStarsInfo.PlayState2();
                }
            } else {
                allStarsInfo.seasonLogos[i-1].GetComponent<WellMatrixSeason>().Shrink();
            }
        }
        
    }

    // input: 季节（1 spring, 2 summer, 3 winter, 4 fall）；
    // output: 是否只有这个季节的星星被点亮
    private bool judgeState(int currentConfirmingState) {
        string correctSeason;
        string[] wrongSeasons;
        if (currentConfirmingState == 1) {
            correctSeason = "Spr";
            wrongSeasons = new string[] {"Sum", "Win", "Fal"};
        } else if (currentConfirmingState == 2) {
            correctSeason = "Sum";
            wrongSeasons = new string[] {"Spr", "Win", "Fal"};
        } else if (currentConfirmingState == 3) {
            correctSeason = "Win";
            wrongSeasons = new string[] {"Spr", "Sum", "Fal"};
        } else {
            correctSeason = "Fal";
            wrongSeasons = new string[] {"Spr", "Sum", "Win"};
        }

        bool correctAnswer = allStarsInfo.shineState["Mid"]; //temp variable
        if (!correctAnswer) return false; //if mid is not selected, no correct group is selected
        for (int i = 1; i <= 6; i++) {
            correctAnswer = correctAnswer && allStarsInfo.shineState[correctSeason+i];
        }
        for (int season = 0; season < 3; season++){
            for (int i = 1; i <= 6; i++) {
                correctAnswer = correctAnswer && !allStarsInfo.shineState[wrongSeasons[season]+i];
            }
        }

        return correctAnswer;
    }
}
