using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
 using UnityEngine.UI;
public class DoorLockPuzzle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler // required interface when using the OnPointerEnter method.
 
{
    // This event echoes the new local angle to which we have been dragged
    private float timeCount;
    private Camera myCam;
    private Vector3 screenPos;

    private Vector3 mouse;
    private float angleOffset;
    private bool isDraging=false;
    private void Start(){
        myCam=Camera.main;
    }
    private void Update(){
        if (Input.GetMouseButtonDown(0)){
           // screenPos = myCam.WorldToScreenPoint(transform.position);
        Vector3 vec4 =Input.mousePosition;
        angleOffset=(Mathf.Atan2(vec4.y-transform.position.y,vec4.x-transform.position.x))*Mathf.Rad2Deg;
        print("angle  is :");
        print(transform.eulerAngles);
        angleOffset=transform.eulerAngles.z-angleOffset;
        // print("vec4 is :");
        // print(vec4);
        //print("transform is :");
        //  print(transform.position);
        }
         
       if(isDraging){
        Vector3 vec3 =Input.mousePosition;
        float angle =(Mathf.Atan2(vec3.y-transform.position.y,vec3.x-transform.position.x))*Mathf.Rad2Deg;

        float temp=angle+angleOffset;

        print("angle is :   ");
        print(angle);
        print("angle offset is ");
        print(angleOffset);
         
        transform.eulerAngles=new Vector3(0,0,angle+angleOffset);
        print("the new angle  is :");
        print(transform.eulerAngles);
       }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("The cursor entered the selectable UI element. " + eventData);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
       // Debug.Log("The cursor clicked the selectable UI element. " + eventData);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
       // Debug.Log("The cursor exited the selectable UI element. " + eventData);
    }
    public void OnBeginDrag(PointerEventData data)
    {
         //Get the Screen positions of the object
    
    }
    public void OnDrag(PointerEventData data)
    
    {
            isDraging=true;
        //print("drag working");
         
    }
    public void OnEndDrag(PointerEventData data)
    {
       // Debug.Log("OnEndDrag: " + data.position);
       isDraging=false;
    }

    public void test(){

    }

 
}