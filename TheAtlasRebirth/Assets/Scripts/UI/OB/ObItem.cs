using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ObItem : MonoBehaviour
{
    private GOManagement go;
    public GameObject correspondingOB;
    // public int itemStateTotalNum;
    // public ItemState[] itemStateTotalNum;
    // [System.Serializable]
    // public struct ItemState
    // {
    // 	public enum StateType {image, animate};
    // 	public StateType State;
    // 	public Texture texture;
    // 	public AnimationClip animate;
    // }
    // [SerializeField]
	// public int CurrentState = 1;

    void Awake() {
        go = GameObject.Find("GameObjectManager").GetComponent<GOManagement>();
    }

    public void DuplicateToBackpackItem() {
        correspondingOB = go.ob.GetComponent<ObManagement>().transferToBackpackItemOB;
    }
}
