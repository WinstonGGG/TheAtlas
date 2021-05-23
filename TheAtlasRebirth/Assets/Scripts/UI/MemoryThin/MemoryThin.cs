using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryThin : MonoBehaviour
{
    [HideInInspector]
    private int length; //存在的记忆数量

    public struct ItemInfo
    {
        public enum MemorySort { image, text, imageText };
        public MemorySort memorySort;
        public Sprite memoryIcon;
        public string memoryText;
        public string memoryTime;
    }

    public ItemInfo[] MemoryThinInfos;

    // Start is called before the first frame update
    void Start()
    {
        length = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
