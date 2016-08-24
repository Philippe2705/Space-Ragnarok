using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class ShipProperties
{
    public static ShipProperty GetShip(int shipId)
    {
        switch (shipId)
        {
            case 0:
                return new ShipProperty(0, "Destroyer", 5f/*Speed Factor*/, 45/*Turn Rate*/, 2/*Armor*/, 10/*Domage*/, 2f/*Reload Time*/, 7/*View Distance*/, 0/*Bullet Dispersion*/, "Orange");
            case 1:
                return new ShipProperty(1, "Frigate", 3f/*Speed Factor*/, 22.5f/*Turn Rate*/, 3f/*Armor*/, 12/*Domage*/, 3/*Reload Time*/, 7/*View Distance*/, 10/*Bullet Dispersion*/, "Orange");
            case 2:
                return new ShipProperty(2, "Cruiser", 1.5f/*Speed Factor*/, 22.5f/*Turn Rate*/, 5f/*Armor*/, 14/*Domage*/, 3/*Reload Time*/, 8/*View Distance*/, 10/*Bullet Dispersion*/, "Red");
            case 3:
                return new ShipProperty(3, "BattleShip", 0.7f/*Speed Factor*/, 18/*Turn Rate*/, 10f/*Armor*/, 17/*Domage*/, 3/*Reload Time*/, 9/*View Distance*/, 10/*Bullet Dispersion*/, "Orange");
            case 4:
                return new ShipProperty(4, "Ragnarok", 0.3f/*Speed Factor*/, 15/*Turn Rate*/, 50/*Armor*/, 24/*Domage*/, 3/*Reload Time*/, 12/*View Distance*/, 10/*Bullet Dispersion*/, "Purple");

            default:
                Debug.LogError("Incorrect Ship ID");
                return new ShipProperty();
        }
    }


    public static BotProperties GetBotProperties(int level)
    {
        switch (level)
        {
            case 10:
                return new BotProperties(10, 10, 10, 10);
            default:
                return new BotProperties(1, 1, 1, 1);
        }
    }
}

public struct BotProperties
{
    public readonly float ArmorMultiplier;
    public readonly float DamageMultiplier;
    public readonly float SpeedMultiplier;
    public readonly float TurnRateMultiplier;

    public BotProperties(float armorMultiplier, float damageMultiplier, float speedMultiplier, float turnRateMultiplier) : this()
    {
        ArmorMultiplier = armorMultiplier;
        DamageMultiplier = damageMultiplier;
        SpeedMultiplier = speedMultiplier;
        TurnRateMultiplier = turnRateMultiplier;
    }
}


public struct ShipProperty
{
    public readonly int ShipID;
    public readonly GameObject PlayerShipPrefab;
    public readonly GameObject BotShipPrefab;
    public readonly string ShipName;
    public readonly Sprite ShipSprite;
    public readonly float SpeedFactor;
    public readonly float TurnRate;
    public readonly float Armor;
    public readonly float Damage;
    public readonly float ReloadTime;
    public readonly float ViewDistance;
    public readonly float BulletDispersion;
    public readonly string MaterialColor;

    public ShipProperty(int shipID, string shipName, float speedFactor, float turnRate, float armor, float damage, float reloadTime, float viewDistance, float bulletDispersion, string materialColor)
    {
        ShipID = shipID;
        ShipName = shipName;
        PlayerShipPrefab = Resources.Load<GameObject>("Prefabs/" + ShipName + "Player");
        BotShipPrefab = Resources.Load<GameObject>("Prefabs/" + ShipName + "Bot");
        ShipSprite = Resources.Load<Sprite>("Images/" + ShipName);
        SpeedFactor = speedFactor;
        TurnRate = turnRate;
        Armor = armor;
        Damage = damage;
        ReloadTime = reloadTime;
        ViewDistance = viewDistance;
        BulletDispersion = bulletDispersion;
        MaterialColor = materialColor;
    }
}
