//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;
//using System.Net;

//public class NetworkSync : MonoBehaviour
//{
//    public GameObject shipPrefab;
//    public GameObject ship;
//    public bool isAlive;
//    public bool isConnected = false;
//    public GameObject errorField;
//    public string myPseudo;
//    float timer;
//    public float respawnTime;
//    bool error = false;
//    string errorReportType;
//    public GameObject LocalIpField;
//    public GameObject pannelManager;
//    public GameObject botShipPrefab;


//    // Use this for initialization
//    void Start()
//    {
//        respawnTime = 1;
//        if (Application.loadedLevel == 0)
//        {
//            LocalIpField.GetComponent<Text>().text = getLocalIp();
//        }
//        if (!GetComponent<NetworkView>())
//        {
//            gameObject.SetActive(false);
//        }
//        Application.runInBackground = true;
//        DontDestroyOnLoad(transform.gameObject);
//        isAlive = true;

//        PlayerPrefs.SetInt("iSFirst", 1);
//        if (PlayerPrefs.GetInt("iSFirst") == 11)
//        {
//            Destroy(GameObject.Find("NetworkSyncer"));
//            print("detruction de l'autre networkSync");
//        }
//        else
//        {
//            PlayerPrefs.SetInt("iSFirst", 11);
//            this.name = "NetworkSyncer1";
//        }
//    }
//    // Update is called once per frame
//    void Update()
//    {
//        if (GameObject.Find("ShipBot(Clone)") == null && Application.loadedLevel == 1)
//        {

//            Network.Instantiate(botShipPrefab, transform.position, transform.rotation, 0);
//        }
//        respawnTime -= Time.deltaTime;
//        if (Application.loadedLevel == 0)
//        {
//            pannelManager = GameObject.Find("PanelManager1"); ;
//            if (pannelManager != null)
//            {
//                myPseudo = pannelManager.GetComponent<PanelManager>().pseudo;
//            }
//        }
//        if (isAlive == false && isConnected == true && respawnTime <= 0)
//        {
//            print("Spawn player");
//            transform.Rotate(0, 0, Random.Range(0, 360));
//            Network.Instantiate(shipPrefab, new Vector2(Random.Range(-50, 50), Random.Range(-20, 20)), transform.rotation, 0);

//            isAlive = true;
//        }

//        timer -= Time.deltaTime;
//        if (timer < 0 && error == true) { FailureReport(errorReportType); }
//    }

//    void OnServerInitialized()
//    {
//        print("Serveur lancé");
//        isConnected = true;

//    }
//    void OnConnectedToServer()
//    {
//        print("Connecté au serveur");
//        isConnected = true;
//    }
//    void OnClientError()
//    {
//        print("Erreur du client");
//    }
//    void OnFailedToConnect()
//    {
//        print("Impossible de se connecter à l'adresse Ip");
//        Application.LoadLevel(0);
//        error = true;
//        timer = 0.2f;
//        errorReportType = "Impossible de se connecter au serveur " + PlayerPrefs.GetString("IpToConnect");
//    }
//    void OnPlayerConnected()
//    {
//        print("Un joueur s'est connecté");
//    }
//    public void FailureReport(string errorSend)
//    {
//        if (Application.loadedLevel == 0)
//        {
//            errorField = GameObject.Find("ErrorField");
//            errorField.GetComponent<Text>().text = errorSend;
//            error = false;
//        }
//    }
//    public void GameInit()
//    {
//        string clientType = PlayerPrefs.GetString("ClientType");
//        if (Application.loadedLevel == 1)
//        {
//            if (clientType == "server")
//            {
//                Network.InitializeServer(4, 2016, false);
//                print("Lancement du serveur...");
//            }
//            else if (clientType == "client")
//            {
//                string Ip = PlayerPrefs.GetString("IpToConnect");
//                Network.Connect(Ip, 2016);
//                print("Connection au serveur en cours...");
//            }
//            else
//            {
//                error = true;
//                errorReportType = "Erreur du type serveur/client";
//                Application.LoadLevel(0);
//            }
//        }
//    }
//    public string getLocalIp()
//    {
//        IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
//        foreach (IPAddress addr in localIPs)
//        {
//            if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
//            {
//                return addr.ToString();

//            }

//        }
//        return "error";
//    }
//    public void selectShip(int shipID)
//    {

//    }
//}