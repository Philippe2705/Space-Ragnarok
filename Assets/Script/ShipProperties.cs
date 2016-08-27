using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class ShipProperties
{
    public static ShipProperty GetShip(int shipId)
    {
        switch (shipId)
        {
            //case 0:
            //    //TODO: put real values back
            //    return new ShipProperty(0, "Destroyer", 5f/*Speed Factor*/, 45/*Turn Rate*/, 2/*Armor*/, 10/*Damage*/, 2f/*Reload Time*/, 7/*View Distance*/, 0/*Bullet Dispersion*/, 0/*Price*/, 80, "Orange");
            //case 1:
            //    return new ShipProperty(1, "Frigate", 3.2f/*Speed Factor*/, 22.5f/*Turn Rate*/, 3f/*Armor*/, 12/*Damage*/, 3/*Reload Time*/, 7/*View Distance*/, 10/*Bullet Dispersion*/, 1250/*Price*/, 80, "Orange");
            //case 2:
            //    return new ShipProperty(2, "Cruiser", 2.2f/*Speed Factor*/, 22.5f/*Turn Rate*/, 5f/*Armor*/, 14/*Damage*/, 3/*Reload Time*/, 8/*View Distance*/, 10/*Bullet Dispersion*/, 2500/*Price*/, 80, "Red");
            case 3:
                return new ShipProperty(3, "BattleShip", 1.2f/*Speed Factor*/, 18/*Turn Rate*/, 10f/*Armor*/, 17/*Damage*/, 3/*Reload Time*/, 9/*View Distance*/, 10/*Bullet Dispersion*/, 5000/*Price*/, 80, 0.1f, "Orange");
            //case 4:
            //    return new ShipProperty(4, "Ragnarok", 0.6f/*Speed Factor*/, 15/*Turn Rate*/, 50/*Armor*/, 24/*Damage*/, 3/*Reload Time*/, 12/*View Distance*/, 10/*Bullet Dispersion*/, 10000/*Price*/, 80, "Purple");

            default:
                Debug.LogError("Incorrect Ship ID");
                return new ShipProperty();
        }
    }


    public static BotProperties GetBotProperties(int level)
    {
        switch (level)
        {
            case 0:
                return new BotProperties(0.7f, 0.7f, 0.7f, 0.7f, "Beginneer");
            case 1:
                return new BotProperties(1, 1, 1, 1, "Easy");
            case 2:
                return new BotProperties(1.4f, 1.4f, 1.4f, 1.4f, "Medium-Easy");
            case 3:
                return new BotProperties(2, 2, 2, 2, "Medium");
            case 4:
                return new BotProperties(3, 3, 3, 3, "Advanced");
            case 5:
                return new BotProperties(4.5f, 4.5f, 4.5f, 4.5f, "Hard");
            case 6:
                return new BotProperties(6, 6, 6, 6, "Very hard");
            case 7:
                return new BotProperties(9, 9, 9, 9, "Hardcore");
            case 8:
                return new BotProperties(12, 12, 12, 12, "Incredible");
            case 9:
                return new BotProperties(20, 20, 20, 20, "Chuck Norris");
            default:
                return new BotProperties(1, 1, 1, 1, "Default");
        }
    }
}

public struct BotProperties
{
    public readonly float ArmorMultiplier;
    public readonly float DamageMultiplier;
    public readonly float SpeedMultiplier;
    public readonly float TurnRateMultiplier;
    public readonly string LevelName;

    public BotProperties(float armorMultiplier, float damageMultiplier, float speedMultiplier, float turnRateMultiplier, string levelName)
    {
        ArmorMultiplier = armorMultiplier;
        DamageMultiplier = damageMultiplier;
        SpeedMultiplier = speedMultiplier;
        TurnRateMultiplier = turnRateMultiplier;
        LevelName = levelName;
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
    public readonly int Price;
    public readonly float FireAngleTolerance;
    public readonly float MinSpeed;
    public readonly string MaterialColor;

    public ShipProperty(int shipID, string shipName, float speedFactor, float turnRate, float armor, float damage, float reloadTime, float viewDistance, float bulletDispersion, int price, float fireAngleTolerance, float minSpeed, string materialColor)
    {
        ShipID = shipID;
        ShipName = shipName;
        PlayerShipPrefab = Resources.Load<GameObject>("Prefabs/Ships/" + ShipName + "Player");
        BotShipPrefab = Resources.Load<GameObject>("Prefabs/Ships/" + ShipName + "Bot");
        ShipSprite = Resources.Load<Sprite>("Images/" + ShipName);
        SpeedFactor = speedFactor;
        TurnRate = turnRate;
        Armor = armor;
        Damage = damage;
        ReloadTime = reloadTime;
        ViewDistance = viewDistance;
        BulletDispersion = bulletDispersion;
        Price = price;
        FireAngleTolerance = fireAngleTolerance;
        MinSpeed = minSpeed;
        MaterialColor = materialColor;
    }
}