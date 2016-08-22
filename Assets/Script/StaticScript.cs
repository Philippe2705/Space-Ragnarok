using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StaticScript : MonoBehaviour
{
    public Image shipPreview;
    public InputField PlayerNameInputServer;
    public InputField PlayerNameInputClient;


    public int shipId;
    public string pseudo;

    void Start()
    {
        PlayerPrefs.SetInt("ShipID", Experience.PlayerData.lastShipUsedID);
        DontDestroyOnLoad(gameObject);
        if (PlayerPrefs.GetInt("ShipID") >= 0 && PlayerPrefs.GetInt("ShipID") <= 4) //Vérifie qu'un ship préféré est déja défini avec un ID correct
        {
            shipId = PlayerPrefs.GetInt("ShipID");
        }
        else
        {
            shipId = 0;
        }
        SetShipID(shipId);
        PlayerNameInputServer.text = PlayerPrefs.GetString("Pseudo");
        PlayerNameInputClient.text = PlayerPrefs.GetString("Pseudo");
        pseudo = PlayerPrefs.GetString("Pseudo");
    }


    void OnLevelWasLoaded()
    {
        if (FindObjectsOfType<StaticScript>().Length == 2 && shipPreview == null)
        {
            Destroy(gameObject);
        }
    }


    public void SetShipID(int id)
    {
        Experience.LoadData();
        PlayerPrefs.SetInt("ShipID", id);
        shipId = id;
        shipPreview.sprite = ShipProperties.GetShip(id).ShipSprite;
        Experience.PlayerData.lastShipUsedID = id;
        Experience.SaveData();
    }

    public void OnPseudoChange(string newPseudo)
    {
        pseudo = newPseudo;
        PlayerPrefs.SetString("Pseudo", pseudo);
    }
}
