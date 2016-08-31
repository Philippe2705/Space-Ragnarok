using UnityEngine;
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

        var classe = GetClass(shipId);
        if (classe == 1)
        {
            bulletName = "BulletOrange";
            switch (shipId % 4) //human
            {
                case 0:
                    shipName = "Destroyer";
                    price = 0;
                    break;
                case 1:
                    shipName = "Frigate";
                    price = 1250;
                    break;
                case 2:
                    shipName = "Cruiser";
                    price = 2500;
                    break;
                case 3:
                    shipName = "BattleShip";
                    price = 5000;
                    break;
                default:
                    shipName = "error";
                    price = 0;
                    break;
            }
        }
        else if (classe == 2) //alien
        {
            bulletName = "BulletOrange";
            switch (shipId % 4)
            {
                case 0:
                    shipName = "Pathfinder";
                    price = 250;
                    break;
                case 1:
                    shipName = "patroller";
                    price = 1500;
                    break;
                case 2:
                    shipName = "fighter";
                    price = 2750;
                    break;
                case 3:
                    shipName = "invasion ship";
                    price = 5250;
                    break;
                default:
                    shipName = "error";
                    price = 0;
                    break;
            }
        }
        else if (classe == 3) //cyborg
        {
            bulletName = "BulletOrange";
            switch (shipId % 4)
            {
                case 0:
                    shipName = "transporter";
                    price = 300;
                    break;
                case 1:
                    shipName = "fighter";
                    price = 1600;
                    break;
                case 2:
                    shipName = "cruisader";
                    price = 2800;
                    break;
                case 3:
                    shipName = "elite ship";
                    price = 5400;
                    break;
                default:
                    shipName = "error";
                    price = 0;
                    break;
            }
        }
        else if (classe == 4) //asgard
        {
            bulletName = "BulletOrange";
            switch (shipId % 4)
            {
                case 0:
                    shipName = "explorer";
                    price = 350;
                    break;
                case 1:
                    shipName = "protector";
                    price = 1650;
                    break;
                case 2:
                    shipName = "corvette";
                    price = 2850;
                    break;
                case 3:
                    shipName = "dreadnought";
                    price = 5450;
                    break;
                default:
                    shipName = "error";
                    price = 0;
                    break;
            }
        }
        else if (classe == 5) //pirate
        {
            bulletName = "BulletOrange";
            switch (shipId % 4)
            {
                case 0:
                    shipName = "shuttle";
                    price = 400;
                    break;
                case 1:
                    shipName = "blocker";
                    price = 1700;
                    break;
                case 2:
                    shipName = "assault ship";
                    price = 2900;
                    break;
                case 3:
                    shipName = "invader";
                    price = 5500;
                    break;
                default:
                    shipName = "error";
                    price = 0;
                    break;
            }
        }
        else { shipName = "error"; price = 0; }

        switch (classe)
        {
            case 1:
                bulletName = "BulletOrange";
                bulletDispersion = 10;
                bulletSpeed = 3;
                fireAngleTolerance = 80;
                shipName = "Human " + shipName;
                break;
            case 2:
                bulletName = "BulletOrange";
                bulletDispersion = 10;
                bulletSpeed = 3;
                fireAngleTolerance = 80;
                shipName = "Alien " + shipName;
                break;
            case 3:
                bulletName = "BulletOrange";
                bulletDispersion = 10;
                bulletSpeed = 3;
                fireAngleTolerance = 80;
                shipName = "Cyborg " + shipName;
                break;
            case 4:
                bulletName = "BulletOrange";
                bulletDispersion = 10;
                bulletSpeed = 3;
                fireAngleTolerance = 80;
                shipName = "Asgard " + shipName;
                break;
            case 5:
                bulletName = "BulletOrange";
                bulletDispersion = 10;
                bulletSpeed = 3;
                fireAngleTolerance = 80;
                shipName = "Pirate " + shipName;
                break;
            default:
                bulletName = "";
                bulletDispersion = 0;
                bulletSpeed = 0;
                fireAngleTolerance = 0;
                break;
        }
        if (shipId <= 50)
        {
            switch (shipId % 4)
            {
                case 0:
                    speedFactor = 7f;
                    turnRate = 65;
                    armor = 2;
                    damage = 10;
                    reloadTime = 6;
                    viewDistance = 12;
                    minSpeed = 0.1f;
                    break;
                case 1:
                    speedFactor = 5.5f;
                    turnRate = 40f;
                    armor = 3;
                    damage = 12;
                    reloadTime = 8;
                    viewDistance = 14;
                    minSpeed = 0.3f;
                    break;
                case 2:
                    speedFactor = 3f;
                    turnRate = 32f;
                    armor = 5;
                    damage = 14;
                    reloadTime = 8;
                    viewDistance = 15;
                    minSpeed = 0.2f;
                    break;
                case 3:
                    speedFactor = 2f;
                    turnRate = 25;
                    armor = 10;
                    damage = 17;
                    reloadTime = 6;
                    viewDistance = 20;
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

            speedFactor *= 0.5f;
            turnRate *= 0.5f;
            armor *= 0.5f;
            damage *= 0.5f;
            reloadTime *= 0.5f;
            viewDistance *= 0.5f;
            price *=  2;
            minSpeed /= 0.5f;


        }
        else if (shipId == 51)
        {
            shipName = "Ragnarok";
            speedFactor = 0.5f;
            turnRate = 10;
            armor = 18;
            damage = 40;
            reloadTime = 5;
            viewDistance = 15;
            price = 15000;
            minSpeed = -1f;
            bulletName = "BulletOrange";
            bulletDispersion = 0;
            bulletSpeed = 5;
            fireAngleTolerance = 30;
        }
        else if (shipId == 52)
        {
            shipName = "The Hunter";
            speedFactor = 0.5f;
            turnRate = 10;
            armor = 18;
            damage = 40;
            reloadTime = 5;
            viewDistance = 15;
            price = 15000;
            minSpeed = -1f;
            bulletName = "BulletOrange";
            bulletDispersion = 0;
            bulletSpeed = 5;
            fireAngleTolerance = 30;
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
                return new BotProperties(4f, 4f, 4f, 4f, "Hard");
            case 6:
                return new BotProperties(5, 5, 5, 5, "Very hard");
            case 7:
                return new BotProperties(8, 8, 8, 8, "Hardcore");
            case 8:
                return new BotProperties(10, 10, 10, 10, "Incredible");
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