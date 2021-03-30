using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InSceneItem : MonoBehaviour
{
    //				可收集&可互动,可收集&不可互动,不可收集&可互动,不可收集&不可互动,
    public enum ItemTypes {CollNIn, CollNUnin, UncollNIn, UncollNUnin };
    public ItemTypes itemType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
