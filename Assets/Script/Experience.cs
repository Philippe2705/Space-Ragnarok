using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;

public static class Experience
{
   
    public static PlayerDataStruct PlayerData;


    static byte[] key = { 5, 65, 25, 46, 21, 55, 76, 68 }; // Where to store these keys is the tricky part, 
                                                    // you may need to obfuscate them or get the user to input a password each time
    static byte[] iv = { 15, 22, 53, 64, 75, 86, 27, 18 };
    static string path = Application.persistentDataPath + "/playerData.dat";
    

    public static void SaveData()
    {
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();

        // Encryption
        using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
        using (var cryptoStream = new CryptoStream(fs, des.CreateEncryptor(key, iv), CryptoStreamMode.Write))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            // This is where you serialize the class
            formatter.Serialize(cryptoStream, PlayerData);
        }
    }
    public static void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            // Decryption
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var cryptoStream = new CryptoStream(fs, des.CreateDecryptor(key, iv), CryptoStreamMode.Read))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                // This is where you deserialize the class
                PlayerData = (PlayerDataStruct)formatter.Deserialize(cryptoStream);
            }
        }

    }
    public static void AddExperience(int experienceToAdd)
    {
        LoadData();
        PlayerData.CurrentExperience += experienceToAdd;
        SaveData();
    }

    public static int GetExperience()
    {
        LoadData();
        return PlayerData.CurrentExperience;
    }

}

[Serializable]
public struct PlayerDataStruct
{
    public int CurrentExperience;
    public int lastShipUsedID;
}
