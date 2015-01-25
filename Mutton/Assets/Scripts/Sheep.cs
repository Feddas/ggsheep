using System.Collections;
using UnityEngine;

/// <summary>
/// Sheep class.
/// </summary>
public class Sheep : Respawnable
{
	public Player lastOwner;

    /// <summary>
    /// Initialize script state.
    /// </summary>
    internal void Start()
    {

    }

	public void Eat()
	{
		if (this.lastOwner != null)
		{
			this.lastOwner.Score(ScoreType.Eat);
		}
	}

	public void Capture()
	{
		if (this.lastOwner != null)
		{
			this.lastOwner.Score(ScoreType.Capture);
		}

		Respawn ();
	}

	internal void OnCollisionEnter(Collision collision) 
	{
		var player = collision.transform.GetComponentInParent<Player>();
		if (player != null)
		{
            this.lastOwner = player;
            audio.Stop();
            audio.pitch = (float)player.playerId / 2;
            audio.Play();
		}
	}
}