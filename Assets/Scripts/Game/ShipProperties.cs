﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class ShipProperties
{
    public static ShipProperty GetShip(int shipId)
    {
        //switch (shipId)
        //{
        //    //case 0:
        //    //    //TODO: put real values back
        //    //    return new ShipProperty(0, "Destroyer", 5f/*Speed Factor*/, 45/*Turn Rate*/, 2/*Armor*/, 10/*Damage*/, 2f/*Reload Time*/, 7/*View Distance*/, 0/*Bullet Dispersion*/, 0/*Price*/, 80, "Orange");
        //    //case 1:
        //    //    return new ShipProperty(1, "Frigate", 3.2f/*Speed Factor*/, 22.5f/*Turn Rate*/, 3f/*Armor*/, 12/*Damage*/, 3/*Reload Time*/, 7/*View Distance*/, 10/*Bullet Dispersion*/, 1250/*Price*/, 80, "Orange");
        //    //case 2:
        //    //    return new ShipProperty(2, "Cruiser", 2.2f/*Speed Factor*/, 22.5f/*Turn Rate*/, 5f/*Armor*/, 14/*Damage*/, 3/*Reload Time*/, 8/*View Distance*/, 10/*Bullet Dispersion*/, 2500/*Price*/, 80, "Red");
        //    case 3:
        //        return new ShipProperty(3, "BattleShip", "Bullet", 1.2f/*Speed Factor*/, 18/*Turn Rate*/, 10f/*Armor*/, 17/*Damage*/, 3/*Reload Time*/, 9/*View Distance*/, 10/*Bullet Dispersion*/, 5000/*Price*/, 80, 0.1f, 3f, "Orange");
        //    //case 4:
        //    //    return new ShipProperty(4, "Ragnarok", 0.6f/*Speed Factor*/, 15/*Turn Rate*/, 50/*Armor*/, 24/*Damage*/, 3/*Reload Time*/, 12/*View Distance*/, 10/*Bullet Dispersion*/, 10000/*Price*/, 80, "Purple");

        //    default:
        //        Debug.LogError("Incorrect Ship ID");
        //        return new ShipProperty();
        //}
        string shipName;
        float speedFactor;
        float minSpeed;
        float turnRate;
        float armor;
        float damage;
        float reloadTime;
        float viewDistance;
        string bulletName;
        float bulletDispersion;
        float bulletSpeed;
        float fireAngleTolerance;
        int price;

        switch (shipId % 4)
        {
            case 0:
                shipName = "Destroyer";
                speedFactor = 5f;
                turnRate = 45;
                armor = 2;
                damage = 10;
                reloadTime = 2;
                viewDistance = 7;
                price = 1;
                minSpeed = 0.1f;
                break;
            case 1:
                shipName = "Frigate";
                speedFactor = 5f;
                turnRate = 45;
                armor = 2;
                damage = 10;
                reloadTime = 2;
                viewDistance = 7;
                price = 2;
                minSpeed = 0.1f;
                break;
            case 2:
                shipName = "Cruiser";
                speedFactor = 5f;
                turnRate = 45;
                armor = 2;
                damage = 10;
                reloadTime = 2;
                viewDistance = 7;
                price = 3;
                minSpeed = 0.1f;
                break;
            case 3:
                shipName = "BattleShip";
                speedFactor = 5f;
                turnRate = 45;
                armor = 2;
                damage = 10;
                reloadTime = 2;
                viewDistance = 7;
                price = 4;
                minSpeed = 0.1f;
                break;
            default:
                shipName = "Error";
                speedFactor = 0f;
                turnRate = 0;
                armor = 0;
                damage = 0;
                reloadTime = 0;
                viewDistance = 0;
                price = 0;
                minSpeed = 0;
                break;
        }
        var classe = shipId / 4 + 1;
        shipName += " Class " + classe.ToString();

        speedFactor *= classe * 0.5f;
        turnRate *= classe * 0.5f;
        armor *= classe * 0.5f;
        damage *= classe * 0.5f;
        reloadTime *= classe * 0.5f;
        viewDistance *= classe * 0.5f;
        price *= classe * 2;
        minSpeed /= classe * 0.5f;

        switch (classe)
        {
            case 1:
                bulletName = "BulletOrange";
                bulletDispersion = 10;
                bulletSpeed = 3;
                fireAngleTolerance = 80;
                break;
            default:
                bulletName = "";
                bulletDispersion = 0;
                bulletSpeed = 0;
                fireAngleTolerance = 0;
                break;
        }
        return new ShipProperty(shipId, shipName, speedFactor, minSpeed, turnRate, armor, damage, reloadTime, viewDistance, bulletName, bulletDispersion, bulletSpeed, fireAngleTolerance, price);
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
    public readonly string ShipName;
    public readonly GameObject PlayerShipPrefab;
    public readonly GameObject BotShipPrefab;
    public readonly GameObject BulletPrefab;
    public readonly Sprite ShipSprite;
    public readonly float SpeedFactor;
    public readonly float MinSpeed;
    public readonly float TurnRate;
    public readonly float Armor;
    public readonly float Damage;
    public readonly float ReloadTime;
    public readonly float ViewDistance;
    public readonly float BulletDispersion;
    public readonly float BulletSpeed;
    public readonly float FireAngleTolerance;
    public readonly int Price;

    public ShipProperty(int shipID, string shipName, float speedFactor, float minSpeed, float turnRate, float armor, float damage, float reloadTime, float viewDistance, string bulletName, float bulletDispersion, float bulletSpeed, float fireAngleTolerance, int price)
    {
        ShipID = shipID;
        ShipName = shipName;
        PlayerShipPrefab = Resources.Load<GameObject>("Prefabs/Ships/" + ShipName + " Player");
        BotShipPrefab = Resources.Load<GameObject>("Prefabs/Ships/" + ShipName + " Bot");
        BulletPrefab = Resources.Load<GameObject>("Prefabs/Bullets/" + bulletName);
        ShipSprite = Resources.Load<Sprite>("Images/" + ShipName);
        SpeedFactor = speedFactor;
        MinSpeed = minSpeed;
        TurnRate = turnRate;
        Armor = armor;
        Damage = damage;
        ReloadTime = reloadTime;
        ViewDistance = viewDistance;
        BulletDispersion = bulletDispersion;
        BulletSpeed = bulletSpeed;
        FireAngleTolerance = fireAngleTolerance;
        Price = price;
    }
}