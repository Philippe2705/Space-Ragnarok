using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StaticScript : MonoBehaviour
{
    public Image shipPreview;

    public int shipId;
    public string pseudo;

    void Start()
    {
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
    }

    public void SetShipID(int id)
    {
        PlayerPrefs.SetInt("ShipID", id);
        shipId = id;
        shipPreview.sprite = ShipProperties.GetShip(id).ShipSprite;
        NetworkManager.singleton.playerPrefab = ShipProperties.GetShip(id).ShipPrefab;
    }

    public void OnPseudoChange(string newPseudo)
    {
        pseudo = newPseudo;
    }
}
