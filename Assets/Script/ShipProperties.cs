using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class ShipProperties
{
    public static ShipProperty GetShip(int shipId)
    {
        switch (shipId)
        {
            case -1:
                return new ShipProperty(-1, "BotShip", 2, 1, 2, 1, 1, Color.white, "BulletOrange");
            case 0:
                return new ShipProperty(0, "SmallShip", 5f, 3, 1, 10, 1, Color.white, "BulletOrange");
            case 1:
                return new ShipProperty(1, "Frigate", 3f, 1.5f, 1.5f, 10, 1.2f, Color.white, "BulletOrange");
            case 2:
                return new ShipProperty(2, "Cruiser", 1.5f, 1.5f, 2, 1, 1.3f, Color.red, "BulletRed");
            case 3:
                return new ShipProperty(3, "BattleShip", 0.7f, 1, 4, 1.5f, 1.5f, Color.white, "BulletOrange");
            case 4:
                return new ShipProperty(0, "Ragnarok", 0.3f, 1, 20, 10, 2, Color.red, "BulletPurple");

            default:
                Debug.LogError("Incorrect Ship ID");
                return new ShipProperty();
        }
    }

}

public struct ShipProperty
{
    public readonly int ShipID;
    public readonly GameObject ShipPrefab;
    public readonly string ShipName;
    public readonly Sprite ShipSprite;
    public readonly float SpeedFactor;
    public readonly float TurnRate;
    public readonly float Armor;
    public readonly float Damage;
    public readonly float ViewDistanceFactor;
    public readonly Color Color;
    public readonly string MaterialColor;

    public ShipProperty(int shipID, string shipName, float speedFactor, float turnRate, float armor, float damage, float viewDistanceFactor, Color color, string materialColor)
    {
        ShipID = shipID;
        ShipName = shipName;
        ShipPrefab = Resources.Load<GameObject>("Prefabs/" + ShipName);
        ShipSprite = Resources.Load<Sprite>("Images/" + ShipName);
        SpeedFactor = speedFactor;
        TurnRate = turnRate;
        Armor = armor;
        Damage = damage;
        ViewDistanceFactor = viewDistanceFactor;
        Color = color;
        MaterialColor = materialColor;
    }
}
