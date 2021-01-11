  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyVariables : MonoBehaviour
{
    
	public static bool enterWaterRoom = false;
	public static int growState = 0; //0: nothing; 1: dirt in; 2: seed in; 3: bud grow; 4: bloom
	public static bool firstTimeLobbyFlag = true; // Used to tell if its the first time visiting the lobby scene
	public static bool accidentallyOpenTalisman = false;
	public static bool canOpenTalisman = true;
	public static bool haveSeenRiverTip = false;
	public static bool windExist = true;
	public static int baseDisappearCount = 0;
	public static int fireLevel = 3;
	public static bool fromGame = false;
}