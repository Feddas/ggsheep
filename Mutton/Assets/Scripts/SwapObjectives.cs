using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class SwapObjectives : MonoBehaviour
{
    [SerializeField]
    private float SecondsToActivate = 5;
    [SerializeField]
    private Image RadialSprite;

    private float secondsLeftToActivate;
    private Player firstButtonPusher;

    #region [ Unity Events ]
    void Start()
    {
        secondsLeftToActivate = SecondsToActivate;
    }

    void Update()
    {
        if (firstButtonPusher != null)
        {
            secondsLeftToActivate -= Time.deltaTime;
            if (secondsLeftToActivate <= 0)
                swap();
            else
                RadialSprite.fillAmount = 1 - secondsLeftToActivate / this.SecondsToActivate;
        }
    }

    internal void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && firstButtonPusher == null)
        {
            firstButtonPusher = player;
        }
    }

    internal void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && firstButtonPusher == player)
        {
            reset();
        }
    }
    #endregion [ Unity Events ]

    private void swap()
    {
        reset();
        Debug.Log("Swapping! (if there is more than 1 team)");
        // TODO: amazing swap animation
        
        // determine which players are active
        IList<PlayerId> activePlayers = new List<PlayerId>();
        foreach (PlayerId player in System.Enum.GetValues(typeof(PlayerId)))
        {
            if (Globals.Instance.Objective.ContainsKey(player))
            {
                activePlayers.Add(player);
            }
        }

        // determine the teams
        var team1 = activePlayers.Where(player => (int)player < 2);
        var team2 = activePlayers.Where(player => (int)player > 1);

        if (team1.Count() == 0 || team2.Count() == 0)
            return; // don't swap, nobody on the other team to swap with

        // get a player from each team
        PlayerId team1player = team1.First();
        PlayerId team2player = team2.First();

        // get the teams objective
        ScoreType team1Objective = Globals.Instance.Objective[team1player];
        ScoreType team2Objective = Globals.Instance.Objective[team2player];

        // after all that, it's finally time to swap!
        Debug.Log("before swap Team1 obj: " + team1Objective + " Team2 obj: " + team2Objective);
        foreach (var player in activePlayers)
        {
            if ((int)player < 2)
                Globals.Instance.Objective[player] = team2Objective;
            else
                Globals.Instance.Objective[player] = team1Objective;
        }
        Debug.Log("after swap Team1 obj: " + Globals.Instance.Objective[team1player] + " Team2 obj: " + Globals.Instance.Objective[team2player]);
    }

    private void reset()
    {
        firstButtonPusher = null;
        RadialSprite.fillAmount = 0f;
        secondsLeftToActivate = this.SecondsToActivate;
    }
}
