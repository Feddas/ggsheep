using UnityEngine;

/// <summary>
/// Grass class.
/// </summary>
public class Grass : TileGround
{
	internal void OnTriggerEnter(Collider other) 
	{
		Debug.Log ("TRIGGER ENTER");
		var player = other.GetComponent<Player>();
		if (player != null)
		{
			player.Trample();
		}

		//Set
		SetState(ETileState.dirt);


		// TODO: replace this with a trampled grass prefab
		//Destroy(this.gameObject);
	}
}