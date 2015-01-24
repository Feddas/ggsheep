using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ScoreManager class.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    /// <summary>
    /// Collection of score text fields.
    /// </summary>
	public List<ScoreboardUI> scoreboards;

    /// <summary>
    /// Each player's score for each score type.
    /// </summary>
	private Dictionary<PlayerId, Dictionary<ScoreType, int>> playerScores;

    /// <summary>
    /// This structure defines our teams.
    /// TODO: This could be configurable in a menu, or extended to include more/fewer teams with more/fewer players.
    /// </summary>
    private Dictionary<int, List<PlayerId>> teamToPlayers = new Dictionary<int, List<PlayerId>>
        {
            { 0, new List<PlayerId> { PlayerId.One, PlayerId.Two } },
            { 1, new List<PlayerId> { PlayerId.Three, PlayerId.Four } }
        };

    /// <summary>
    /// This structure gives us the team a given player is on.
    /// </summary>
    private Dictionary<PlayerId, int> playerToTeam; 

    /// <summary>
    /// Initialize script state.
    /// </summary>
    internal void Start()
    {
        // initialize player score structure
		this.playerScores = new Dictionary<PlayerId, Dictionary<ScoreType, int>>();
        foreach (PlayerId playerId in Enum.GetValues(typeof(PlayerId)))
		{
			this.playerScores.Add(playerId, new Dictionary<ScoreType, int>());
			foreach (ScoreType scoreType in Enum.GetValues(typeof(ScoreType)))
			{
				this.playerScores[playerId].Add(scoreType, 0);
			}
		}

        // initialize reverse lookup team lookup, playerToTeam
        this.playerToTeam = new Dictionary<PlayerId, int>();
        for (int teamNumber = 0; teamNumber < this.teamToPlayers.Count; teamNumber++)
        {
            foreach (var playerId in this.teamToPlayers[teamNumber])
            {
                this.playerToTeam.Add(playerId, teamNumber);
            }
        }
    }

    /// <summary>
    /// Update script, called once per frame.
    /// </summary>
    internal void Update()
    {
		// TODO: timed game rules
    }

    /// <summary>
    /// Update the player's score and calculates the new team score.
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="scoreType"></param>
	public void Score(PlayerId playerId, ScoreType scoreType)
	{
        // increment player score
		this.playerScores[playerId][scoreType]++;

        // get player's team
        var team = this.playerToTeam[playerId];

        // aggregate team score 
        var teamScore = this.TeamScore(team, scoreType);

        // get the text field for this player's team and score type and update it
		this.scoreboards[team].SetScore(scoreType, teamScore);
	}

    /// <summary>
    /// Get the team's total score in the given category.
    /// </summary>
    /// <param name="team"></param>
    /// <param name="scoreType"></param>
    /// <returns></returns>
    private int TeamScore(int team, ScoreType scoreType)
    {
        var rval = 0;
        for (var index = 0; index < this.teamToPlayers[team].Count; index++)
        {
            var playerId = this.teamToPlayers[team][index];
            rval += this.playerScores[playerId][scoreType];
        }

        return rval;
    }
}