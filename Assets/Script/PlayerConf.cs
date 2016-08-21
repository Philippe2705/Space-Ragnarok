//using UnityEngine;
//using UnityEngine.Networking;
//using UnityEngine.UI;
//using System.Collections;

//public class PlayerConf : MonoBehaviour
//{

//    //Les différents Vcx
//    public GameObject smallShip, cruiser, frigate, battleShip, ragnarok;
//    public Sprite smallShipSprite, cruiserSprite, frigateSprite, battleShipSprite, ragnarokSprite; //Les Sprites des ships



//    public GameObject shipPreview; // Image du vcx dans les profiles
//    public ShipProperty shipSettings;


//    void Start()
//    {

//        // Code qui définie un vcx
//        if (PlayerPrefs.GetInt("ShipID") >= 0 && PlayerPrefs.GetInt("ShipID") <= 4) //Vérifie qu'un ship préféré est déja défini avec un ID correct
//        {
//            SetLocalShip(PlayerPrefs.GetInt("ShipID"));
//        }
//        else
//        {
//            SetLocalShip(0);// Ship par défaut
//        }
//    }

//    public void SetLocalShip(int shipIDChoose) // Change le Vcx a spawn et ses propriétés
//    {
//        PlayerPrefs.SetInt("ShipID", shipIDChoose);
//        shipSettings.shipID = shipIDChoose;
//        if (shipIDChoose == 0)
//        {
//            shipSettings.shipPrefab = smallShip;
//            shipSettings.speedFactor = 1.6f;
//            shipSettings.armor = 1;
//            shipSettings.numberOfGunsOnEachSide = 4;
//            shipPreview.GetComponent<Image>().sprite = smallShipSprite;
//            shipPreview.GetComponent<Image>().SetNativeSize();
//        }
//        else if (shipIDChoose == 1)
//        {
//            shipSettings.shipPrefab = cruiser;
//            shipSettings.speedFactor = 1.2f;
//            shipSettings.armor = 1.5f;
//            shipSettings.numberOfGunsOnEachSide = 6;
//            shipPreview.GetComponent<Image>().sprite = frigateSprite;
//            shipPreview.GetComponent<Image>().SetNativeSize();
//        }
//        else if (shipIDChoose == 2)
//        {
//            shipSettings.shipPrefab = frigate;
//            shipSettings.speedFactor = 1f;
//            shipSettings.armor = 2;
//            shipSettings.numberOfGunsOnEachSide = 8;
//            shipPreview.GetComponent<Image>().sprite = cruiserSprite;
//            shipPreview.GetComponent<Image>().SetNativeSize();
//        }
//        else if (shipIDChoose == 3)
//        {
//            shipSettings.shipPrefab = battleShip;
//            shipSettings.speedFactor = 0.7f;
//            shipSettings.armor = 4;
//            shipSettings.numberOfGunsOnEachSide = 8;
//            shipPreview.GetComponent<Image>().sprite = battleShipSprite;
//            shipPreview.GetComponent<Image>().SetNativeSize();
//        }
//        else if (shipIDChoose == 4)
//        {
//            shipSettings.shipPrefab = ragnarok;
//            shipSettings.speedFactor = 0.3f;
//            shipSettings.armor = 10;
//            shipSettings.numberOfGunsOnEachSide = 8;
//            shipPreview.GetComponent<Image>().sprite = ragnarokSprite;
//            shipPreview.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
//        }
//        else
//        {
//            print("Incorrect Ship ID");
//            return;
//        }

//        GetComponent<NetworkManager>().playerPrefab = shipSettings.shipPrefab; // Joueur a spawn
//    }
//}

