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
                return new ShipProperty(0, "SmallShip", 1.6f, 1, 1, Color.white);
            case 1:
                return new ShipProperty(1, "Cruiser", 1.2f, 1.5f, 1, Color.white);
            case 2:
                return new ShipProperty(2, "Frigate", 1, 2, 1, Color.red);
            case 3:
                return new ShipProperty(3, "BattleShip", 0.7f, 4, 1, Color.white);
            case 4:
                return new ShipProperty(0, "Ragnarok", 0.3f, 10, 1000, Color.red);

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
    public readonly float Armor;
    public readonly float Attack;
    public readonly Color Color;

    public ShipProperty(int shipID, string shipName, float speedFactor, float armor, float attack, Color color)
    {
        ShipID = shipID;
        ShipName = shipName;
        ShipPrefab = Resources.Load<GameObject>("Prefabs/" + ShipName);
        ShipSprite = Resources.Load<Sprite>("Images/" + ShipName);
        SpeedFactor = speedFactor;
        Armor = armor;
        Attack = attack;
        Color = color;
    }
}
