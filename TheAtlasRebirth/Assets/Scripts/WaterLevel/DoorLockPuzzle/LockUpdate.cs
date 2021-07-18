using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockUpdate : MonoBehaviour
{
    public Image img1;
 
    void Start()
    {
        //hide the outerUnlock image
        img1.enabled=false;
    }
 
    void Update() 
    {
        //if all locks are unlocked, set the image to true
        if(MedLock.LockActionMed&&InnerLock.LockActionInner&&OuterLock.LockActionOuter){
            img1.enabled=true;
        }
       
        
    }
    
}
