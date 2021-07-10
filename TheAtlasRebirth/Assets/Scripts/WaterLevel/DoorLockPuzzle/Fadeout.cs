using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Fadeout : MonoBehaviour
{
    private Image img;
    public float waitTime;
    // 
    public void Wait()
    {
        img = GetComponent<Image>();
        Debug.Log("before start");
        StartCoroutine(WaitAfterseconds(waitTime));
        Debug.Log("after start");
    }
    //child head disappear after 1.5s 
    private IEnumerator WaitAfterseconds(float wait)
    {
        //Debug.Log("this is: " + wait);
          for (float i = wait; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        
        //Debug.Log("after wait:" + wait);

    }
}

