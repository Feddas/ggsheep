using UnityEngine;
using System.Collections;

public class PlayerMgr : MonoBehaviour 
{
	Player[] _players;
	void Awake()
	{
		_players = GameObject.FindObjectsOfType<Player> ();
	}

	// Use this for initialization
	void Start () 
	{
		foreach(Player player in _players)
		{
			if( !Globals.Instance.Objective.ContainsKey(player.playerId) )
			{
				player.gameObject.SetActive(false);
			}
		}

		if( Globals.Instance.Objective.Count == 0 )
		{
			_players[0].gameObject.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
