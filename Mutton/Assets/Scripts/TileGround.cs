﻿using UnityEngine;
using System.Collections;

public enum ETileState
{
	dirt,
	grass
}

public class TileGround : MonoBehaviour 
{

	public GameObject _dirt;
	public GameObject _grass;
	public ETileState _state = ETileState.grass;

	// Use this for initialization
	void Start () {
		SetState (_state);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetState(ETileState state)
	{
		if (state == _state) 
		{
			return;
		}

		switch(state)
		{
			case ETileState.dirt:
			{
				_dirt.SetActive(true);
				_grass.SetActive(false);
				break;
			}
			case ETileState.grass:
			{
				_dirt.SetActive(false);
				_grass.SetActive(true);
				break;
			}
		}

		_state = state;
	}
}
