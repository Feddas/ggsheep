using UnityEngine;
using System.Collections;

public class Respawnable : MonoBehaviour {
	private Vector3 m_respawnPoint;

	void Awake()
	{
		m_respawnPoint = transform.position;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (transform.position.y < TileManager.instance.RespawnHeight) 
		{
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
