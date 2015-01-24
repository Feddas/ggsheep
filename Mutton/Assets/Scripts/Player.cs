using System.Collections;
using UnityEngine;

/// <summary>
/// Player class.
/// </summary>
public class Player : MonoBehaviour
{
	public PlayerId playerId;

	private ScoreManager scoreManager;

    /// <summary>
    /// Initialize script state.
    /// </summary>
    internal void Start()
    {
		this.scoreManager = FindObjectOfType<ScoreManager>();
    }

    /// <summary>
    /// Update script, called once per frame.
    /// </summary>
    internal void Update()
    {

    }

	public void Trample()
	{
		this.scoreManager.Score(this.playerId, ScoreType.Trample);
	}
}