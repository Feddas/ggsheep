using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour {

	static public TileManager instance;

	public GameObject _levelGenerator;
	public bool _continuousGeneration;
	private float m_timeBetweenGeneration = 3.0f;
	public Vector3 m_playerOffset = new Vector3(0.0f,10.0f,0.0f);

	public float _respawnHeight = -5.0f;

	public float RespawnHeight {
		get {
			return _respawnHeight;
		}
		set {
			_respawnHeight = value;
		}
	}

	private bool _runningGeneration = false;

	public bool RunningGeneration {
		get {
			return _runningGeneration;
		}
		set 
		{
			if(_runningGeneration != value )
			{
				_runningGeneration = value;
				if(_runningGeneration )
				{
					InvokeRepeating( "PopulateMapRaycast", 0.0f, m_timeBetweenGeneration);
				}
				else
				{
					CancelInvoke("PopulateMapRaycast");
				}
			}
		}
	}

	// Use this for initialization
	void Start () 
	{
		instance = this;

		if( _levelGenerator != null )
		{
			PopulateMapRaycast();
		}
		else
		{
			PopulateMap();
		}
	}


	
	// Update is called once per frame
	void Update () 
	{
		RunningGeneration = _continuousGeneration;
	}

	void ClearMap()
	{
		_tileCount = 0;

		if( m_tiles != null )
		{
			for (int j=0; j<_height; ++j) 
			{
				for (int i=0; i<_width; ++i) 
				{
					Destroy(m_tiles[j,i]);
				}
			}
		}
	}

	public Vector3 GetSpawnPosition()
	{
		int spawnIndex = Random.Range (0, _tileCount-1);
		int count = 0;

		for (int j=0; j<_height; ++j) 
		{
			for (int i=0; i<_width; ++i) 
			{
				if( m_tiles[j,i] != null && count++ == spawnIndex)
				{
					return m_tiles[j,i].transform.position + m_playerOffset;
				}
			}
		}
		return Vector3.zero;
	}

	void PopulateMapRaycast()
	{
		//Destory any previous map
		ClearMap ();

		//Calculate the start tile position
		Vector3 vStart = new Vector3 (-(float)_width * 0.5f, 0.0f, -(float)_height * 0.5f);
		
		
		m_tiles = new TileGround[_height,_width];
		for (int j=0; j<_height; ++j) 
		{
			for (int i=0; i<_width; ++i) 
			{
				Vector3 origin = vStart + new Vector3((float)i,0.0f,(float)j);

				int mask = (1 << 10);
				if( Physics.Raycast(origin,new Vector3(0.0f,-1.0f,0.0f),1000.0f,mask) )
				{
					m_tiles[j,i] = (TileGround)Instantiate(_tileGround,origin,Quaternion.identity);
					m_tiles[j,i].transform.parent = gameObject.transform;
					_tileCount++;
				}
			}
		}

	}

	void PopulateMap()
	{
		//Calculate the start tile position
		Vector3 vStart = new Vector3 (-(float)_width * 0.5f, 0.0f, -(float)_height * 0.5f);


		m_tiles = new TileGround[_height,_width];
		for (int j=0; j<_height; ++j) 
		{
			for (int i=0; i<_width; ++i) 
			{
				m_tiles[j,i] = (TileGround)Instantiate(_tileGround,vStart + new Vector3((float)i,0.0f,(float)j),Quaternion.identity);
				m_tiles[j,i].transform.parent = gameObject.transform;
			}
		}
	}

	public TileGround _tileGround; 

	public TileGround[,] m_tiles;

	public int _width = 30;
	public int _height = 15;

	private int _tileCount = 0;
}
