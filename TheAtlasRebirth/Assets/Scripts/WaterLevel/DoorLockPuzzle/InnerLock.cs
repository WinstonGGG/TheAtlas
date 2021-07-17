﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
 using UnityEngine.UI;
public class InnerLock : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler // required interface when using the OnPointerEnter method.
 
{
    // This event echoes the new local angle to which we have been dragged
 

    private float angleOffset;
    private bool isDraging;

    public static bool LockActionInner;

    public bool Lock;

    public int RotatedCircle;

    public float CheckAngle;

    private void Start(){
        Lock=true;
        RotatedCircle=0;
        LockActionInner=false;
        isDraging=false;
        CheckAngle=0;
    }
    private void Update(){
        if(MedLock.LockActionMed&&InnerLock.LockActionInner&&OuterLock.LockActionOuter){
            gameObject.SetActive(false);
            //GameObject.Find("UnLockedOuter").GetComponent<LockUpdate>().setTrue();
        }
        else{
            if (Input.GetMouseButtonDown(0)){
            Vector3 vec4 =Input.mousePosition;
            angleOffset=(Mathf.Atan2(vec4.y-transform.position.y,vec4.x-transform.position.x))*Mathf.Rad2Deg;

            angleOffset=transform.eulerAngles.z-angleOffset;

            }
         
            if(isDraging){
            Vector3 vec3 =Input.mousePosition;
            float angle =(Mathf.Atan2(vec3.y-transform.position.y,vec3.x-transform.position.x))*Mathf.Rad2Deg;

            float temp=angle+angleOffset;
         
            transform.eulerAngles=new Vector3(0,0,angle+angleOffset);
            // print("the new angle  is :");
       
            // print(transform.eulerAngles.z);
            }
        }
        
    }
    
    public void OnDrag(PointerEventData data)
    
    {
        isDraging=true;
        float currentAngle=360-transform.eulerAngles.z;
        float gap=CheckAngle-currentAngle;
        if(currentAngle!=360&&CheckAngle>currentAngle&&gap>100){
            RotatedCircle+=1;
        }
        if(RotatedCircle>1&&currentAngle>300){
            Lock=false;
        }
        CheckAngle=currentAngle;

        print(RotatedCircle);
        //print("drag working");
         
    }
    public void OnEndDrag(PointerEventData data)
    {
       // Debug.Log("OnEndDrag: " + data.position);
        isDraging=false;
        float currentAngle=360-transform.eulerAngles.z;
        print(currentAngle);
        print(Lock);
        if(currentAngle>260&&currentAngle<280&&RotatedCircle-1==1&&Lock){
            print("Lock in position!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            LockActionInner=true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }
    public void OnPointerClick(PointerEventData eventData)
    {
    }
    public void OnPointerExit(PointerEventData eventData)
    {
    }
    public void OnBeginDrag(PointerEventData data)
    {
        
    }
 
}