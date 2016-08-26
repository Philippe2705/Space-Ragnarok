using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;


public class ScoreBoard : NetworkBehaviour
{
    public GameObject PlayerScoreDisplayer;
    public Button NewGameButton;
    public Button QuitButton;

    float scoreBoardTimer;
    bool timerActivated;
    Dictionary<string, PlayerScore> playerScores;


    void Start()
    {
        playerScores = new Dictionary<string, PlayerScore>();
        NewGameButton.onClick.AddListener(OnNewGame);
        QuitButton.onClick.AddListener(OnQuit);
        NewGameButton.gameObject.SetActive(false);
        QuitButton.gameObject.SetActive(false);
        timerActivated = false;
        scoreBoardTimer = Constants.TimeBeforeButtonsScoreBoard;
        ShowScoreBoard(false);
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
        if (timerActivated && scoreBoardTimer > 0)
        {
            scoreBoardTimer -= Time.deltaTime;
        }
        if (scoreBoardTimer < 0 && timerActivated)
        {
            timerActivated = false;
            NewGameButton.gameObject.SetActive(true);
            QuitButton.gameObject.SetActive(true);
        }
    }


    public void ShowScoreBoard(bool show)
    {
        timerActivated = true;
        SortPlayers();
        transform.parent.parent.parent.GetComponent<Canvas>().enabled = show;
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
        RpcAddHit(pseudo);
    }

    public void AddKill(string killer, string killed)
    {
        RpcAddKill(killer, killed);
    }

    public void ShowScoreBoardOnAll(bool show)
    {
        RpcShowScoreBoard(show);
    }

    public void AddPlayerOnAll(string pseudo)
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
            psd.transform.localScale = Vector3.one;
            psd.transform.SetAsLastSibling();
            psd.GetComponent<PlayerScoreDisplayer>().PlayerScore = ps;
        }
    }

    void SortPlayers()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        var l = new List<PlayerScore>();
        foreach (var ps in playerScores.Values)
        {
            l.Add(ps);
        }
        l.Sort((a, b) => b.Xp.CompareTo(a.Xp));
        foreach (var ps in l)
        {
            var psd = Instantiate(PlayerScoreDisplayer) as GameObject;
            psd.transform.SetParent(transform);
            psd.transform.localScale = Vector3.one;
            psd.transform.SetAsLastSibling();
            psd.GetComponent<PlayerScoreDisplayer>().PlayerScore = ps;
        }
    }

    public void OnNewGame()
    {
        //if already respawn (multi clicks)
        if (scoreBoardTimer > 0)
        {
            return;
        }
        Start();
        if (isServer)
        {
            foreach (var bs in FindObjectsOfType<BotShip>())
            {
                bs.Respawn();
            }
        }
        foreach (var ps in FindObjectsOfType<PlayerShip>())
        {
            if (ps.isLocalPlayer)
            {
                ps.Respawn();
            }
        }
    }

    public void OnQuit()
    {
        FindObjectOfType<Disconnect>().StartDisconnect();
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