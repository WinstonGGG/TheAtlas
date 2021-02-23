using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Start is called before the first frame update
	private Vector3 dy;
	public bool init = true;  //controls the camera movement in the beginning
	bool isPrinted = false;
	//private Vector3 dz;
    void Start()
    {
        dy = new Vector3(0f,-1f,0.5f); //camera adjustment value per mouse scroll
		
    }

    // Update is called once per frame
    void Update()
    {
		float cameraY = this.transform.localPosition.y;
		float cameraZ = this.transform.localPosition.z;
		
		// camera initial movement
		if(init){
			if(cameraZ>= -40f){     
				init = false;   // stops the loop when camera is in the correct position.
			}
			Vector3 offset = new Vector3(0f,-0.1f,0.05f);
			this.transform.position += offset;
			
		}
		else if (!isPrinted){
			TipsDialog.PrintDialog("Self Introduction");
			TipsDialog.introAppear = true;
			isPrinted = true;
		}
		
		
		
        if(Input.GetAxis("Mouse ScrollWheel")>0f &&cameraZ <= -30f ){
			this.transform.position += dy;
			// Debug.Log(cameraY);
		}
		
		if(Input.GetAxis("Mouse ScrollWheel")<0f && cameraZ >= -55f ){
			this.transform.position -= dy;
			// Debug.Log(cameraY);
		}
    }
	
	
}