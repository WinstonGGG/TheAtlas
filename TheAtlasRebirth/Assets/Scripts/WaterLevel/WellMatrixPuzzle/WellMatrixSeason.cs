using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellMatrixSeason : MonoBehaviour
{
    private bool canEnlarge = true;
    private string season;
    private RectTransform rect;

    void Start() {
        season = gameObject.name;
        rect = gameObject.GetComponent<RectTransform>();
    }
    public void Enlarge() {
        if (canEnlarge) {
            transform.localScale *= 1.5f;
            if (season.CompareTo("Spring") == 0) {
                rect.anchoredPosition = rect.anchoredPosition - new Vector2(25, 0);
            } else if (season.CompareTo("Fall") == 0) {
                rect.anchoredPosition = rect.anchoredPosition + new Vector2(25, 0);
            } else if (season.CompareTo("Summer") == 0) {
                rect.anchoredPosition = rect.anchoredPosition + new Vector2(0, 25);
            } else {
                rect.anchoredPosition = rect.anchoredPosition - new Vector2(0, 25);
            }
            canEnlarge = false;
        }
    }

    public void Shrink() {
        if (!canEnlarge) {
            transform.localScale /= 1.5f;
            if (season.CompareTo("Spring") == 0) {
                rect.anchoredPosition = rect.anchoredPosition + new Vector2(25, 0);
            } else if (season.CompareTo("Fall") == 0) {
                rect.anchoredPosition = rect.anchoredPosition - new Vector2(25, 0);
            } else if (season.CompareTo("Summer") == 0) {
                rect.anchoredPosition = rect.anchoredPosition - new Vector2(0, 25);
            } else {
                rect.anchoredPosition = rect.anchoredPosition + new Vector2(0, 25);
            }
            canEnlarge = true;
        }
    }
}
