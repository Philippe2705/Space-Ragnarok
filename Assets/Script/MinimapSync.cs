using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MinimapSync : MonoBehaviour
{
    public RectTransform playerIcon;
    public RectTransform enemyIcon;
    Transform player;
    Transform enemy;


    public void SearchForPlayers()
    {
        foreach (var playerShip in FindObjectsOfType<PlayerShip>())
        {
            if (playerShip.isLocalPlayer)
            {
                player = playerShip.transform;
                playerIcon.GetComponent<Image>().sprite = ShipProperties.GetShip(playerShip.ShipId).ShipSprite;
            }
            else
            {
                enemy = playerShip.transform;
                enemyIcon.GetComponent<Image>().sprite = ShipProperties.GetShip(playerShip.ShipId).ShipSprite;
            }
        }
        if (enemy == null)
        {
            enemy = FindObjectOfType<BotShip>().transform;
            enemyIcon.GetComponent<Image>().sprite = ShipProperties.GetShip(-1).ShipSprite;
        }
    }


    void Update()
    {
        if (player != null)
        {
            playerIcon.localPosition = player.position * 2;
            playerIcon.localRotation = player.rotation;
        }
        if (enemy != null)
        {
            enemyIcon.localPosition = enemy.position * 2;
            enemyIcon.localRotation = enemy.rotation;
        }
    }
}
