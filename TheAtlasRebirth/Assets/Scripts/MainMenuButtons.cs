using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    private PointerEventData pointerData;
    private EventSystem eventSystem;
    private GraphicRaycaster raycaster;
    // Start is called before the first frame update
    void Start() {
        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (raycaster == null) {

        }
        else if (Input.GetMouseButtonDown(0)) {
            pointerData = new PointerEventData(eventSystem);
            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            foreach (RaycastResult result in results) {
                string name = result.gameObject.name;
                // Debug.Log("print" + name);
                if (name.CompareTo("LoadButton") == 0) {
                    PlayGame();
                }
                else if (name.CompareTo("QuitButton") == 0){
                    QuitGame();
                }
            }
        }
    }

    private void PlayGame(){
        SceneManager.LoadScene("Tutorial");
    }
    private void QuitGame(){
      //  UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
