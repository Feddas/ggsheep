using UnityEngine;
using System.Collections;

public enum ETileState
{
	dirt = 0,
	grass = 1
}

public class TileGround : MonoBehaviour 
{
	public Material[] _materials;
	public MeshRenderer[] _tileMeshes;
	public ETileState _state = ETileState.grass;
	public GameObject _dirt;
	public GameObject _grass;

	// Use this for initialization
	void Start () {
		SetState (_state);
		SetMaterial ((int)_state);
	}

	public void SetMaterial(int index)
	{
		foreach(MeshRenderer mesh in _tileMeshes)
		{
			//@@ FIXX Possible memory leak
			Material[] matCopy = mesh.materials;
			matCopy[0] = _materials[index];
			mesh.materials = matCopy;
		}
	}

	public void SetState(ETileState state)
	{
		if (state == _state) 
		{
			return;
		}

		//SetMaterial((int)state);
		
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
