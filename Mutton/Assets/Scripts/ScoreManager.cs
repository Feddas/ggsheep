
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ScoreManager class.
/// </summary>
public class ScoreManager : MonoBehaviour
{
	public Text teamATrampleScore;

	private Dictionary<PlayerId, Dictionary<ScoreType, int>> playerScores;

    /// <summary>
    /// Initialize script state.
    /// </summary>
    internal void Start()
    {
		this.playerScores = new Dictionary<PlayerId, Dictionary<ScoreType, int>>();
		foreach (PlayerId playerId in Enum.GetValues(typeof(PlayerId)))
		{
			this.playerScores.Add(playerId, new Dictionary<ScoreType, int>());
			foreach (ScoreType scoreType in Enum.GetValues(typeof(ScoreType)))
			{
				this.playerScores[playerId].Add(scoreType, 0);
			}
		}
    }

    /// <summary>
    /// Update script, called once per frame.
    /// </summary>
    internal void Update()
    {

    }

	public void Score(PlayerId playerId, ScoreType scoreType)
	{
		this.playerScores[playerId][scoreType]++;

		if (scoreType == ScoreType.Trample)
		{
			this.teamATrampleScore.text = this.playerScores[playerId][scoreType].ToString();
		}
	}
}