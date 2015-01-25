using UnityEngine.UI;
using System.Collections;
using UnityEngine;

/// <summary>
/// Popup class.
/// </summary>
public class Popup : MonoBehaviour
{
	private Text text;

    /// <summary>
    /// Initialize script state.
    /// </summary>
    internal void Start()
    {
		this.text = this.GetComponent<Text>();
    }

	public void Show(string message)
	{
		this.animation.Stop();
		this.text.text = message;
		this.animation.Rewind();
		this.animation.Play();
	}
}