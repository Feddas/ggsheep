using UnityEngine;
using System.Collections;

public class PushActivator : MonoBehaviour {

	Player _player;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) 
	{
		Pen pen = other.GetComponent<Pen>();
		if (pen != null)
		{
			_player.PlayerState = PlayerStates.PushPen;
		}
	}

	void OnTriggerExit(Collider other) 
	{
		Pen pen = other.GetComponent<Pen>();
		if (pen != null && _player.PlayerState == PlayerStates.PushPen)
		{
			_player.PlayerState = PlayerStates.Trample;
		}
	}
}
