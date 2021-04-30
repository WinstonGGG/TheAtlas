using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ClickManagement))]
public class ClickInScene : MonoBehaviour
{
    public static GOManagement go;
    private ClickManagement dispManager;
    private ObManagement ob;
    
    public bool canAct => !obShown && !dialogShown && !talismanShown && !shineIcon && !FindObjectOfType<ClickManagement>().lockGame;
    public bool dialogShown => FindObjectOfType<TipsDialog>() != null;
    public bool talismanShown = false;
        // GameObject.Find("Talisman") != null;
        
    public bool shineIcon = false;
        // GameObject.Find("DarkBackground").GetComponent<LeaveIconBright>().shine == true;
    public bool obShown => GameObject.Find("OB") != null;

    private int layerMask;
    public bool dialogShow = false;
    public bool descShow = true;
    public int distanceToClick;
    public float cameraDistance = 0;

    public bool isTalismanPicked, isBrushPicked, isGourdPicked;

    private void Start() {
        go = GameObject.Find("GameObjectManager").GetComponent<GOManagement>();

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = 1 << 8;
        layerMask = ~layerMask;

        dispManager = go.mainUI.GetComponent<ClickManagement>();
        ob = go.ob.GetComponent<ObManagement>();

        // Check to see if current scene is the lobby if so show spell tree description
        // "Scene 0" name might be changed later
        if (SceneManager.GetActiveScene().name == "EarthRoom")
        {
            // ItemEffects.getGroundNames();
        }
        else if (SceneManager.GetActiveScene().name == "scene0"){
            dispManager.ToggleLock(false);
            // GameObject.Find("EarthSoundManager").GetComponents<AudioSource>()[2].volume = 0.3f;
            // ItemEffects.getGroundNames();
            // EarthSoundManager.StopPlaySound();
            if (DontDestroyVariables.firstTimeLobbyFlag){
                // TipsDialog.PrintDialog("Lobby");
                DontDestroyVariables.firstTimeLobbyFlag = false; // Used to tell if its the first time visiting the lobby scene
            } 
            else {
                Destroy(GameObject.Find("River Tip"));
                GameObject river = GameObject.Find("River");
                Destroy(river);
                Destroy(GameObject.Find("River Sound 1"));
                Destroy(GameObject.Find("River Sound 2"));
                Destroy(GameObject.Find("River Sound 3"));
            }

            ItemEffects.fireLevel(DontDestroyVariables.fireLevel, GameObject.Find("法阵-scene2").transform.position);
        }
        else if (SceneManager.GetActiveScene().name == "scene3") {
            dispManager.ToggleLock(false);
            // ItemEffects.getGroundNames();
            DontDestroyVariables.enterWaterRoom = true;
            ItemEffects.flowerpot = GameObject.Find("Flowerpot");
            // GameObject.Find("EarthSoundManager").GetComponents<AudioSource>()[2].volume = 0f;
        } 
    }

    // 捡起物品，或者点击物品；在场景里点东西，代码都应放置于此
    public void ClickOnGround(){
        if (descShow){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, (distanceToClick + cameraDistance), layerMask) && canAct) {
                GameObject clickObject = hitInfo.collider.gameObject;
                if (clickObject.tag == "Pickable"){
                    Debug.Log(clickObject.name);
                    if (clickObject.name.CompareTo("Cold Fire Seed") == 0) {
                        // ItemEffects.s.UnlockElement(TalisDrag.Elements.FIRE);
                        Destroy(clickObject);
                        DontDestroyVariables.fireLevel = 0;
                    }
                    else if (go.backpack.GetComponent<Backpack>().CanAddItem()) {
                        Sprite item = clickObject.GetComponent<SpriteRenderer>().sprite;
                        go.mainUI.GetComponent<FlyingOnUI>().FlyTowardsIcon(item, false, clickObject.name);
                        ob.GetObItemData(clickObject);
                        // GameObject.Find("pickupEffect").GetComponent<pickupEffect>().castAni(clickObject.transform.position);
                        Destroy(clickObject);
                    }
                } 
                else if (clickObject.name.CompareTo("PickableTailsman") == 0) {
                    Debug.Log("talisman");
                    isTalismanPicked = true;
                    dispManager.ShowTalismanIcon();
                    EnableTalismanIfAvailable();
                    Destroy(clickObject);
                }
                else if (clickObject.name.CompareTo("PickableBrush") == 0) {
                    Debug.Log("brush");
                    isBrushPicked = true;
                    dispManager.ShowTalismanIcon();
                    EnableTalismanIfAvailable();
                    Destroy(clickObject);
                }
                else if (clickObject.name.CompareTo("PickableGourd") == 0) {
                    isGourdPicked = true;
                    dispManager.ShowBackpackIcon();
                    Destroy(clickObject);
                }
                // else if (clickObject.name.CompareTo("Flower 1") == 0 || clickObject.name.CompareTo("Flower 2") == 0 || clickObject.name.CompareTo("Flower 3") == 0 || clickObject.name.CompareTo("Flower 4") == 0 || clickObject.name.CompareTo("Flower 5") == 0 || clickObject.name.CompareTo("Flower 6") == 0) {  
                //     clickObject.GetComponent<flowerInMirror>().ClickMirror();
                // }
                // if (clickObject.tag == "Portals") {
                //     ChangeSprite change = clickObject.GetComponent<ChangeSprite>();
                //     if (clickObject.name.CompareTo("EarthPortal") == 0) {
                //         if (change.Trigger) {
                //             if (clickObject.GetComponent<sceneTransition>().enterable)
                //                 TipsDialog.PrintDialog(clickObject.name + " Open");
                //             else
                //                 TipsDialog.PrintDialog(clickObject.name + " Wait Key");
                //         }
                //     } else {
                //         if (change.Trigger) {
                //             TipsDialog.PrintDialog(clickObject.name + " Open");
                //         }
                //     }
                //     if (!change.Trigger) {
                //         change.OpenScroll();
                //         TipsDialog.PrintDialog(clickObject.name);
                //     } 
                // } 
                // else if (clickObject.name.CompareTo("Flowerpot") == 0) {
                //     if (DontDestroyVariables.growState < 3) {
                //         TipsDialog.PrintDialog("Flowerpot");
                //     } else if (DontDestroyVariables.growState < 4) {
                //         TipsDialog.PrintDialog("Flower Need Sun");
                //     } else {
                //         TipsDialog.PrintDialog("Water Seed Grow Order 3.1");
                //     }
                // }
                // else if (TipsDialog.dialogList.Contains(clickObject.name)){
                //     TipsDialog.PrintDialog(clickObject.name);
                //     dialogShow = true;
                // }
                // UISoundScript.PlayPick();
            }
        }
        descShow = false;
    }

    public void ObOnGround() {
        if (descShow){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, (distanceToClick + cameraDistance), layerMask) && canAct) {
                GameObject clickObject = hitInfo.collider.gameObject;
                if (clickObject.GetComponent<ObItem>() != null) {
                    ob.GetItemType(clickObject);
                }
            }
        }
        descShow = false;
    }

    private void EnableTalismanIfAvailable() {
        if (isTalismanPicked && isBrushPicked) {
            dispManager.EnableTalisman();
        }
    }
}