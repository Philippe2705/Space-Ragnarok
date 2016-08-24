using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;


public class ScoreBoard : NetworkBehaviour
{
    public GameObject PlayerScoreDisplayer;

    Dictionary<string, PlayerScore> playerScores = new Dictionary<string, PlayerScore>();

    void Start()
    {
        transform.parent.GetComponent<Canvas>().enabled = false;
    }

    void Update()
    {
        /*
         * Score board
         */
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ShowScoreBoard(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            ShowScoreBoard(false);
        }
    }


    public void ShowScoreBoard(bool show)
    {
        transform.parent.GetComponent<Canvas>().enabled = show;
        foreach (var c in FindObjectsOfType<Canvas>())
        {
            if (!transform.IsChildOf(c.transform))
            {
                c.enabled = !show;
            }
        }
    }



    public void AddHit(string pseudo)
    {
        CmdAddHit(pseudo);
    }

    public void AddKill(string killer, string killed)
    {
        CmdAddKill(killer, killed);
    }

    public void ShowScoreBoardOnAll(bool show)
    {
        CmdShowScoreBoard(show);
    }

    public void AddPlayerOnAll(string pseudo)
    {
        CmdAddPlayer(pseudo);
    }

    /*
     * Commands
     */
    [Command]
    void CmdAddHit(string pseudo)
    {
        RpcAddHit(pseudo);
    }

    [Command]
    void CmdAddKill(string killer, string killed)
    {
        RpcAddKill(killer, killed);
    }

    [Command]
    void CmdShowScoreBoard(bool show)
    {
        RpcShowScoreBoard(show);
    }

    [Command]
    void CmdAddPlayer(string pseudo)
    {
        RpcAddPlayer(pseudo);
    }

    /*
     * Rpcs
     */
    [ClientRpc]
    void RpcAddHit(string pseudo)
    {
        AddPlayer(pseudo);
        PlayerScore ps;
        if (playerScores.TryGetValue(pseudo, out ps))
        {
            ps.NumberOfHits++;
            ps.Xp += Constants.XpForHit;
        }
    }

    [ClientRpc]
    void RpcAddKill(string killer, string killed)
    {
        AddPlayer(killer);
        AddPlayer(killed);
        PlayerScore ps;
        if (playerScores.TryGetValue(killer, out ps))
        {
            ps.NumberOfKill++;
            ps.Xp += Constants.XpForKill;
        }
        if (playerScores.TryGetValue(killed, out ps))
        {
            ps.KilledBy = killer;
        }
    }

    [ClientRpc]
    void RpcShowScoreBoard(bool show)
    {
        ShowScoreBoard(show);
    }

    [ClientRpc]
    void RpcAddPlayer(string pseudo)
    {
        AddPlayer(pseudo);
    }



    void AddPlayer(string pseudo)
    {
        if (!playerScores.ContainsKey(pseudo))
        {
            var ps = new PlayerScore(pseudo);
            playerScores.Add(pseudo, ps);
            var psd = Instantiate(PlayerScoreDisplayer) as GameObject;
            psd.transform.SetParent(transform);
            psd.transform.SetAsLastSibling();
            psd.GetComponent<PlayerScoreDisplayer>().PlayerScore = ps;
        }
    }

}

public class PlayerScore
{
    public string Pseudo;
    public int NumberOfHits;
    public int NumberOfKill;
    public string KilledBy;
    public int Xp;

    public PlayerScore(string pseudo)
    {
        Pseudo = pseudo;
        KilledBy = "-";
    }
}
