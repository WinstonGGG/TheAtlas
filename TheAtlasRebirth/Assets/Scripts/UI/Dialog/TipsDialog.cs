﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsDialog : MonoBehaviour {
    public bool isPaused => FindObjectOfType<ClickManagement>().lockGame;
    public static Text dialogText;
    public TextAsset textFile;
    public static int index; //特定对话时，表示第几行
    public static List<string> AllDialogTextlist = new List<string>(); //原先textlist
    public static List<string> CurrentDialogTextlist = new List<string>(); //原先textlist2
    public static string ToSceneName;
    public static GameObject dialog;
    public static List<string> dialogList;
    public static bool nextOnClick = false; //是否点了nextbutton
    public static int ditto = 0;
    public static string Line; //存一行对话的内容
    public static bool isStartTyping; //判断是否进入下一行开始打字
    public static bool isTyping = false; //判断是否正在打字中
    public static bool isPrintfull = false; //判断是否需要显示整段句子
    public float textspeed;
    private static string currDialogRef;
    public static GameObject nextButton;
    public static GameObject Option;
    // store option name(instrad of using GetOption)
    public static string GetOption;
    public static List<string> OptionList = new List<string>();
    public static Text OptionAText;
    public static Text OptionBText;
    public static Text OptionCText;
    public static Text OptionDText;
    public static bool getOption = false; //判断是否选择正确选项 
    public static bool isPickOption = false; //判断是否有选择题出现
    public static bool introAppear = false;
    private static GameObject goManager;
    private static GOManagement go;

    void Awake() {
        var linetext = textFile.text.Split('\n');
        index = 2;
        foreach (var line in linetext) {
            AllDialogTextlist.Add(line);
        }
        dialog = GameObject.Find("DialogBox");
        dialogText = GameObject.Find("DialogText").GetComponent<Text>();
        Option = GameObject.Find("G_Options");
        OptionAText = GameObject.Find("OptionAText").GetComponent<Text>();
        OptionBText = GameObject.Find("OptionBText").GetComponent<Text>();
        OptionCText = GameObject.Find("OptionCText").GetComponent<Text>();
        OptionDText = GameObject.Find("OptionDText").GetComponent<Text>();
        nextButton = GameObject.Find("NextButton");
        nextButton.GetComponent<NextButtonEffect>().effective = false;
        dialog.SetActive(false);
        dialog.SetActive(false);
        Option.SetActive(false);
        nextOnClick = false;
        ditto = 0;
        isTyping = false;
        isPrintfull = false;
        OptionList = new List<string>();
        getOption = false;
        isPickOption = false;
        introAppear = false;
        goManager = GameObject.Find("GameObjectManager");
        go = goManager.GetComponent<GOManagement>();
    }

    void Update() {
        if (isStartTyping) {
            //Stops the previous sentence
            StopAllCoroutines(); 
            StartCoroutine(Typing(Line));
        }
        // type full text if click next
        if (isTyping && isPrintfull) {
                StopAllCoroutines(); 
                dialogText.text = "";
                dialogText.text = Line;
                isTyping = false;
                isPrintfull = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isPaused && !isPickOption) {
            nextButton.GetComponent<NextButtonEffect>().ChangeNextButton();
            // type full text if press space
            if (isTyping) {
                StopAllCoroutines(); 
                dialogText.text = "";
                dialogText.text = Line;
                isTyping = false;
            } 
            // 若对话没有下一页
            else if(!NextPage()) {
                dialogText.text = "";
                dialog.SetActive(false);
                nextOnClick = false;
                CheckCurrentTipForNextMove();
            }
        }
        // 点击nextbutton时，load图像
        if (!isPickOption) {
	        if ((Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space)) && nextOnClick && !isPaused) {
	            GameObject.Find("NextButton").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Next Button");
	            nextOnClick = false;
	        }
	    }
    }

    public static void CheckCurrentTipForNextMove() {
        if (currDialogRef.CompareTo("Self Introduction") == 0) {
            //show backpack
            GameObject.Find("MainUI").GetComponent<ClickManagement>().ShowBackpackIcon();
            go.backpack.GetComponent<Backpack>().Show(true);
        } 
        else if (currDialogRef.CompareTo("Fire Boss") == 0) {
            GameObject.Find("法阵-scene2").GetComponent<SpriteRenderer>().enabled = true;
            GameObject.Find("Fire Boss").GetComponent<SpriteRenderer>().enabled = false;
        } 
    }

    public static bool NextPage() {
        // To make sure the conditions for activating multiple choice are met
        if (index < CurrentDialogTextlist.Count - 1 && CurrentDialogTextlist[index].CompareTo("Qiang Yu: And I have some questions for you:=【Click on the choices】") == 0) {
            //Active option button
            Option.SetActive(true);
            nextButton.SetActive(false);
            isPickOption = true;
            PrintOptions();
            return true;      	
	   	} 
        //对话目前所在的行数>对话总行数，行数复原=2，return对话没有下一页，结束对话框
        else if (index > CurrentDialogTextlist.Count - 1) {
    		index = 2;
            dialogText.text = "";
            GameObject.Find("NextButton").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Next Button");
            return false;
        }
        //if对话还有下一页
        Line = CurrentDialogTextlist[index].Replace("=", "\n"); //获取下一行对话内容
        isStartTyping = true;
        index ++;
        return true;
    }

    public static void PrintDialog(string objName) {
        if (objName.CompareTo("Self Introduction") == 0 && introAppear) {
            return ;
        }
        // dialogOrDesc = true;
        currDialogRef = objName;
        CurrentDialogTextlist.Clear(); //清空先前的对话内容
        // 是否有对应的对话内容
        if (AllDialogTextlist.Contains(objName)) { 
            int j = AllDialogTextlist.IndexOf(objName); //找此ojname的对话在所有对话中的第几行开始
            // 读取ojname的对话内容，存到CurrentDialogTextlist内
            while (AllDialogTextlist[j].CompareTo("---") != 0 ) {
                if (AllDialogTextlist[j].CompareTo("Ditto board copied") == 0) {
                    CurrentDialogTextlist.Add("Ditto board copied " + ditto + "th mirror.");
                } 
                else {
                    CurrentDialogTextlist.Add(AllDialogTextlist[j]);
                }
                j++;
            }
        }
        ToSceneName = objName;
        Line = CurrentDialogTextlist[1].Replace("=", "\n");
        isStartTyping = true;
        dialog.SetActive(true); 
    }

    public static void HideTextBox() {
        dialogText.text = "";
        dialog.SetActive(false);
    }

    public static void ToggleTextBox(bool show) {
        if (!show || (show && dialogText.text != "")) {
            dialog.SetActive(show);
        }
        if (show && dialogText.text != "") {
            isTyping = false;
            dialogText.text = Line;
        }
    }

    public static bool CallScene() {
        if ( ToSceneName.CompareTo("Water Boss 2") == 0) {
            return true;
        } 
        else {
            return false; 
        }
    }
    
    public static void PrintFullDialog() {
        isPrintfull = true;
    }
 
    public static void PlayOption(string Name) {
    	// store option name
        if (Name.CompareTo("OptionA") == 0) {
            OptionList.Add("A");
        } 
        else if (Name.CompareTo("OptionB") == 0) {
            OptionList.Add("B");
        }
        else if (Name.CompareTo("OptionC") == 0) {
            OptionList.Add("C");
        } 
        else if (Name.CompareTo("OptionD") == 0) {
            OptionList.Add("D");
        }
        //reset
        if (index == CurrentDialogTextlist.Count-1 && CurrentDialogTextlist[index].CompareTo("Me") == 0) {
            getOption = false;
            Option.SetActive(false);
            nextButton.SetActive(true);
            isPickOption = false;
            index = 2;
            print(OptionList[0] + OptionList[1] + OptionList[2] +OptionList[3]);
            PrintDialog("Water Boss 2");
        //next option
        } 
        else {
            PrintOptions();
        }
    }

    public static void PrintOptions() {
        index++;
        dialogText.text = CurrentDialogTextlist[index];
        OptionAText.text = CurrentDialogTextlist[++index];
        OptionBText.text = CurrentDialogTextlist[++index];
        OptionCText.text = CurrentDialogTextlist[++index];
        OptionDText.text = CurrentDialogTextlist[++index];
    }
    
    //打字效果
    IEnumerator Typing(string sentences) {
        isStartTyping = false;
        isTyping = true;
        dialogText.text = "";
         //print("sentences : " + sentences);
        foreach (char letter in sentences.ToCharArray()) {
            dialogText.text += letter;
            yield return new WaitForSeconds(textspeed);
        }
        isTyping = false; 
    }
}