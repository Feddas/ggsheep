using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ScoreManager class.
/// </summary>
public class ScoreManager : MonoBehaviour
{
	public int tramplePoints = 1;
	public int eatPoints = 2;
	public int capturePoints = 25;
	public int plantPoints = 2;

	public float countdownTime = 5f;
	public float gameTime = 90f;
	private bool startTimer = false;

	/// <summary>
    /// Collection of score text fields.
    /// </summary>
	public List<ScoreboardUI> scoreboards;

	public Text timer;

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
        //Test persistance of Globals.cs from menu scene
        //Debug.Log("Globals instance = " + Globals.Instance.Objective[PlayerId.One] + Globals.Instance.Objective[PlayerId.Two]);

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

		// enable only relevant scoreboards
		for (int teamNumber = 0; teamNumber < this.scoreboards.Count; teamNumber++)
		{
			this.scoreboards[teamNumber].gameObject.SetActive(teamNumber < this.teamToPlayers.Count);
		}

		this.StartCoroutine(this.WaitAndInvoke(1f, () => this.StartTimer()));
    }

    /// <summary>
    /// Update script, called once per frame.
    /// </summary>
    internal void Update()
    {
		if (!this.startTimer)
		{
			return;
		}

		if (this.countdownTime > 0f)
		{
			this.countdownTime -= Time.deltaTime;
			if (this.countdownTime < 0f)
			{
				this.countdownTime = 0f;
			}
		} 
		else
		{
			if (this.gameTime > 0f)
			{
				this.gameTime -= Time.deltaTime;
				if (this.gameTime < 0f)
				{
					this.gameTime = 0f;
				}
			}
		}

		float displayTime = this.gameTime;
		if (this.countdownTime > 0f)
		{
			displayTime = Mathf.Floor(this.countdownTime) + 1;
		}

		this.timer.text = string.Format("{0:0}", this.countdownTime > 0f ? this.countdownTime : this.gameTime);
    }

    /// <summary>
    /// Update the player's score and calculates the new team score.
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="scoreType"></param>
	public void Score(PlayerId playerId, ScoreType scoreType)
	{
        // increment player score
		this.playerScores[playerId][scoreType] += this.ScoreModifier(scoreType);

        // get player's team
        var team = this.playerToTeam[playerId];

        // aggregate team score 
        var teamScore = this.TeamScore(team, scoreType);

        // get the text field for this player's team and score type and update it
		if (team < this.scoreboards.Count && this.scoreboards[team] != null)
		{
			this.scoreboards[team].SetScore(scoreType, teamScore);
		}
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

	private int ScoreModifier(ScoreType scoreType)
	{
		switch (scoreType)
		{
		case ScoreType.Trample:
			return this.tramplePoints;
		case ScoreType.Eat:
			return this.eatPoints;
		case ScoreType.Capture:
			return this.capturePoints;
		case ScoreType.Plant:
			return this.plantPoints;
		}

		throw new InvalidEnumArgumentException("No score modifier defined for ScoreType " + scoreType);
	}

	private void StartTimer()
	{
		Debug.Log("Start Timer");
		this.startTimer = true;
	}

	internal IEnumerator WaitAndInvoke(float delay, Action function)
	{
		yield return new WaitForSeconds(delay);
		function();
	}
}