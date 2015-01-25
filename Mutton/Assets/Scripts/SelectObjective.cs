using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SelectObjective : MonoBehaviour
{
    #region [ Unity3D Inspector ]
    [SerializeField]
    private Text TimerText;
    [SerializeField]
    private float SecondsToSelect = 30;
    [SerializeField]
    private Text[] TeamText;
    [SerializeField]
    private Image[] PlayerIcons;
    #endregion [ Unity3D Inspector ]

    #region [ private variables ]
    private Dictionary<DirectionEnum, ScoreType> ObjectiveUiMap = new Dictionary<DirectionEnum, ScoreType>()
    {
        {DirectionEnum.Up, ScoreType.Eat},
        {DirectionEnum.Down, ScoreType.Plant},
        {DirectionEnum.Right, ScoreType.Capture},
        {DirectionEnum.Left, ScoreType.Trample},
    };

    private bool counterOn;
    #endregion [ private variables ]

    #region [ Unity Events ]
    void Start() { }

    void Update()
    {
        if (counterOn)
        {
            SecondsToSelect -= Time.deltaTime;
            TimerText.text = string.Format("Seconds left {0:0}", SecondsToSelect);
            if (SecondsToSelect <= 0)
                CounterFinished();
        }

        SetObjective(PlayerId.One);
        SetObjective(PlayerId.Two);
        SetObjective(PlayerId.Three);
        SetObjective(PlayerId.Four);
    }
    #endregion [ Unity Events ]

    #region [ Change Objective ]
    private void SetObjective(PlayerId player)
    {
        float xAxis = Input.GetAxis("HorizontalGP" + (int)player);
        float yAxis = Input.GetAxis("VerticalGP" + (int)player);
        if (xAxis != 0 || yAxis != 0)
        {
            DirectionEnum dir = getDirection(xAxis, yAxis);
            var objective = ObjectiveUiMap[dir];
            bool newPlayer = Globals.Instance.Objective.ContainsKey(player) == false;
            if (newPlayer)
            {
                counterOn = true;
                enablePlayer(player);
            }
            if (newPlayer ||
                Globals.Instance.Objective[player] != objective)
            {
                Globals.Instance.Objective[player] = objective;
                updateConcensus(player);
                audio.Stop();
                audio.pitch = (float)player / 2f;
                audio.Play();
            }
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

    private void enablePlayer(PlayerId player)
    {
        Color playerColor = PlayerIcons[(int)player - 1].color;
        playerColor.a = 1f;
        PlayerIcons[(int)player - 1].color = playerColor;
    }
    #endregion [ Change Objective ]

    #region [ Concensus ]
    private void updateConcensus(PlayerId player)
    {
        int team = (int)player < 3 ? 0 : 1;

        if (isConcensus(player))
            TeamText[team].color = Color.white;
        else
            TeamText[team].color = Color.black;
    }

    private bool isConcensus(PlayerId player)
    {
        PlayerId teammate = getTeammate(player);

        if (Globals.Instance.Objective.ContainsKey(teammate))
        {
            bool result = Globals.Instance.Objective[player] == Globals.Instance.Objective[teammate];
            return result;
        }
        else
        {
            return false;
        }
    }

    private PlayerId getTeammate(PlayerId player)
    {
        PlayerId teammate;
        switch (player)
        {
            case PlayerId.One:
                teammate = PlayerId.Two;
                break;
            case PlayerId.Two:
                teammate = PlayerId.One;
                break;
            case PlayerId.Three:
                teammate = PlayerId.Four;
                break;
            case PlayerId.Four:
                teammate = PlayerId.Three;
                break;
            default:
                throw new System.Exception("invalid playerId in isConcensus");
        }
        return teammate;
    }

    /// <param name="forcingPlayer">player that contains the objective that will be used for the entire team</param>
    private void forceConcensus(PlayerId forcingPlayer)
    {
        PlayerId teammate = getTeammate(forcingPlayer);
        if (Globals.Instance.Objective.ContainsKey(teammate))
        {
            Debug.Log(Globals.Instance.Objective[forcingPlayer] + ":1:" + Globals.Instance.Objective[teammate]);
            Globals.Instance.Objective[teammate] = Globals.Instance.Objective[forcingPlayer];
            Debug.Log(Globals.Instance.Objective[forcingPlayer] + ":2:" + Globals.Instance.Objective[teammate]);

        }
    }
    #endregion [ Concensus ]

    #region [ Counter ]
    private void CounterFinished()
    {
        forceConcensus((PlayerId)Random.Range(1, 2));
        forceConcensus((PlayerId)Random.Range(3, 4));
        //TODO: if players on team don't have same objective, pick one of them randomly to be the obj for both
        string players = "";
        foreach (PlayerId player in System.Enum.GetValues(typeof(PlayerId)))
        {
            if (Globals.Instance.Objective.ContainsKey(player))
            {
                players += player + " is " + Globals.Instance.Objective[player] + "   ";
            }
        }
        Debug.Log(players);

        Application.LoadLevel("Farm");
    }
    #endregion [ Counter ]
}