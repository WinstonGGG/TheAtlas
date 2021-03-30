﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ObItem : MonoBehaviour
{
	private GameObject goManager;
    private GOManagement go;
    
    public ItemState[] itemStateTotalNum;
    [System.Serializable]
    public struct ItemState
    {
    	public enum StateType {image, animate};
    	public StateType State;
    	public Image image;
    	public AnimationClip animate;
    }
    [SerializeField]
	private int CurrentState = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}