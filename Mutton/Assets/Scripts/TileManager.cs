using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour {

	public GameObject _levelGenerator;
	private bool _continuousGeneration;
	private float m_timeBetweenGeneration = 3.0f;

	public bool ContinuousGeneration {
		get {
			return _continuousGeneration;
		}
		set 
		{
			_continuousGeneration = value;
			if(_continuousGeneration )
			{
				InvokeRepeating( "PopulateMapRaycast", 0.0f, m_timeBetweenGeneration);
			}
			else
			{
				CancelInvoke("PopulateMapRaycast");
			}
		}
	}

	// Use this for initialization
	void Start () 
	{
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
		if (_continuousGeneration) 
		{
			PopulateMapRaycast();
		}
	}

	void ClearMap()
	{
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

				int mask = (1 << 9);
				if( Physics.Raycast(origin,new Vector3(0.0f,-1.0f,0.0f),mask) )
				{
					m_tiles[j,i] = (TileGround)Instantiate(_tileGround,origin,Quaternion.identity);
					m_tiles[j,i].transform.parent = gameObject.transform;
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
}
