﻿using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public enum PlayerStates
{
	Trample,
	PushPen,
	PushSheep,
};

/// <summary>
/// Player class.
/// </summary>
public class Player : Respawnable
{
	public PlayerId playerId;
	public PlayerStates playerState = PlayerStates.Trample;

	public Transform playerHead;
	public float minPlayerHeadSize = 1.0f;
	public float maxPlayerHeadSize = 2.0f;
	
	//public List<PlayerStates> _stateList; //@@ Add list of states to fix potential bugs with sheep shed push

	public PlayerStates PlayerState 
	{
		get {
			return playerState;
		}
		set {
			playerState = value;
		}
	}

	float GetHeadScale()
	{
		float total = (float)ScoreManager.instance.GetTotalScore ();

		float scale = total < float.Epsilon ? 0.0f : ((float)ScoreManager.instance.GetScore (playerId) / total);
		return minPlayerHeadSize + (maxPlayerHeadSize - minPlayerHeadSize) * scale;
	}

	public override void Update () 
	{
		base.Update ();

		float scale = GetHeadScale();

		playerHead.transform.localScale = new Vector3(scale,scale,scale);
	}


	private ScoreManager scoreManager;


    /// <summary>
    /// Initialize script state.
    /// </summary>
    internal void Start()
    {
		this.scoreManager = FindObjectOfType<ScoreManager>();
    }
	

	public void Score(ScoreType scoreType)
	{
		this.scoreManager.Score(this.playerId, scoreType);
	}
}