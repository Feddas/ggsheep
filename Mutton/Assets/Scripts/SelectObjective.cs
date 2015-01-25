using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SelectObjective : MonoBehaviour
{
    [SerializeField]
    private Text TimerText;
    [SerializeField]
    private float SecondsToSelect = 30;

    Dictionary<DirectionEnum, ScoreType> Objectives = new Dictionary<DirectionEnum, ScoreType>()
    {
        {DirectionEnum.Up, ScoreType.Eat},
        {DirectionEnum.Down, ScoreType.Plant},
        {DirectionEnum.Right, ScoreType.Capture},
        {DirectionEnum.Left, ScoreType.Trample},
    };

    private bool counterOn;

    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (counterOn)
        {
            SecondsToSelect -= Time.deltaTime; // start counter
            TimerText.text = string.Format("Seconds left {0:0}", SecondsToSelect);
            if (SecondsToSelect <= 0)
                CounterFinished();
        }

        SetObjective(PlayerId.One);
        SetObjective(PlayerId.Two);
        SetObjective(PlayerId.Three);
        SetObjective(PlayerId.Four);
    }

    private void SetObjective(PlayerId player)
    {
        float xAxis = Input.GetAxisRaw("HorizontalGP" + (int)player);
        float yAxis = Input.GetAxisRaw("VerticalGP" + (int)player);
        if (xAxis != 0 && yAxis != 0)
        {
            if (Globals.Instance.Objective.ContainsKey(player) == false)
            {
                audio.Stop();
                audio.pitch = (float)player / 2;
                audio.Play();
            }

            counterOn = true;
            DirectionEnum dir = getDirection(xAxis, yAxis);

            Globals.Instance.Objective[player] = Objectives[dir];
            // Debug.Log(xAxis + ", " + yAxis
            //    + dir + " " + player + ":" + Globals.Instance.Objective[player]);
        }
    }

    private DirectionEnum getDirection(float horizontal, float vertical)
    {
        if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
        {
            if (horizontal > 0)
                return DirectionEnum.Right;
            else
                return DirectionEnum.Left;
        }
        else if (vertical != 0)
        {
            if (vertical > 0)
                return DirectionEnum.Up;
            else
                return DirectionEnum.Down;
        }
        else
            return DirectionEnum.None;
    }

    private void CounterFinished()
    {
        string players = "";
        foreach (PlayerId player in System.Enum.GetValues(typeof(PlayerId)))
        {
            if (Globals.Instance.Objective.ContainsKey(player))
            {
                players += player + " is " + Globals.Instance.Objective[player] + System.Environment.NewLine;
            }
        }
        Debug.Log(players);

        Application.LoadLevel("Farm");
    }
}