using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
 using UnityEngine.UI;
public class InnerLock : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler // required interface when using the OnPointerEnter method.
 
{
    // record the current angle of the object
    private float angleOffset;
    //detect if user are dragging 
    private bool isDraging;
    //this lock is set to false and it will be true if user solve the puzzle
    public static bool LockActionInner;
    //this lock is used to dectect if user is moving the circle into wrong direction
    public bool Lock;
    //record how many circle that user rotated
    public int RotatedCircle;
    //record the angle after user's movement
    public float CheckAngle;
    //record the angle after user's movement(this will be modified in another fucntion)
    public float RestrictAngle;
    private void Start(){
        Lock=true;
        RotatedCircle=0;
        LockActionInner=false;
        isDraging=false;
        CheckAngle=0;
        RestrictAngle=0;
    }
    private void Update(){
        //if the whole puzzle is solved , lock user's access 
        if(MedLock.LockActionMed&&InnerLock.LockActionInner&&OuterLock.LockActionOuter){
            gameObject.SetActive(false);
            //GameObject.Find("UnLockedOuter").GetComponent<LockUpdate>().setTrue();
        }
        else{
            //moving along the cursor
            if (Input.GetMouseButtonDown(0)){
            Vector3 vec4 =Input.mousePosition;
            angleOffset=(Mathf.Atan2(vec4.y-transform.position.y,vec4.x-transform.position.x))*Mathf.Rad2Deg;

            angleOffset=transform.eulerAngles.z-angleOffset;

            }
         
            if(isDraging){
            Vector3 vec3 =Input.mousePosition;
            float angle =(Mathf.Atan2(vec3.y-transform.position.y,vec3.x-transform.position.x))*Mathf.Rad2Deg;
             float temp;
            float rotateVal=angle+angleOffset;
 
           // temp= Mathf.DeltaAngle(rotateVal, RestrictAngle);
        
     
           
  
        
            /* if(temp<=-1f&&RestrictAngle<=0&&rotateVal<=0){
            
                rotateVal=RestrictAngle-1f;
            }
            else if(temp>=1f&&RestrictAngle>=0&&rotateVal>=0){
               
                 rotateVal=RestrictAngle+1f;
            }
            else if(temp>=1f&&RestrictAngle<=0&&rotateVal>=0){
                rotateVal=RestrictAngle-1f;
            }
            else  if(temp<=-1f&&RestrictAngle>=0&&rotateVal<=0){
                rotateVal=RestrictAngle+1f;
            }
             else  if(temp>=1f&&RestrictAngle>=0&&rotateVal>=0){
                rotateVal=RestrictAngle+1f;
            }
               else  if(temp<=-1f&&RestrictAngle<=0&&rotateVal<=0){
                rotateVal=RestrictAngle-1f;
            }
            else if(RestrictAngle==360f||rotateVal==360f){

            }*/

         
    

         
   
            RestrictAngle=rotateVal;
            //do the rotation
            transform.eulerAngles=new Vector3(0,0,rotateVal);
            
            }
        }
        
    }
    
    public void OnDrag(PointerEventData data)
    
    {
        //check if the user violate the rule 
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
        //dectect if user move the circle into right position
        //user will have a range to statisfied requriements
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