
using System.Collections;
using UnityEngine;

/// <summary>
/// Grass class.
/// </summary>
public class Grass : MonoBehaviour
{
	internal void OnTriggerEnter(Collider other) 
	{
		var player = other.GetComponent<Player>();
		if (player != null)
		{
			player.Trample();
		}

		// TODO: replace this with a trampled grass prefab
		Destroy(this.gameObject);
	}
}