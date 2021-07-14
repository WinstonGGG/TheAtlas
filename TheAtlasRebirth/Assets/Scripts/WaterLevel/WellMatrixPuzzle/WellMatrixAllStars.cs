using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellMatrixAllStars : MonoBehaviour
{
    public Dictionary<string, bool> shineState;
    public GameObject[] seasonLogos;
    private GOManagement go;
    public GameObject ob;
    public GameObject videoPlayer;
    public GameObject state2;

    
    void Start()
    {
        go = GameObject.Find("GameObjectManager").GetComponent<GOManagement>();
        ob = go.ob;

        shineState = new Dictionary<string, bool>();
        shineState.Add("Mid", false);
        shineState.Add("Fal1", false);
        shineState.Add("Fal2", false);
        shineState.Add("Fal3", false);
        shineState.Add("Fal4", false);
        shineState.Add("Fal5", false);
        shineState.Add("Fal6", false);
        shineState.Add("Sum1", false);
        shineState.Add("Sum2", false);
        shineState.Add("Sum3", false);
        shineState.Add("Sum4", false);
        shineState.Add("Sum5", false);
        shineState.Add("Sum6", false);
        shineState.Add("Spr1", false);
        shineState.Add("Spr2", false);
        shineState.Add("Spr3", false);
        shineState.Add("Spr4", false);
        shineState.Add("Spr5", false);
        shineState.Add("Spr6", false);
        shineState.Add("Win1", false);
        shineState.Add("Win2", false);
        shineState.Add("Win3", false);
        shineState.Add("Win4", false);
        shineState.Add("Win5", false);
        shineState.Add("Win6", false);
    }

    public void PlayState2() {
        UnityEngine.Video.VideoPlayer player = videoPlayer.GetComponent<UnityEngine.Video.VideoPlayer>();
        player.frame = 10;
        player.Play();
        StartCoroutine(AdjustLayer(0.5f, player));
    }

    IEnumerator AdjustLayer(float secs, UnityEngine.Video.VideoPlayer player) {
        yield return new WaitForSeconds(secs);
        state2.transform.SetSiblingIndex(1);
    }

}
