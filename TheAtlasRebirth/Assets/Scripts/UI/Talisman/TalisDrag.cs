using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TalisDrag : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
    public enum Elements { METAL, WOOD, WATER, FIRE, EARTH, THUNDER, SUN, WIND, MOON, NONE };
    public Elements element;
    public bool locked, known;
    public Transform talisman, dispManager;

    private Vector3 origin;
    private bool setTalis;

    private TalismanManager Tmanager;

    public void OnPointerDown(PointerEventData pointerEventData) {
        //Output the name of the GameObject that is being clicked
        dispManager.GetComponent<TalismanManager>().AddCraft(element, GetComponentInChildren<Image>().sprite);
        if (Tmanager.TenSecTimer) Tmanager.timeLeft = Tmanager.countdownTime;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if(transform.localPosition == origin)
            dispManager.GetComponent<TalismanManager>().DispTextBox(true, element, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData) {
        dispManager.GetComponent<TalismanManager>().DispTextBox(false, element, eventData.position);
    }

    public bool isLevelTwo() {
        return element == Elements.THUNDER || element == Elements.SUN ||
            element == Elements.WIND || element == Elements.MOON;
    }

    // Reset position of talisman
    private void OnDisable() {
        transform.position = origin;
    }

    // Start is called before the first frame update
    void Start() {
        origin = gameObject.GetComponent<RectTransform>().localPosition;

        Tmanager = GameObject.Find("MainUI").GetComponent<TalismanManager>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void UpdateOrigin(Vector3 newPos) {
        origin = newPos;
    }
}