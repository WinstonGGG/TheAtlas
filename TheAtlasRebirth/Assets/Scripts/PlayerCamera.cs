using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Start is called before the first frame update
	private Vector3 dy;
	bool init = true;
	//private Vector3 dz;
    void Start()
    {
        dy = new Vector3(0f,-1f,0.5f);
		
		//dz = (0,0,-0.3f);
    }

    // Update is called once per frame
    void Update()
    {
		float cameraY = this.transform.localPosition.y;
		float cameraZ = this.transform.localPosition.z;
		if(init){
			if(cameraZ>= -40f){
				init = false;
			}
			Vector3 offset = new Vector3(0f,-0.1f,0.05f);
			this.transform.position += offset;
			
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