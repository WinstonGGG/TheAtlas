using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
 using UnityEngine.UI;
public class DoorLockPuzzle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler // required interface when using the OnPointerEnter method.
 
{
    // This event echoes the new local angle to which we have been dragged float rotSpeed = 20;
      public Button theButton;
    private float timeCount;
 
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("The cursor entered the selectable UI element. " + eventData);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("The cursor clicked the selectable UI element. " + eventData);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("The cursor exited the selectable UI element. " + eventData);
    }
    public void OnBeginDrag(PointerEventData data)
    {
        Debug.Log("OnBeginDrag: " + data.position);
        data.pointerDrag = null;
    }
    public void OnDrag(PointerEventData data)
    {
        if (data.dragging)
        {
            timeCount += Time.deltaTime;
            if (timeCount > 1.0f)
            {
                Debug.Log("Dragging:" + data.position);
                timeCount = 0.0f;
            }
        }
    }
    public void OnEndDrag(PointerEventData data)
    {
        Debug.Log("OnEndDrag: " + data.position);
    }
 
}