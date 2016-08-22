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
                return new ShipProperty(-1, "BotShip", 3/*Speed Factor*/, 45/*Turn Rate*/, 2/*Armor*/, 10/*Domage*/, 10/*Reload Time*/, 5/*View Distance*/, "Orange");
            case 0:
                return new ShipProperty(0, "SmallShip", 5f/*Speed Factor*/, 45/*Turn Rate*/, 1/*Armor*/, 10/*Domage*/, 2f/*Reload Time*/, 7/*View Distance*/, "Orange");
            case 1:
                return new ShipProperty(1, "Frigate", 3f/*Speed Factor*/, 22.5f/*Turn Rate*/, 1.2f/*Armor*/, 12/*Domage*/, 3/*Reload Time*/, 7/*View Distance*/, "Orange");
            case 2:
                return new ShipProperty(2, "Cruiser", 1.5f/*Speed Factor*/, 22.5f/*Turn Rate*/, 1.5f/*Armor*/, 14/*Domage*/, 3/*Reload Time*/, 8/*View Distance*/, "Red");
            case 3:
                return new ShipProperty(3, "BattleShip", 0.7f/*Speed Factor*/, 18/*Turn Rate*/, 2.5f/*Armor*/, 17/*Domage*/, 3/*Reload Time*/, 9/*View Distance*/, "Orange");
            case 4:
                return new ShipProperty(4, "Ragnarok", 0.3f/*Speed Factor*/, 15/*Turn Rate*/, 5/*Armor*/, 24/*Domage*/, 3/*Reload Time*/, 12/*View Distance*/, "Purple");

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
    public readonly float ReloadTime;
    public readonly float ViewDistance;
    public readonly string MaterialColor;

    public ShipProperty(int shipID, string shipName, float speedFactor, float turnRate, float armor, float damage, float reloadTime, float viewDistance, string materialColor)
    {
        ShipID = shipID;
        ShipName = shipName;
        ShipPrefab = Resources.Load<GameObject>("Prefabs/" + ShipName);
        ShipSprite = Resources.Load<Sprite>("Images/" + ShipName);
        SpeedFactor = speedFactor;
        TurnRate = turnRate;
        Armor = armor;
        Damage = damage;
        ReloadTime = reloadTime;
        ViewDistance = viewDistance;
        MaterialColor = materialColor;
    }
}
