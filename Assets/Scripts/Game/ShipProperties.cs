using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class ShipProperties
{
    public static ShipProperty GetShip(int shipId)
    {
        string shipName = "";
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
        float classeDomageFactor = 1;
        float classeViewDistanceFactor = 1;
        float classeArmorFactor = 1;
        int price = 0;

        var classe = GetClass(shipId);

        if (shipId <= 50)
        {
            switch (classe)
            {

                case 1:
                    classeDomageFactor = 1.3f;
                    classeViewDistanceFactor = 1;
                    switch (shipId % 4) //human
                    {
                        case 0:
                            shipName = "Destroyer";
                            price = 0;
                            break;
                        case 1:
                            shipName = "Frigate";
                            price = 1500;
                            break;
                        case 2:
                            shipName = "Cruiser";
                            price = 5000;
                            break;
                        case 3:
                            shipName = "BattleShip";
                            price = 10000;
                            break;
                    }
                    bulletName = "BulletOrange";
                    bulletDispersion = 10;
                    bulletSpeed = 3;
                    fireAngleTolerance = 80;
                    shipName = "Human " + shipName;
                    break;
                case 2:
                    classeDomageFactor = 1.2f;
                    switch (shipId % 4)
                    {
                        case 0:
                            shipName = "Pathfinder";
                            price = 250;
                            break;
                        case 1:
                            shipName = "Patroller";
                            price = 1500;
                            break;
                        case 2:
                            shipName = "Fighter";
                            price = 2750;
                            break;
                        case 3:
                            shipName = "Invasion Ship";
                            price = 5250;
                            break;
                    }
                    bulletName = "BulletPuple";
                    bulletDispersion = 10;
                    bulletSpeed = 5;
                    fireAngleTolerance = 10;
                    shipName = "Alien " + shipName;
                    break;
                case 3:
                    switch (shipId % 4)
                    {
                        case 0:
                            shipName = "Transporter";
                            price = 300;
                            break;
                        case 1:
                            shipName = "Fighter";
                            price = 1600;
                            break;
                        case 2:
                            shipName = "Cruisader";
                            price = 2800;
                            break;
                        case 3:
                            shipName = "Elite Ship";
                            price = 5400;
                            break;
                    }
                    bulletName = "BulletOrange";
                    bulletDispersion = 10;
                    bulletSpeed = 3;
                    fireAngleTolerance = 10;
                    shipName = "Cyborg " + shipName;
                    break;
                case 4:
                    classeDomageFactor = 1.2f;
                    classeViewDistanceFactor = 1;
                    switch (shipId % 4)
                    {
                        case 0:
                            shipName = "Explorer";
                            price = 350;
                            break;
                        case 1:
                            shipName = "Protector";
                            price = 1650;
                            break;
                        case 2:
                            shipName = "Corvette";
                            price = 2850;
                            break;
                        case 3:
                            shipName = "Dreadnought";
                            price = 5450;
                            break;
                    }
                    bulletName = "BulletOrange";
                    bulletDispersion = 10;
                    bulletSpeed = 4;
                    fireAngleTolerance = 10;
                    shipName = "Asgard " + shipName;
                    break;
                case 5:
                    classeArmorFactor = 0.6f;
                    classeDomageFactor = 0.4f;
                    classeViewDistanceFactor = 1;
                    switch (shipId % 4)
                    {
                        case 0:
                            shipName = "Shuttle";
                            price = 400;
                            break;
                        case 1:
                            shipName = "Blocker";
                            price = 1700;
                            break;
                        case 2:
                            shipName = "Assault Ship";
                            price = 2900;
                            break;
                        case 3:
                            shipName = "Invader";
                            price = 5500;
                            break;
                    }
                    bulletName = "BulletRed";
                    bulletDispersion = 10;
                    bulletSpeed = 4;
                    fireAngleTolerance = 10;
                    shipName = "Daleks " + shipName;
                    break;
                default:
                    bulletName = "";
                    bulletDispersion = 0;
                    bulletSpeed = 0;
                    fireAngleTolerance = 0;
                    break;
            }
            switch (shipId % 4)
            {
                case 0:
                    speedFactor = 7f;
                    turnRate = 65;
                    armor = 2;
                    damage = 10;
                    reloadTime = 6;
                    viewDistance = 8;
                    minSpeed = 0.5f;
                    break;
                case 1:
                    speedFactor = 5.5f;
                    turnRate = 40f;
                    armor = 3;
                    damage = 12;
                    reloadTime = 8;
                    viewDistance = 9;
                    minSpeed = 0.3f;
                    break;
                case 2:
                    speedFactor = 4.5f;
                    turnRate = 32f;
                    armor = 5;
                    damage = 14;
                    reloadTime = 8;
                    viewDistance = 10;
                    minSpeed = 0.2f;
                    break;
                case 3:
                    speedFactor = 3.5f;
                    turnRate = 25;
                    armor = 10;
                    damage = 17;
                    reloadTime = 6;
                    viewDistance = 10;
                    minSpeed = 0.1f;
                    break;
                default:
                    speedFactor = 0f;
                    turnRate = 0;
                    armor = 0;
                    damage = 0;
                    reloadTime = 0;
                    viewDistance = 0;
                    minSpeed = 0;
                    break;
            }
        }
        else if (shipId == 51)
        {
            shipName = "Ragnarok";
            speedFactor = 0.5f;
            turnRate = 14;
            armor = 22;
            damage = 55;
            reloadTime = 3;
            viewDistance = 15;
            price = 15000;
            minSpeed = -1f;
            bulletName = "BulletRed";
            bulletDispersion = 0;
            bulletSpeed = 5;
            fireAngleTolerance = 10;
        }
        else if (shipId == 52)
        {
            shipName = "The Hunter";
            speedFactor = 0.5f;
            turnRate = 10;
            armor = 10;
            damage = 30;
            reloadTime = 5;
            viewDistance = 15;
            price = 15000;
            minSpeed = -1f;
            bulletName = "BulletOrange";
            bulletDispersion = 0;
            bulletSpeed = 6;
            fireAngleTolerance = 10;
        }
        else
        {
            shipName = "Error";
            speedFactor = 0f;
            turnRate = 0;
            armor = 0;
            damage = 0;
            reloadTime = 0;
            viewDistance = 0;
            price = 0;
            minSpeed = 0;
            bulletName = "";
            bulletDispersion = 0;
            bulletSpeed = 0;
            fireAngleTolerance = 0;
        }
        price *= 2;
        viewDistance *= classeViewDistanceFactor;
        damage *= classeDomageFactor;
        armor *= classeArmorFactor;
        return new ShipProperty(shipId, shipName, speedFactor, minSpeed, turnRate, armor, damage, reloadTime, viewDistance, bulletName, bulletDispersion, bulletSpeed, fireAngleTolerance, price);
    }

    public static int GetClass(int shipId)
    {
        if (shipId <= 50)
        {
            return shipId / 4 + 1;
        }
        else if (shipId == 51)
        {
            return 1;
        }
        else if (shipId == 52)
        {
            return 1;
        }
        else
        {
            Debug.LogError("Wrong id");
            return 0;
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