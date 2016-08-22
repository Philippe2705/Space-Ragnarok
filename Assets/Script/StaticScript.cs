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
        DontDestroyOnLoad(gameObject);
        SetShipID(UserData.GetLastShipId());
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
        UserData.SetLastShipId(id);
        shipId = id;
        shipPreview.sprite = ShipProperties.GetShip(id).ShipSprite;
    }

    public void OnPseudoChange(string newPseudo)
    {
        pseudo = newPseudo;
        PlayerPrefs.SetString("Pseudo", pseudo);
    }
}
