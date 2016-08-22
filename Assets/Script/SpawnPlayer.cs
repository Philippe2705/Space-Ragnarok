using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SpawnPlayer : NetworkBehaviour
{
    void Start()
    {
        if (isLocalPlayer)
        {
            //CmdSpawnPlayer(FindObjectOfType<StaticScript>().shipId);
        }
    }

    [Command]
    void CmdSpawnPlayer(int shipId)
    {
        var player = Instantiate(ShipProperties.GetShip(shipId).PlayerShipPrefab, new Vector3(Random.Range(-60, 60), Random.Range(-30, 30), 0), Quaternion.identity) as GameObject;
        NetworkServer.ReplacePlayerForConnection(NetworkServer.connections[NetworkServer.connections.Count - 1], player, 0);
    }
}
