using UnityEngine;
using UnityEngine.Networking;

class NetworkStaticScript : NetworkBehaviour
{
    public int Player1ShipID;
    public int Player2ShipID;
    public string Player1Pseudo;
    public string Player2Pseudo;

    [Command]
    void CmdSetPlayerConf(int shipId, string pseudo)
    {
        Debug.LogError(NetworkManager.singleton.numPlayers.ToString() + " players connected");
        if (NetworkManager.singleton.numPlayers == 1)
        {
            Player1ShipID = shipId;
            Player1Pseudo = pseudo;
        }
        else if (NetworkManager.singleton.numPlayers == 2)
        {
            Player2ShipID = shipId;
            Player2Pseudo = pseudo;
        }
    }

    void Start()
    {
        if (isClient)
        {
            var ss = FindObjectOfType<StaticScript>();
            CmdSetPlayerConf(ss.shipId, ss.pseudo);
        }
    }
}
