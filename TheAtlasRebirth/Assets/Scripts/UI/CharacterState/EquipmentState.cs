using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentState : MonoBehaviour
{
    [HideInInspector]
    public bool isEquiped = false;
    [HideInInspector]
    public string currentEquipmentName;
    private RawImage currentImage;

    void Start() {
        currentImage = GetComponent<RawImage>();
    }

    public void equip(GameObject equipmentItem) {
        currentImage.texture = equipmentItem.GetComponent<RawImage>().texture;
        currentEquipmentName = equipmentItem.name;
        isEquiped = true;
    }
}
