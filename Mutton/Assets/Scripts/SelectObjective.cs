using System.Linq;
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
    [SerializeField]
    private string LevelToLoad = "Farm";
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
    private Dictionary<PlayerId, ScoreType> objective;
    private TeamManager teamManager;
    #endregion [ private variables ]

    #region [ Unity Events ]

    void Start()
    {
        this.objective = new Dictionary<PlayerId, ScoreType>();
        this.teamManager = FindObjectOfType<TeamManager>();
    }

    void Update()
    {
        if (counterOn)
        {
            SecondsToSelect -= Time.deltaTime;
            TimerText.text = string.Format("Seconds left {0:0}", SecondsToSelect);
            if (SecondsToSelect <= 0)
                CounterFinished();
        }

        foreach (var affix in Globals.Instance.ManageTeam.ControllerSchemes)
        {
            SetObjective(affix);
        }
    }
    #endregion [ Unity Events ]

    #region [ Change Objective ]
    private void SetObjective(string controllerAffix)
    {
        float xAxis = Input.GetAxis("Horizontal" + controllerAffix);
        float yAxis = Input.GetAxis("Vertical" + controllerAffix);
        if (xAxis != 0 || yAxis != 0)
        {
            DirectionEnum dir = getDirection(xAxis, yAxis);
            var objective = ObjectiveUiMap[dir];
            bool newPlayer, newObjective;
            TeamPlayer player = Globals.Instance.ManageTeam.ObjectiveUpdated(controllerAffix, objective, out newPlayer, out newObjective);

            // enable player icon
            if (newPlayer)
            {
                counterOn = true;
                enablePlayer(player.PlayerNumber);
            }

            // update objective
            if (newPlayer || newObjective)
            {
                audio.Stop();
                audio.pitch = (float)player.PlayerNumber / 2f;
                audio.Play();
            }

            Debug.Log("added new " + controllerAffix + " player to team " + Globals.Instance.ManageTeam.GetTeamNumber(player.PlayerNumber));
        }
    }

    private void SetObjective(PlayerId player)
    {
        float xAxis = Input.GetAxis("HorizontalGP" + (int)player);
        float yAxis = Input.GetAxis("VerticalGP" + (int)player);
        if (xAxis != 0 || yAxis != 0)
        {
            DirectionEnum dir = getDirection(xAxis, yAxis);
            var objective = ObjectiveUiMap[dir];
            bool newPlayer = this.objective.ContainsKey(player) == false;

            // enable player icon
            if (newPlayer)
            {
                counterOn = true;
                enablePlayer(player);
            }

            // update objective
            if (newPlayer || this.objective[player] != objective)
            {
                this.objective[player] = objective;

                // apply to team
                this.teamManager.GetTeam(player).objective = objective;

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
        //Debug.Log("enabled player " + player);
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

        if (this.objective.ContainsKey(teammate))
        {
            bool result = this.objective[player] == this.objective[teammate];
            return result;
        }
        else
        {
            return false;
        }
    }

    private PlayerId getTeammate(PlayerId player)
    {
        return this.teamManager.GetTeam(player).members.First(x => x != player);//.Select(x => x != player).First();
        //PlayerId teammate;
        //switch (player)
        //{
        //    case PlayerId.One:
        //        teammate = PlayerId.Three;
        //        break;
        //    case PlayerId.Two:
        //        teammate = PlayerId.Four;
        //        break;
        //    case PlayerId.Three:
        //        teammate = PlayerId.One;
        //        break;
        //    case PlayerId.Four:
        //        teammate = PlayerId.Two;
        //        break;
        //    default:
        //        throw new System.Exception("invalid playerId in isConcensus");
        //}
        //return teammate;
    }

    /// <param name="forcingPlayer">player that contains the objective that will be used for the entire team</param>
    private void forceConcensus(PlayerId forcingPlayer)
    {
        PlayerId teammate = getTeammate(forcingPlayer);
        if (this.objective.ContainsKey(teammate) && this.objective.ContainsKey(forcingPlayer))
        {
            Debug.Log(this.objective[forcingPlayer] + ":1:" + this.objective[teammate]);
            this.objective[teammate] = this.objective[forcingPlayer];
            Debug.Log(this.objective[forcingPlayer] + ":2:" + this.objective[teammate]);
            // apply to team
            this.teamManager.GetTeam(forcingPlayer).objective = this.objective[forcingPlayer];
        }
    }
    #endregion [ Concensus ]

    #region [ Counter ]
    private void CounterFinished()
    {
        string players = "";
        foreach (var team in Globals.Instance.ManageTeam.Teams)
        {
            // choose one of the highest voted options http://stackoverflow.com/questions/4179448/return-list-with-maximum-count-using-linq
            var topObjectives = team.Players.GroupBy(p => p.ObjectiveSelected); // group the same objectives to count them
            var maxVotes = topObjectives.Max(g => g.Count()); // find the highest counts
            topObjectives = topObjectives.Where(g => g.Count() == maxVotes); // keep only the objectives with the highest count
            var teamObjective = topObjectives // choose one from the list
                .Select(g => g.Key)
                .ToList()[Random.Range(0, topObjectives.Count())];

            team.Objective = teamObjective;

            players += "Team #" + team.TeamNumber + " is " + team.Objective + ". ";
        }
        Debug.Log(players);

        Application.LoadLevel(this.LevelToLoad);
    }
    #endregion [ Counter ]
}