using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
	public bool canAct = true;  // player movement will be disabled when canAct is false
	public float dx=0,dy=0,dz=0;  // player celocity caused by environment; for example wind. 
	public int isReverse = 1;    // if player movement is reversed   for water room
	public bool isMoving;   // check if player is moving ; for animation
	public float speed = 3;   //player speed
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canAct) {
			
			if (Input.GetKey("w")) {
				isMoving=true;
				if(Input.GetKey("a")){
					GetComponent<Rigidbody>().velocity = new Vector3(-0.75f*speed * isReverse+dx, 0+dy, 0.75f*speed * isReverse +dz);
				}else if(Input.GetKey("d")){
					GetComponent<Rigidbody>().velocity = new Vector3(0.75f*speed * isReverse +dx, 0, 0.75f * speed * isReverse +dz);
				}else {
					GetComponent<Rigidbody>().velocity = new Vector3(0+dx, 0, 1 * speed * isReverse +dz);
				}
			}
			else if (Input.GetKey("s")) {
				isMoving = true;
				if(Input.GetKey("a")){
					GetComponent<Rigidbody>().velocity = new Vector3(-0.75f*speed * isReverse+dx, 0, -0.75f*speed * isReverse +dz);
				}else if(Input.GetKey("d")){
					GetComponent<Rigidbody>().velocity = new Vector3(0.75f*speed * isReverse +dx, 0, -0.75f * speed * isReverse+dz);
				}else {
					GetComponent<Rigidbody>().velocity = new Vector3(0+dx, 0, -1 * speed * isReverse+dz);
				}
			}
			else if (Input.GetKey("a")) {
				isMoving = true;
				if(Input.GetKey("w")){
					GetComponent<Rigidbody>().velocity = new Vector3(-0.75f*speed * isReverse+dx, 0, 0.75f*speed * isReverse +dz);
				}else if(Input.GetKey("s")){
					GetComponent<Rigidbody>().velocity = new Vector3(-0.75f*speed * isReverse+dx, 0, -0.75f * speed * isReverse+dz);
				}else {
					GetComponent<Rigidbody>().velocity = new Vector3(-1 * speed * isReverse +dx, 0, 0+dz);
				}
			}
			else if (Input.GetKey("d")) {
				isMoving = true;
				if(Input.GetKey("w")){
					GetComponent<Rigidbody>().velocity = new Vector3(0.75f*speed * isReverse+dx, 0, 0.75f*speed * isReverse +dz);
				}else if(Input.GetKey("s")){
					GetComponent<Rigidbody>().velocity = new Vector3(0.75f*speed * isReverse+dx, 0, -0.75f*speed * isReverse+dz);
				}else {
					GetComponent<Rigidbody>().velocity = new Vector3(1 * speed * isReverse +dx, 0, 0);
				}
			}
			else {
				GetComponent<Rigidbody>().velocity = new Vector3(dx, dy, dz);
				isMoving = false;
			}
			
		}
    }
}
