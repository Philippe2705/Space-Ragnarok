using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MinimapSync : MonoBehaviour
{
    public RectTransform playerIcon;

    Transform parent;
    Ship player;
    List<Ship> enemies;
    List<RectTransform> enemiesIcons = new List<RectTransform>();


    public void SearchForPlayers()
    {
        parent = playerIcon.parent;
        enemies = new List<Ship>();

        foreach (var playerShip in FindObjectsOfType<PlayerShip>())
        {
            if (playerShip.isLocalPlayer)
            {
                player = playerShip;
                playerIcon.GetComponent<Image>().sprite = ShipProperties.GetShip(playerShip.ShipId).ShipSprite;
            }
            else
            {
                enemies.Add(playerShip);
            }
        }
        foreach (var botShip in FindObjectsOfType<BotShip>())
        {
            enemies.Add(botShip);
        }
        while (enemiesIcons.Count > enemies.Count)
        {
            Destroy(enemiesIcons[0].gameObject);
            enemiesIcons.RemoveAt(0);
        }
        while (enemiesIcons.Count < enemies.Count)
        {
            var go = Instantiate(playerIcon);
            go.transform.SetParent(parent);
            go.GetComponent<Image>().color = Color.red;
            enemiesIcons.Add(go.GetComponent<RectTransform>());
        }
        for (var i = 0; i < enemies.Count; i++)
        {
            var enemy = enemies[i];
            var enemyIcon = enemiesIcons[i];
            enemyIcon.GetComponent<Image>().sprite = ShipProperties.GetShip(enemy.ShipId).ShipSprite;
        }
    }


    void Update()
    {
        if (player != null)
        {
            playerIcon.localPosition = player.transform.position * 2;
            playerIcon.localRotation = player.transform.rotation;
            if (player.IsDead)
            {
                playerIcon.GetComponent<Image>().color = Color.gray;
            }
        }
        if (enemies != null)
        {
            for (var i = 0; i < enemies.Count; i++)
            {
                var enemy = enemies[i];
                var enemyIcon = enemiesIcons[i];
                if (enemy.IsDead)
                {
                    enemyIcon.GetComponent<Image>().color = Color.gray;
                }
                enemyIcon.localPosition = enemy.transform.position * 2;
                enemyIcon.localRotation = enemy.transform.rotation;
            }
        }
    }
}
