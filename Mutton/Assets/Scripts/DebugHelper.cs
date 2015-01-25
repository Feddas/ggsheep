using System.Collections;
using UnityEngine;

/// <summary>
/// DebugHelper class.
/// </summary>
public class DebugHelper : MonoBehaviour
{
    /// <summary>
    /// Initialize script state.
    /// </summary>
    internal void Start()
    {
		if (Globals.Instance.Objective.Count < 1)
		{
			foreach (PlayerId player in System.Enum.GetValues(typeof(PlayerId)))
			{
				Globals.Instance.Objective[player] = (ScoreType)Random.Range(0, 4);
			}
		}
    }
}