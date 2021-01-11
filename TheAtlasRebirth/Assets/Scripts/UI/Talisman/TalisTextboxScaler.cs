using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalismanTextboxScaler : MonoBehaviour {

    public GameObject box;
    public Text texty;

    public float baseLength, scaleLength;

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.X)) {
            UpdateBoxSize();
        }
    }

    public void UpdateBoxSize() {
        float scalar = baseLength + texty.text.Length * scaleLength;
        box.transform.localScale = new Vector3(scalar,box.transform.localScale.y,box.transform.localScale.z);
    }
}
