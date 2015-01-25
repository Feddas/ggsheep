using UnityEngine;
using System.Collections;
using System.Linq;

public class PlayerMgr : MonoBehaviour 
{
	Player[] _players;
	void Awake()
	{
		_players = GameObject.FindObjectsOfType<Player> ();
		_players = _players.OrderBy (player => player.name).ToArray();
	}

	// Use this for initialization
	void Start ()
	{
	    var teamManager = FindObjectOfType<TeamManager>();
		foreach(Player player in _players)
		{
			if( !teamManager.HasTeam(player.playerId) )
			{
				player.gameObject.SetActive(false);
			}
		}

		if( teamManager.teams.Count == 0 )
		{
			_players[0].gameObject.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
