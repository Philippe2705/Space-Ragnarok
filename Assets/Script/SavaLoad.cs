using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SavaLoad : MonoBehaviour
{
    private playerDataStruct playerData;

    // This script is loading the player's XP and then saving it

	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(gameObject);
        loadData();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        saveData();
	}

    public void saveData() // Sauvegarde continue
    {
        BinaryFormatter binFor = new BinaryFormatter();
        FileStream dataFile = File.Create(Application.persistentDataPath + "/playerData.dat");

        binFor.Serialize(dataFile, playerData);

        dataFile.Close();
    }
    public void loadData() //Chargé une fois au début
    {
        if(File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            BinaryFormatter binFor = new BinaryFormatter();
            FileStream dataFile = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open);

            playerData = (playerDataStruct)binFor.Deserialize(dataFile);

            dataFile.Close();
        }
        
    }
    public void addExperience(int experienceToAdd)
    {
        playerData.currentExperience += experienceToAdd;
    }

    [Serializable]
    struct playerDataStruct
    {
        public int currentExperience;
    }

}
