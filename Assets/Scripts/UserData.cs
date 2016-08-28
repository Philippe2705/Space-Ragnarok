using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;

public static class UserData
{

    private static PlayerData playerData;


    private static byte[] key = { 5, 65, 25, 46, 21, 55, 76, 68 }; // Where to store these keys is the tricky part, 
                                                                   // you may need to obfuscate them or get the user to input a password each time
    private static byte[] iv = { 15, 22, 53, 64, 75, 86, 27, 18 };
    private static string path = Application.persistentDataPath + "/playerData.dat";


    private static void SaveData()
    {
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();

        // Encryption
        using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
        using (var cryptoStream = new CryptoStream(fs, des.CreateEncryptor(key, iv), CryptoStreamMode.Write))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            // This is where you serialize the class
            formatter.Serialize(cryptoStream, playerData);
        }
    }
    private static void LoadData()
    {
        if (File.Exists(path))
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            // Decryption
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var cryptoStream = new CryptoStream(fs, des.CreateDecryptor(key, iv), CryptoStreamMode.Read))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                // This is where you deserialize the class
                playerData = (PlayerData)formatter.Deserialize(cryptoStream);
            }
        }
        else
        {
            Reset();
        }

    }

    private static void RemoveCredit(int creditToRemove)
    {
        LoadData();
        playerData.Credits -= creditToRemove;
        SaveData();
    }

    public static void AddExperience(int experienceToAdd)
    {
        LoadData();
        playerData.Credits += experienceToAdd;
        playerData.Experience += experienceToAdd;
        SaveData();
    }

    public static int GetExperience()
    {
        LoadData();
        return playerData.Experience;
    }

    public static int GetCredits()
    {
        LoadData();
        return playerData.Credits;
    }

    public static void AddCredits(int creditsToAdd)
    {
        LoadData();
        playerData.Credits += creditsToAdd;
        SaveData();
    }

    public static void SetShipId(int shipId)
    {
        LoadData();
        if (shipId < 0 || shipId > Constants.ShipsCount) //If a correct value never init, set to default
        {
            playerData.ShipId = 0;
        }
        else if (HasBoughtShip(shipId))
        {
            playerData.ShipId = shipId;
        }
        SaveData();
    }

    public static int GetShipId()
    {
        LoadData();
        if (playerData.ShipId < 0 || playerData.ShipId > Constants.ShipsCount) //If a correct value never init, set to default
        {
            playerData.ShipId = 0;
            SaveData();
        }
        return playerData.ShipId;
    }

    public static bool CanBuyShip(int shipId)
    {
        if (ShipProperties.GetShip(shipId).Price > GetCredits())
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static void BuyShip(int shipId)
    {
        if (CanBuyShip(shipId))
        {
            LoadData();
            RemoveCredit(ShipProperties.GetShip(shipId).Price);
            playerData.BoughtShips[shipId] = true;
            SaveData();
        }
    }
    public static bool HasBoughtShip(int shipId)
    {
        LoadData();
        return playerData.BoughtShips[shipId];
    }

    public static void AddHit()
    {
        LoadData();
        AddExperience(Constants.XpForHit);
        playerData.HitsCount++;
        SaveData();
    }

    public static void AddKill()
    {
        LoadData();
        AddExperience(Constants.XpForKill);
        playerData.KillsCount++;
        SaveData();
    }

    public static int GetKills()
    {
        LoadData();
        return playerData.KillsCount;
    }

    public static void Reset()
    {
        playerData = new PlayerData();
        playerData.BoughtShips[0] = true;
        SaveData();
    }
}

[Serializable]
public class PlayerData
{
    public int Experience;
    public int Credits;
    public int ShipId;

    public bool[] BoughtShips = new bool[100];

    public int HitsCount;
    public int KillsCount;
}
