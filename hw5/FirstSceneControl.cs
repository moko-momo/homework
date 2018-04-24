﻿using System;  
using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  

public class FirstSceneControl : MonoBehaviour, ISceneControl, IUserAction {  

	public ActionMode mode { get; set; }  

	public IActionManager actionManager { get; set; }  

	public ScoreRecorder scoreRecorder { get; set; }  

	public Queue<GameObject> diskQueue = new Queue<GameObject>();  

	private int diskNumber;  

	private int currentRound = -1;  


	public int round = 3;  

	private float time = 0;  
 
	private GameState gameState = GameState.START;  

	void Awake () {  
		Director director = Director.getInstance();  
		director.currentSceneControl = this;  
		diskNumber = 10;  
		this.gameObject.AddComponent<ScoreRecorder>();  
		this.gameObject.AddComponent<DiskFactory>();  
		this.gameObject.AddComponent<CCFlyActionFactory>();  
		mode = ActionMode.NOTSET;  
		scoreRecorder = Singleton<ScoreRecorder>.Instance;  
		director.currentSceneControl.LoadResources();  
	}  

	private void Update()  
	{  
		 
		if (mode != ActionMode.NOTSET && actionManager != null)  
		{  
			if (actionManager.getDiskNumber() == 0 && gameState == GameState.RUNNING)  
			{  
				gameState = GameState.ROUND_FINISH;  

			}  

			if (actionManager.getDiskNumber() == 0 && gameState == GameState.ROUND_START)  
			{  
				currentRound = (currentRound + 1) % round;  
				NextRound();  
				actionManager.setDiskNumber(10);  
				gameState = GameState.RUNNING;  
			}  

			if (time > 1)  
			{  
				ThrowDisk();  
				time = 0;  
			}  
			else  
			{  
				time += Time.deltaTime;  
			}  
		}    



	}  

	private void NextRound()  
	{  
		DiskFactory df = Singleton<DiskFactory>.Instance;  
		for (int i = 0; i < diskNumber; i++)  
		{  
			diskQueue.Enqueue(df.GetDisk(currentRound, mode));  
		}  

		actionManager.StartThrow(diskQueue);  

	}  

	void ThrowDisk()  
	{  
		if (diskQueue.Count != 0)  
		{  
			GameObject disk = diskQueue.Dequeue();  


			Vector3 position = new Vector3(0, 0, 0);  
			float y = UnityEngine.Random.Range(0f, 4f);  
			position = new Vector3(-disk.GetComponent<DiskData>().direction.x * 7, y, 0);  
			disk.transform.position = position;  

			disk.SetActive(true);  
		}  

	}  

	public void LoadResources()  
	{  
		  
		GameObject greensward = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("greensward"));  
	}  


	public void GameOver()  
	{  
		GUI.color = Color.red;  
		GUI.Label(new Rect(700, 300, 400, 400), "GAMEOVER");  

	}  

	public int GetScore()  
	{  
		return scoreRecorder.score;  
	}  

	public GameState getGameState()  
	{  
		return gameState;  
	}  

	public void setGameState(GameState gs)  
	{  
		gameState = gs;  
	}  

	public void hit(Vector3 pos)  
	{  
		Ray ray = Camera.main.ScreenPointToRay(pos);  

		RaycastHit[] hits;  
		hits = Physics.RaycastAll(ray);  
		for (int i = 0; i < hits.Length; i++)  
		{  
			RaycastHit hit = hits[i];  

			if (hit.collider.gameObject.GetComponent<DiskData>() != null)  
			{  
				scoreRecorder.Record(hit.collider.gameObject);  


				hit.collider.gameObject.transform.position = new Vector3(0, -5, 0);  
			}  

		}  
	}  

	public ActionMode getMode()  
	{  
		return mode;  
	}  

	public void setMode(ActionMode m)  
	{  

		if (m == ActionMode.NORMAL)  
		{  
			this.gameObject.AddComponent<CCActionManager>();  
		}  
		else  
		{  
			this.gameObject.AddComponent<PhysicActionManager>();  
		}  

		mode = m;  
	}  
}  
