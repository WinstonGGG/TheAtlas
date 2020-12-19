using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakingIcon : MonoBehaviour
{
    private bool shaking = false;
    [SerializeField]
    private float shakeAmp;
    private float shakeAmt = 0f;
    private Vector3 originalPos;
    
    void Start() {
        originalPos = transform.position;
    }
    void Update () {
        if (shaking) {
            float randomFloat = Random.Range(0f, 1000f);
            Vector3 newPos = new Vector3(0f, 0f, 0f);
            if (randomFloat < 250) {
                newPos = new Vector3(-1f, -1f, 0f) * (shakeAmt * 0.02f) + originalPos;
                shakeAmt = -shakeAmp;
            }
            else if (randomFloat < 500) {
                newPos = new Vector3(1f, 1f, 0f) * (shakeAmt * 0.02f) + originalPos;
                shakeAmt = -shakeAmp;
            }
            else if (randomFloat < 750) {
                newPos = new Vector3(-1f, 1f, 0f) * (shakeAmt * 0.02f) + originalPos;
                shakeAmt = shakeAmp;
            }
            else {
                newPos = new Vector3(1f, -1f, 0f) * (shakeAmt * 0.02f) + originalPos;
                shakeAmt = shakeAmp;
            }
            
            newPos.z = transform.position.z;
            transform.position = newPos;
        }
    }

    public void ShakeMe() {
        StartCoroutine("ShakeNow");
    }

    IEnumerator ShakeNow() {
        if (shaking == false) {
            shaking = true;
        }

        yield return new WaitForSeconds(1f);

        shaking = false;
        transform.position = originalPos;
    }
}