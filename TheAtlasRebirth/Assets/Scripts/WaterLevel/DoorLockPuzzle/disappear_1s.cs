using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disappear_1s : MonoBehaviour
{
    // 
    public void Wait()
    {
        StartCoroutine(WaitAfterseconds(1.5f));
    }
    //child head disappear after 1.5s 
    private IEnumerator WaitAfterseconds(float wait)
    {
        Debug.Log("this is: " + wait);

        yield return new WaitForSeconds(wait);
        this.gameObject.SetActive(false);
        Debug.Log("after wait:" + wait);

    }
}

