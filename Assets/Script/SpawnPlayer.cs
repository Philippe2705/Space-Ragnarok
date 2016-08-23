using UnityEngine;
using UnityEngine.Networking;

public class SpawnPlayer : NetworkBehaviour
{
    public int ShipId;
    public string Pseudo;

    public override void OnStartLocalPlayer()
    {
        CmdSpawnPlayer();
    }

    [Command]
    void CmdSpawnPlayer()
    {
        var player = Instantiate(ShipProperties.GetShip(ShipId).PlayerShipPrefab, new Vector3(Random.Range(-60, 60), Random.Range(-30, 30), 0), Quaternion.identity) as GameObject;
        NetworkServer.ReplacePlayerForConnection(connectionToClient, player, 0);
        player.GetComponent<PlayerShip>().Pseudo = Pseudo;
        player.GetComponent<PlayerShip>().ShipId = ShipId;
    }
}
