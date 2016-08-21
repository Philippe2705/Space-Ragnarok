using UnityEngine;
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
        PlayerPrefs.SetInt("ShipID", id);
        shipId = id;
        shipPreview.sprite = ShipProperties.GetShip(id).ShipSprite;
        NetworkManager.singleton.playerPrefab = ShipProperties.GetShip(id).ShipPrefab;
        if (id == 4)
        {
            shipPreview.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
        }
        else
        {
            shipPreview.GetComponent<Image>().SetNativeSize();
        }
    }

    public void OnPseudoChange(string newPseudo)
    {
        pseudo = newPseudo;
        PlayerPrefs.SetString("Pseudo", pseudo);
    }


    public void AvailableShip()
    {
        GameObject.Find("ExperienceLabel").GetComponent<Text>().text = "Experience : " + Experience.PlayerData.CurrentExperience.ToString();
        if (Experience.PlayerData.CurrentExperience >= 50)
        {
        //Frigate Available
            GameObject frigate = GameObject.Find("FrigateB");
            frigate.GetComponent<Button>().interactable = true;
            Color Couleur = frigate.transform.FindChild("Background").GetComponent<Image>().color;
            Couleur.a = 1;
            frigate.transform.FindChild("Background").GetComponent<Image>().color = Couleur;
        }
        else
        {
            //Frigate Disable
            GameObject frigate = GameObject.Find("FrigateB");
            frigate.GetComponent<Button>().interactable = false;
            Color Couleur = frigate.transform.FindChild("Background").GetComponent<Image>().color;
            Couleur.a = 0.5f;
            frigate.transform.FindChild("Background").GetComponent<Image>().color = Couleur;
        }

        if (Experience.PlayerData.CurrentExperience >= 200)
        {
            //Cruiser Available
            GameObject cruiser = GameObject.Find("CruiserB");
            cruiser.GetComponent<Button>().interactable = true;
            Color Couleur = new Color(1.0f, 1.0f, 1.0f, 0.2f);
            cruiser.transform.transform.GetChild(0).GetComponent<Image>().color = Couleur;
            print(cruiser.transform.GetChild(0));
        }
        else
        {
            //Cruiser Disable
            GameObject cruiser = GameObject.Find("CruiserB");
            cruiser.GetComponent<Button>().interactable = false;
            Color Couleur = cruiser.transform.transform.GetChild(0).GetComponent<Image>().color;
            Couleur.a = 0.2f;
            cruiser.transform.transform.GetChild(0).GetComponent<Image>().color = Couleur;
        }

        if (Experience.PlayerData.CurrentExperience >= 600)
        {
            //BatleShip Available
            GameObject battleShip = GameObject.Find("BattleShipB");
            battleShip.GetComponent<Button>().interactable = true;
            Color Couleur = battleShip.transform.FindChild("Background").GetComponent<Image>().color;
            Couleur.a = 1;
            battleShip.transform.FindChild("Background").GetComponent<Image>().color = Couleur;
        }
        else
        {
            //BatleShip Disable
            GameObject battleShip = GameObject.Find("BattleShipB");
            battleShip.GetComponent<Button>().interactable = false;
            Color Couleur = battleShip.transform.FindChild("Background").GetComponent<Image>().color;
            Couleur.a = 0.5f;
            battleShip.transform.FindChild("Background").GetComponent<Image>().color = Couleur;
        }

        if (Experience.PlayerData.CurrentExperience >= 1200)
        {
            //Ragnarok Available
            GameObject ragnarok = GameObject.Find("RagnarokB");
            ragnarok.GetComponent<Button>().interactable = true;
            Color Couleur = ragnarok.transform.FindChild("Background").GetComponent<Image>().color;
            Couleur.a = 1;
            ragnarok.transform.FindChild("Background").GetComponent<Image>().color = Couleur;
        }
        else
        {
            //Ragnarok Disable
            GameObject ragnarok = GameObject.Find("RagnarokB");
            ragnarok.GetComponent<Button>().interactable = false;
            Color Couleur = ragnarok.transform.FindChild("Background").GetComponent<Image>().color;
            Couleur.a = 0.5f;
            ragnarok.transform.FindChild("Background").GetComponent<Image>().color = Couleur;
        }
        if (Experience.PlayerData.CurrentExperience < 0)
        {
            Experience.PlayerData.CurrentExperience = 0;
        }
    }
    public void AddXP(int xp) // test funct
    {
        Experience.AddExperience(xp);
    }
}
