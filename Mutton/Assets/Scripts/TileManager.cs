using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		PopulateMap ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
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
