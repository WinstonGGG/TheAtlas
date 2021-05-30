using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryBook : MonoBehaviour
{
    private int length;  //存在的记忆数量

    private int memoryPage;  //记忆的页数

    [System.Serializable]
    public struct ItemInfo
    {
        public enum MemorySort { image, text, imageText };
        public MemorySort memorySort;  //记忆种类
        public Sprite memoryIcon;  //
        public string memoryText;
        public string memoryTime;
    }

    public ItemInfo[] memoryBookInfos;

    // Start is called before the first frame update
    void Start()
    {
        length = 0;
        length = memoryBookInfos.Length;
        memoryPage = (int)Math.Ceiling((double)length / 3);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
