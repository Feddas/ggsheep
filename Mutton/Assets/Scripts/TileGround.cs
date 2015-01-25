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

		SetMaterial((int)state);

		_state = state;
	}
}
