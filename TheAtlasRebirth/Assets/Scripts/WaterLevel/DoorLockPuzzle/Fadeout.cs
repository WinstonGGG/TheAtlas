using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
public class Fadeout : MonoBehaviour
{
    private Image img;
    public float waitTime;

    //Let childhead disappear after waitTime seconds
    public void Wait() {
        img = GetComponent<Image>();
        StartCoroutine(WaitAfterseconds(waitTime));
        //Debug.Log("after start");
        
    }

    //child head disappear after 1.5s 
    private IEnumerator WaitAfterseconds(float wait)
    {
        for (float i = wait; i >= 0; i -= Time.deltaTime) {
            // set color with i as alpha
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }

        gameObject.transform.parent.gameObject.SetActive(false);
        
        Debug.Log("after wait:" + wait);

    }
}

