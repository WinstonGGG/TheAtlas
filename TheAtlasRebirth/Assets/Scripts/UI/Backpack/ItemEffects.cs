using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemEffects : MonoBehaviour
{
    public static GOManagement go;
    
    private static Hashtable itemOnPuzzle;
    public static string[] availablePutList;
    // public static SpellTreeManager s;
    public static GameObject flowerpot;
    public static ClickManagement dispManager;
    // public static TalismanManager talisDisp;
    private static bool spell = false;
    // private static string[] groundNames;
    private static bool taoBookOpened = false;

    void Start() {
        go = GameObject.Find("GameObjectManager").GetComponent<GOManagement>();

        spell = false;
        taoBookOpened = false;
        
        // s = GameObject.Find("MainUI").GetComponent<SpellTreeManager>();
        itemOnPuzzle = new Hashtable();
        itemOnPuzzle.Add("Earth Key", "EarthPortal");
        itemOnPuzzle.Add("Changable Soil", "River,Flowerpot,Flowerpot cld,FutureRock,EarthPortal");
        itemOnPuzzle.Add("Water Seed", "Flowerpot,Flowerpot cld");
        itemOnPuzzle.Add("Golden Wood", "法阵-scene2,Fire Seed,Left Reefs,Right Reefs,rockInWaterRoom");
        itemOnPuzzle.Add("Heavenly Water", "Flowerpot,Flowerpot cld,River Collider,Fire Seed,法阵-scene2");
        itemOnPuzzle.Add("Prime Sun", "Flowerpot,Flowerpot cld");
        itemOnPuzzle.Add("Taiji Key", "Water Boss Door");
        itemOnPuzzle.Add("Ditto Board", "Background,Flower 1,Flower 2,Flower 3,Flower 4,Flower 5,Flower 6");
        itemOnPuzzle.Add("Yin-Yang Portal", "atlasmap2");
        itemOnPuzzle.Add("Taoism Wind", "Wind Collider,法阵-scene2");

        // talisDisp = GameObject.FindObjectOfType<TalismanManager>();
        dispManager = GameObject.FindObjectOfType<ClickManagement>();
    }

    // public static void getGroundNames() {
    //     groundNames = GameObject.FindGameObjectsWithTag("Ground").Select(x => x.name).ToArray();
    // }

    public static bool canPlace(string item, string targetObj) {
        spell = false;
        if (item.CompareTo("Tao-Book") == 0) {
            taoBookOpened = true;
            return true;
        }
        else if (item.CompareTo("Talisman") == 0 || item.CompareTo("The Atlas") == 0) {
            return true;
        }
        string available = (string)itemOnPuzzle[item];
        if (available == null)
            return false;
        availablePutList = available.Split(',');
        for (int i = 0; i < availablePutList.Length; i++) {
            if (targetObj.CompareTo(availablePutList[i]) == 0){
                if (targetObj.CompareTo("Flowerpot") == 0 || targetObj.CompareTo("Flowerpot cld") == 0) {
                    if (item.CompareTo("Changable Soil") == 0) {
                        if (DontDestroyVariables.growState == 0) {
                            return true;
                        } else {
                            // TipsDialog.PrintDialog("Water Seed Grow Order 0");
                            return false;
                        }
                    } else if (item.CompareTo("Water Seed") == 0) {
                        if (DontDestroyVariables.growState == 1) {
                            return true;
                        } else {
                            // TipsDialog.PrintDialog("Water Seed Grow Order 1");
                            return false;
                        }
                    } else if (item.CompareTo("Heavenly Water") == 0){
                        if (DontDestroyVariables.growState == 2)
                            return true;
                        else if (DontDestroyVariables.growState < 2){
                            // TipsDialog.PrintDialog("Water Seed Grow Order 2");
                            return false;
                        } else {
                            // TipsDialog.PrintDialog("Water Seed Grow Order 2.1");
                            return false;
                        }
                    } else if (item.CompareTo("Prime Sun") == 0) {
                        if (DontDestroyVariables.growState == 3) {
                            return true;
                        } else if (DontDestroyVariables.growState < 3) {
                            // TipsDialog.PrintDialog("Water Seed Grow Order 3");
                            return false;
                        } else {
                            // TipsDialog.PrintDialog("Water Seed Grow Order 3.1");
                            return false;
                        }
                    }
                } else if (item.CompareTo("Earth Key") == 0) {
                    // if (GameObject.Find("EarthPortal").GetComponent<sceneTransition>().openScroll)
                    //     return true;
                    // else return false;
                }
                else {
                    return true;
                }
            } 
        }
        // TipsDialog.PrintDialog("Wrong Spell");
        return false;
    }

    private static void deleteSpellObject(string objName) {
        SpriteRenderer[] spellObjects = FindObjectsOfType<SpriteRenderer>();
        foreach (SpriteRenderer component in spellObjects){
            if (component.name.CompareTo(objName) == 0) {
                Destroy(component.gameObject);
                return ;
            }
        }
    }

    public static void puzzleEffect(string item, string position, Vector3 hitPoint) {
        if (item.CompareTo("Changable Soil") == 0 && position.CompareTo("FutureRock") == 0){
            GameObject futureRock = GameObject.Find("FutureRock");
            Destroy(futureRock);

            GameObject.Find("Rock10").transform.position = GameObject.Find("Rock Base (1)").transform.position + new Vector3(4.0f, 0.34f, 0f);
            GameObject.Find("Earth Key").transform.position = new Vector3(-1.34f, -1.765f, 3.5f);
            
            // AIDataManager.UpdateStandardSpellCount("Changable Soil", 1);
            // AIDataManager.UpdateStandardSpellCount("earth", 3);
        } 
        else if (item.CompareTo("Changable Soil") == 0 && position.CompareTo("River") == 0){
            GameObject river = GameObject.Find("River");
            river.GetComponent<BoxCollider>().enabled = false;
            river.GetComponent<SpriteRenderer>().enabled = false;
            GameObject.Find("River Sound 1").GetComponent<AudioSource>().enabled = false;
            GameObject.Find("River Sound 2").GetComponent<AudioSource>().enabled = false;
            GameObject.Find("River Sound 3").GetComponent<AudioSource>().enabled = false;
            GameObject.Find("River Collider").GetComponent<BoxCollider>().enabled = true;
            // s.UnlockElement(TalisDrag.Elements.WATER);
            
            // AIDataManager.UpdateStandardSpellCount("Changable Soil", 1);
            // AIDataManager.UpdateStandardSpellCount("earth", 3);

            // SpellEffectSounds.PlayDirt();
        } 
        else if (item.CompareTo("Heavenly Water") == 0 && position.CompareTo("River Collider") == 0){
            GameObject river = GameObject.Find("River");
            river.GetComponent<BoxCollider>().enabled = true;
            river.GetComponent<SpriteRenderer>().enabled = true;
            GameObject.Find("River Sound 1").GetComponent<AudioSource>().enabled = true;
            GameObject.Find("River Sound 2").GetComponent<AudioSource>().enabled = true;
            GameObject.Find("River Sound 3").GetComponent<AudioSource>().enabled = true;
            GameObject.Find("River Collider").GetComponent<BoxCollider>().enabled = false;

            GameObject player = GameObject.Find("Main Character");
            if (player.transform.position.z < -5.5) {
                player.transform.position = new Vector3(player.transform.position.x, 1.442091f, -5.5f);
            }
            // AIDataManager.wrongItemPlacementCount++;
        }
        else if ((item.CompareTo("Heavenly Water") == 0 || item.CompareTo("Taoism Wind") == 0) && position.CompareTo("法阵-scene2") == 0){
            fireLevel(2, hitPoint);
        }
        else if (item.CompareTo("Heavenly Water") == 0 && position.CompareTo("Fire Seed") == 0){
            fireLevel(1, hitPoint);
        }
        else if (item.CompareTo("Tao-Book") == 0){
            go.mainUI.GetComponent<ClickManagement>().ShowSpelltreeIcon();
            // TipsDialog.PrintDialog("Spelltree 1");
            GameObject.Find("Dialog Box").transform.SetSiblingIndex(6);
        } 
        else if (item.CompareTo("Talisman") == 0){
            go.mainUI.GetComponent<ClickManagement>().ShowTalismanIcon();
            // TipsDialog.PrintDialog("Talisman 1");
        } 
        else if (item.CompareTo("The Atlas") == 0){
            // talisDisp.atlas.SetActive(true);
            dispManager.ToggleIcons(false);
        }
        // else if ((item.CompareTo("Earth Key") == 0 || item.CompareTo("Changable Soil") == 0) && position.CompareTo("EarthPortal") == 0){
        //     GameObject earthPortal = GameObject.Find("EarthPortal");
        //     earthPortal.GetComponent<sceneTransition>().enterable = true;
        //     earthPortal.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ChangeAsset/Earth Portal");
        //     // if (item.CompareTo("Changable Soil") == 0) {
        //     //     AIDataManager.wrongItemPlacementCount++;
        //     // }
        // } 
        else if (item.CompareTo("Changable Soil") == 0 && (position.CompareTo("Flowerpot") == 0 || position.CompareTo("Flowerpot cld") == 0)){
            flowerpot.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ChangeAsset/Flowerpot/Flowerpot with dirt");

            // AIDataManager.UpdateStandardSpellCount("Changable Soil", 1);
            // AIDataManager.UpdateStandardSpellCount("earth", 3);

            DontDestroyVariables.growState = 1;
        } 
        else if (item.CompareTo("Ditto Board") == 0 && position.CompareTo("Background") == 0){
            // GameObject.Find("Background").GetComponent<TileMovement>().CheckDone();

            // AIDataManager.UpdateStandardSpellCount("Ditto Board", 1);
            // AIDataManager.UpdateStandardSpellCount("earth", 2);
            // AIDataManager.UpdateStandardSpellCount("water", 1);
        } 
        // else if (item.CompareTo("Ditto Board") == 0 && (position.CompareTo("Flower 1") == 0 || position.CompareTo("Flower 2") == 0 || position.CompareTo("Flower 3") == 0 || position.CompareTo("Flower 4") == 0 || position.CompareTo("Flower 5") == 0 || position.CompareTo("Flower 6") == 0)) {
        //     if (DontDestroyVariables.growState == 4) {
        //         GameObject[] mirrorArray = GameObject.Find("mirrors").GetComponent<mirrors>().mirrorArray;

        //         int rightIndex = 0;
        //         foreach (GameObject mirror in mirrorArray) {
        //             rightIndex++;
        //             if (mirror.GetComponent<flowerInMirror>().isRight) {
        //                 mirror.GetComponent<flowerInMirror>().ClickedCorrectMirror();
        //                 break;
        //             }
        //         }
        //         // TipsDialog.ditto = rightIndex;
        //         // TipsDialog.PrintDialog("Ditto Mirror");
        //     } else {
        //         // TipsDialog.PrintDialog("Ditto No Mirror");
        //     }

        // }
        else if (item.CompareTo("Water Seed") == 0 && (position.CompareTo("Flowerpot") == 0 || position.CompareTo("Flowerpot cld") == 0)){
            DontDestroyVariables.growState = 2;
            flowerpot.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ChangeAsset/Flowerpot/Flowerpot with seed");
        } 
        else if (item.CompareTo("Heavenly Water") == 0 && (position.CompareTo("Flowerpot") == 0 || position.CompareTo("Flowerpot cld") == 0)){
            DontDestroyVariables.growState = 3;
            flowerpot.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ChangeAsset/Flowerpot/Flowerpot with bud");

            // s.UnlockElement(TalisDrag.Elements.WOOD);

            // AIDataManager.UpdateStandardSpellCount("Heavenly Water", 1);
            // AIDataManager.UpdateStandardSpellCount("water", 3);
        }
        // else if (item.CompareTo("Golden Wood") == 0 && position.CompareTo("法阵-scene2") == 0){
        //     sceneTransition sceneTrans = GameObject.Find("法阵-scene2").GetComponent<sceneTransition>();
        //     GameObject.Find("法阵-scene2").GetComponent<SpriteRenderer>().enabled = false;
        //     GameObject.Find("Fire Boss").GetComponent<SpriteRenderer>().enabled = true;

        //     s.UnlockElement(TalisDrag.Elements.FIRE);

        //     // AIDataManager.UpdateStandardSpellCount("Golden Wood", 1);
        //     // AIDataManager.UpdateStandardSpellCount("wood", 3);
        //     // AIDataManager.UpdateStandardSpellCount("earth", 1);
        //     // AIDataManager.UpdateStandardSpellCount("wind", 2);
        //     // AIDataManager.UpdateStandardSpellCount("Yin-Yang Portal", 1);

        //     SpellEffectSounds.PlayFire();

        //     // TipsDialog.PrintDialog("Fire Boss");
        // } 
        else if (item.CompareTo("Golden Wood") == 0 && position.CompareTo("Fire Seed") == 0){
            fireLevel(3, hitPoint);
        }
        // else if (item.CompareTo("Prime Sun") == 0 && (position.CompareTo("Flowerpot") == 0 || position.CompareTo("Flowerpot cld") == 0)){
        //     DontDestroyVariables.growState = 4;
        //     flowerpot.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ChangeAsset/Flowerpot/Flowerpot with Flower and Sun");
            
        //     GameObject[] mirrorArray = GameObject.Find("mirrors").GetComponent<mirrors>().mirrorArray;
        //     foreach (GameObject mirror in mirrorArray) {
        //         mirror.GetComponent<flowerInMirror>().clickable = true;
        //     }

        //     GameObject.Find("cameraControl").GetComponent<cameraControl>().active = true;
        //     // TipsDialog.PrintDialog("Zoom In Active");

        //     // AIDataManager.UpdateStandardSpellCount("Prime Sun", 1);
        //     // AIDataManager.UpdateStandardSpellCount("sun", 2);
        //     // AIDataManager.UpdateStandardSpellCount("earth", 1);
        //     // AIDataManager.UpdateStandardSpellCount("fire", 2);
        // } 
        // else if (item.CompareTo("Taiji Key") == 0 && position.CompareTo("Water Boss Door") == 0){
        //     deleteSpellObject("Water Boss Door");
        //     GameObject.Find("doorLeft").GetComponent<doorController>().openDoor();
        //     GameObject.Find("doorRight").GetComponent<doorController>().openDoor();

        //     // AIDataManager.gentlypassthedoor = true; 
        // }
        // else if (item.CompareTo("Golden Wood") == 0 && (position.CompareTo("rockInWaterRoom") == 0 || position.CompareTo("Left Reefs") == 0 || position.CompareTo("Right Reefs") == 0)){
        //     Destroy(GameObject.Find(position));
        //     if (position.CompareTo("rockInWaterRoom") != 0) {
        //         Destroy(GameObject.Find("wall " + position));
        //     }
        //     // TipsDialog.PrintDialog("Stick Destroy Rock");
        //     // AIDataManager.gentlypassthedoor = false; 
        // }
        // else if (item.CompareTo("Taoism Wind") == 0 && position.CompareTo("Wind Collider") == 0){
        //     GameObject wind = GameObject.Find("WindGroup");
        //     wind.GetComponent<wind>().windFadeOut();

        //     DontDestroyVariables.windExist = false;

        //     // AIDataManager.UpdateStandardSpellCount("fire", 1);
        //     // AIDataManager.UpdateStandardSpellCount("wood", 1);
        //     // AIDataManager.UpdateStandardSpellCount("wind", 1);
        //     // AIDataManager.UpdateStandardSpellCount("Taoism Wind", 1);
        // }
        else if (item.CompareTo("Yin-Yang Portal") == 0 && position.CompareTo("atlasmap2") == 0){
            GameObject portal = GameObject.Find("WaterToEarthPortal");
            portal.GetComponent<SpriteRenderer>().enabled = true;
            portal.transform.position = hitPoint + new Vector3(0.0f, 0.6f, 0);
        } 
    }

    public static void fireLevel(int level, Vector3 hitPoint) {
        if (level == 0) {
            Destroy(GameObject.Find("Fire Seed"));
            DontDestroyVariables.fireLevel = 0;
        } else if (level == 1) {
            GameObject seed = GameObject.Find("Fire Seed");
            seed.name = "Cold Fire Seed";
            seed.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("spell/Cold Fire Seed");;
            // TipsDialog.PrintDialog("Cooled down");
            seed.tag = "Pickable";

            DontDestroyVariables.fireLevel = 1;
        } else if (level == 2) {
            GameObject flame = GameObject.Find("法阵-scene2");
            flame.GetComponent<SpriteRenderer>().enabled = false;
            flame.GetComponent<BoxCollider>().enabled = false;
            GameObject.Find("Firecld").GetComponent<BoxCollider>().enabled = false;

            GameObject seed = GameObject.Find("Fire Seed");
            seed.GetComponent<SpriteRenderer>().enabled = true;
            seed.GetComponent<BoxCollider>().enabled = true;
            seed.transform.position = hitPoint;
            seed.GetComponent<Rigidbody>().useGravity = true;

            DontDestroyVariables.fireLevel = 2;
        } else if (level == 3) {
            GameObject seed = GameObject.Find("Fire Seed");
            seed.GetComponent<SpriteRenderer>().enabled = false;
            seed.GetComponent<BoxCollider>().enabled = false;

            GameObject flame = GameObject.Find("法阵-scene2");
            flame.GetComponent<SpriteRenderer>().enabled = true;
            flame.GetComponent<BoxCollider>().enabled = true;
            GameObject.Find("Firecld").GetComponent<BoxCollider>().enabled = true;

            DontDestroyVariables.fireLevel = 3;
        }
    }
}
