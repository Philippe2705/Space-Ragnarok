using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class PlayerConf : MonoBehaviour
{

    //Les différents Vcx
    public GameObject smallShip, cruiser, frigate, battleShip, ragnarok;
    public Sprite smallShipSprite, cruiserSprite, frigateSprite, battleShipSprite, ragnarokSprite; //Les Sprites des ships



    public GameObject shipPreview; // Image du vcx dans les profiles
    public ShipPropriety shipSettings;


    void Start()
    {

        // Code qui définie un vcx
        if (PlayerPrefs.GetInt("ShipID") >= 0 && PlayerPrefs.GetInt("ShipID") <= 4) //Vérifie qu'un ship préféré est déja défini avec un ID correct
        {
            SetLocalShip(PlayerPrefs.GetInt("ShipID"));
        }
        else
        {
            SetLocalShip(0);// Ship par défaut
        }
    }

    public void SetLocalShip(int shipIDChoose) // Change le Vcx a spawn et ses propriétés
    {
        PlayerPrefs.SetInt("ShipID", shipIDChoose);
        shipSettings.shipID = shipIDChoose;
        if (shipIDChoose == 0)
        {
            shipSettings.shipPrefab = smallShip;
            shipSettings.speedFactor = 1.6f;
            shipSettings.armor = 0;
            shipSettings.numberOfGunsOnEachSide = 8;
            shipPreview.GetComponent<Image>().sprite = smallShipSprite;
        }
        else if (shipIDChoose == 1)
        {
            shipSettings.shipPrefab = cruiser;
            shipSettings.speedFactor = 1f;
            shipSettings.armor = 0;
            shipSettings.numberOfGunsOnEachSide = 8;
            shipPreview.GetComponent<Image>().sprite = cruiserSprite;
        }
        else if (shipIDChoose == 2)
        {
            shipSettings.shipPrefab = frigate;
            shipSettings.speedFactor = 0.9f;
            shipSettings.armor = 0;
            shipSettings.numberOfGunsOnEachSide = 8;
            shipPreview.GetComponent<Image>().sprite = frigateSprite;
        }
        else if (shipIDChoose == 3)
        {
            shipSettings.shipPrefab = battleShip;
            shipSettings.speedFactor = 0.7f;
            shipSettings.armor = 0;
            shipSettings.numberOfGunsOnEachSide = 8;
            shipPreview.GetComponent<Image>().sprite = battleShipSprite;
        }
        else if (shipIDChoose == 4)
        {
            shipSettings.shipPrefab = ragnarok;
            shipSettings.speedFactor = 0.3f;
            shipSettings.armor = 0;
            shipSettings.numberOfGunsOnEachSide = 8;
            shipPreview.GetComponent<Image>().sprite = ragnarokSprite;
        }
        else { print("Incorect Ship ID"); return; }

        GetComponent<NetworkManager>().playerPrefab = shipSettings.shipPrefab; // Joueur a spawn
    }
    public struct ShipPropriety
    {
        public int shipID;
        public GameObject shipPrefab;
        public float speedFactor;
        public float armor;
        public int numberOfGunsOnEachSide;
    }
}
