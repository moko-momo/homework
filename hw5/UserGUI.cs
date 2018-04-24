﻿using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  

public class UserGUI : MonoBehaviour  
{  
	private IUserAction action;  
	bool isFirst = true;  
	void Start () {  
		action = Director.getInstance().currentSceneControl as IUserAction;  

	}  

	private void OnGUI()  
	{  
		if (action.getMode() == ActionMode.NOTSET)  
		{  
			if (GUI.Button(new Rect(800, 100, 90, 90), "normal"))  
			{  
				action.setMode(ActionMode.NORMAL);  
			}  
			if (GUI.Button(new Rect(500, 100, 90, 90), "physic"))  
			{  
				action.setMode(ActionMode.PHYSIC);  
			}  
		}  
		else  
		{  
			if (Input.GetButtonDown("Fire1"))  
			{  

				Vector3 pos = Input.mousePosition;  
				action.hit(pos);  

			}  

			GUI.Label(new Rect(1000, 0, 400, 400), action.GetScore().ToString());  

			if (isFirst && GUI.Button(new Rect(600, 100, 90, 90), "Start"))  
			{  
				isFirst = false;  
				action.setGameState(GameState.ROUND_START);  
			}  

			if (!isFirst && action.getGameState() == GameState.ROUND_FINISH && GUI.Button(new Rect(700, 100, 90, 90), "Next Round"))  
			{  
				action.setGameState(GameState.ROUND_START);  
			}  
		}  
	}  


}  
