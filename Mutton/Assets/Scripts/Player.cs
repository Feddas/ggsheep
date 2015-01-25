using System.Collections;
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