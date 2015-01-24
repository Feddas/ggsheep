using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectObjective : MonoBehaviour
{
    System.Collections.Generic.Dictionary<DirectionEnum, ScoreType> Objectives = new Dictionary<DirectionEnum, ScoreType>()
    {
        {DirectionEnum.Up, ScoreType.Eat},
        {DirectionEnum.Down, ScoreType.Plant},
        {DirectionEnum.Right, ScoreType.Capture},
        {DirectionEnum.Left, ScoreType.Trample},
    };

    private bool startCounter;

    void Start()
    {
        Debug.Log("SecretObjective");
    }

    // Update is called once per frame
    void Update()
    {
        if (startCounter) ; // start counter

        SetObjective(PlayerId.One);
    }

    private void SetObjective(PlayerId player)
    {
        float xAxis = Input.GetAxisRaw("HorizontalGP" + (int)player);
        float yAxis = Input.GetAxisRaw("VerticalGP" + (int)player);
        if (xAxis != 0 && yAxis != 0)
        {
            startCounter = true;
            DirectionEnum dir = getDirection(xAxis, yAxis);

            Globals.Instance.Objective[player] = Objectives[dir];
            Debug.Log(xAxis + ", " + yAxis
                + dir + Globals.Instance.Objective[player]);
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
        Application.LoadLevel("Farm");
    }
}