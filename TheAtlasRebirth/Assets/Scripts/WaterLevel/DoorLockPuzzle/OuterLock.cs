using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
 using UnityEngine.UI;
public class OuterLock : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler // required interface when using the OnPointerEnter method.
 
{
 

    private float angleOffset;
    private bool isDraging;

    public static bool LockActionOuter;

    public bool OLock;

    public int RotatedCircle;

    public float CheckAngle;

    private void Start(){
        OLock=true;
        RotatedCircle=0;
        LockActionOuter=false;
        isDraging=false;
        CheckAngle=0;
    }
    private void Update(){
        if(MedLock.LockActionMed&&InnerLock.LockActionInner&&OuterLock.LockActionOuter){
            gameObject.SetActive(false);
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
        if(RotatedCircle>=1&&currentAngle>240){
            OLock=false;
        }
        CheckAngle=currentAngle;
        print(currentAngle);
        print(RotatedCircle);
        //print("drag working");
         
    }
    public void OnEndDrag(PointerEventData data)
    {
       // Debug.Log("OnEndDrag: " + data.position);
        isDraging=false;
        float currentAngle=360-transform.eulerAngles.z;
        print(currentAngle);
        print(OLock);
        if(currentAngle>215&&currentAngle<225&&RotatedCircle-1==0&&OLock){
            print("Lock in position!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            LockActionOuter=true;
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