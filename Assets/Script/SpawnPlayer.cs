using UnityEngine;
using UnityEngine.Networking;

public class SpawnPlayer : NetworkBehaviour
{
    public int ShipId;
    public string Pseudo;
    public bool IsBot;

    public override void OnStartLocalPlayer()
    {
        CmdSpawnPlayer();
    }

    [Command]
    void CmdSpawnPlayer()
    {
        var randomPos = new Vector3(Random.Range(-60, 60), Random.Range(-30, 30), 0);
        if (IsBot)
        {
            var bot = Instantiate(ShipProperties.GetShip(ShipId).BotShipPrefab, randomPos, Quaternion.identity) as GameObject;
            NetworkServer.Spawn(bot);
            bot.GetComponent<BotShip>().Pseudo = Pseudo;
            bot.GetComponent<BotShip>().ShipId = ShipId;
        }
        else
        {
            var player = Instantiate(ShipProperties.GetShip(ShipId).PlayerShipPrefab, randomPos, Quaternion.identity) as GameObject;
            NetworkServer.ReplacePlayerForConnection(connectionToClient, player, 0);
            player.GetComponent<PlayerShip>().Pseudo = Pseudo;
            player.GetComponent<PlayerShip>().ShipId = ShipId;
        }
    }
}
