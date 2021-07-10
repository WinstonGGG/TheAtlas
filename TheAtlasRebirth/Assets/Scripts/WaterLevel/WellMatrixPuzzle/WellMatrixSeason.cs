using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellMatrixSeason : MonoBehaviour
{
    private bool canEnlarge = true;
    public void Enlarge() {
        if (canEnlarge) {
            transform.localScale *= 2;
            canEnlarge = false;
        }
    }

    public void Shrink() {
        if (!canEnlarge) {
            transform.localScale /= 2;
            canEnlarge = true;
        }
    }
}
