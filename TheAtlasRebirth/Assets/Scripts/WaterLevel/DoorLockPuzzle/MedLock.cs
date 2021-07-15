using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
 using UnityEngine.UI;
public class MedLock : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler // required interface when using the OnPointerEnter method.
 
{
 

    private float angleOffset;
    private bool isDraging;

    public float CheckAngle;
    public static bool LockActionMed;
    public bool CounterClockWise;
    public int RotatedCircle;

    private void Start(){
        CheckAngle=0;
        CounterClockWise=true;
        LockActionMed=false;
        isDraging=false;
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
        if(currentAngle>30&&currentAngle<250&&currentAngle!=360){
            CounterClockWise=false;
        }       
        //print("drag working");
        //if(!CounterClockWise){
        //   print("warning!!!!!!!!!!!!");
        //} 
    }
    public void OnEndDrag(PointerEventData data)
    {
       // Debug.Log("OnEndDrag: " + data.position);
        isDraging=false;
        float currentAngle=360-transform.eulerAngles.z;
        if(currentAngle>260&&currentAngle<280&&CounterClockWise){
            print("Lock in position");
            LockActionMed=true;
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