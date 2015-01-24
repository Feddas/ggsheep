﻿using System.Collections;
using UnityEngine;

/// <summary>
/// Sheep class.
/// </summary>
public class Sheep : MonoBehaviour
{
	public Player lastOwner;

    /// <summary>
    /// Initialize script state.
    /// </summary>
    internal void Start()
    {

    }

    /// <summary>
    /// Update script, called once per frame.
    /// </summary>
    internal void Update()
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

		Destroy(this.gameObject);
		// TODO: spawn more sheep!
	}

	internal void OnCollisionEnter(Collision collision) 
	{
		var player = collision.transform.GetComponentInParent<Player>();
		if (player != null)
		{
			this.lastOwner = player;
		}
	}
}