using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ScoreUI class.
/// </summary>
public class ScoreUI : MonoBehaviour
{
	public Image image;
	public Text text;

	/// <summary>
	/// Update the score UI.
	/// </summary>
	/// <param name="value">Value.</param>
	public void SetScore(int value)
	{
		this.text.text = value.ToString();
		// TODO: if we want to animate the icon/play sound/etc when this score changes, this is where we'd do that
	}
}