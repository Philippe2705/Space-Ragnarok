using UnityEngine;
using System.Collections;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

public class LobbyHookPseudoAndShip : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        gamePlayer.GetComponent<SpawnPlayer>().ShipId = lobbyPlayer.GetComponent<LobbyPlayer>().Ship;
        gamePlayer.GetComponent<SpawnPlayer>().Pseudo = lobbyPlayer.GetComponent<LobbyPlayer>().Pseudo;
        gamePlayer.GetComponent<SpawnPlayer>().IsBot = lobbyPlayer.GetComponent<LobbyPlayer>().IsBot;
    }
}
