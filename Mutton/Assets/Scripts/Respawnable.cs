using UnityEngine;
using System.Collections;

public class Respawnable : MonoBehaviour {
	private Vector3 m_respawnPoint;

	void Awake()
	{
		m_respawnPoint = transform.position;
	}

	// Update is called once per frame
	public virtual void Update () 
	{
		if (transform.position.y < TileManager.instance.RespawnHeight) 
		{
			if (this is Sheep)
			{
				(this as Sheep).soundEffectManager.Play(SoundEffectType.KillMe);
			}

			Respawn ();
		}
	
	}

	public virtual void Respawn()
	{
		Vector3 vSpawnPos = TileManager.instance.GetSpawnPosition ();
		if (vSpawnPos == Vector3.zero)
			transform.position = m_respawnPoint;
		else
			transform.position = vSpawnPos;
	}
}
