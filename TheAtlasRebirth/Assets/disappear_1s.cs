using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disappear_1s : MonoBehaviour
{
    // 
    public GameObject ChildHead = null;
    public void Start()
    {
        ChildHead.SetActive(true);

        StartCoroutine(WaitAfterseconds(1.5f));        
                ChildHead.SetActive(false);

       

    }
    //child head disappear after 1.5s 
    private IEnumerator WaitAfterseconds(float wait)
    {
        Debug.Log("this is: "+wait);

        yield return new WaitForSeconds(wait);
        Debug.Log("after wait:"+ wait);

    }
}
