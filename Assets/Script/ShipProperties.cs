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
                return new ShipProperty(-1, "BotShip", 2, 1, 2, 1, Color.white);
            case 0:
                return new ShipProperty(0, "SmallShip", 10.6f, 5, 1, 10, Color.white);
            case 1:
                return new ShipProperty(1, "Frigate", 10.2f, 1, 1.5f, 10, Color.white);
            case 2:
                return new ShipProperty(2, "Cruiser", 1, 1, 2, 1, Color.red);
            case 3:
                return new ShipProperty(3, "BattleShip", 0.7f, 1, 4, 1, Color.white);
            case 4:
                return new ShipProperty(0, "Ragnarok", 0.3f, 1, 10, 1000, Color.red);

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
    public readonly Color Color;

    public ShipProperty(int shipID, string shipName, float speedFactor, float turnRate, float armor, float damage, Color color)
    {
        ShipID = shipID;
        ShipName = shipName;
        ShipPrefab = Resources.Load<GameObject>("Prefabs/" + ShipName);
        ShipSprite = Resources.Load<Sprite>("Images/" + ShipName);
        SpeedFactor = speedFactor;
        TurnRate = turnRate;
        Armor = armor;
        Damage = damage;
        Color = color;
    }
}
