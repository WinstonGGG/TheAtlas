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
    public RawImage currentImage;
    public Texture savedEquipmentTexture;
    [HideInInspector]
    public ObItem equipedItemOb;

    void Start() {
        
    }

    public void equip(GameObject equipmentItem) {
        savedEquipmentTexture = equipmentItem.GetComponent<RawImage>().texture;
        if (currentImage != null) {
            putEquipmentTexture();
        }
        currentEquipmentName = equipmentItem.name;
        isEquiped = true;
        equipedItemOb = equipmentItem.GetComponent<ObItem>();
        print("just equip " + equipedItemOb);
    }

    public void putEquipmentTexture() {
        currentImage.texture = savedEquipmentTexture;
    }
}
