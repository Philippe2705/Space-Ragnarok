using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class Experience
{
    public static PlayerDataStruct PlayerData;


    public static void SaveData()
    {
        BinaryFormatter binFor = new BinaryFormatter();
        FileStream dataFile = File.Create(Application.persistentDataPath + "/playerData.dat");

        binFor.Serialize(dataFile, PlayerData);

        dataFile.Close();
    }
    public static void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            BinaryFormatter binFor = new BinaryFormatter();
            FileStream dataFile = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open);

            PlayerData = (PlayerDataStruct)binFor.Deserialize(dataFile);

            dataFile.Close();
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
}
