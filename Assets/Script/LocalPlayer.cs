//using UnityEngine;
//using UnityEngine.Networking;

//public class LocalPlayer : NetworkBehaviour
//{
//    public bool isAtStartup = true;
//    NetworkClient myClient;

//    public GameObject Landscape;
//    public GameObject MainCamera;
//    public GameObject playerName;
//    public GameObject netsyncer;
//    public GameObject Mycamera;
//    [SyncVar]
//    public string pname; // Var to sync


//    void Start()
//    {
//        if (GetComponent<NetworkView>().isMine)
//        {
//            netsyncer = GameObject.Find("NetworkSyncer1"); // The username is saved in networksyncer1, this object is running in background
//            GetComponent<PlayerController>().enabled = true;
//            Landscape.SetActive(true);
//            MainCamera.SetActive(true);
//            playerName.GetComponent<MeshRenderer>().enabled = false;
//        }

//        else
//        {
//            Mycamera = GameObject.Find("Main Camera");
//            GetComponent<PlayerController>().rightGun.gameObject.SetActive(false);
//            GetComponent<PlayerController>().leftGun.gameObject.SetActive(false);
//        }
//    }


//    // Update is called once per frame
//    void Update()
//    {
//        if (GetComponent<NetworkView>().isMine == false)
//        {
//            playerName.transform.rotation = Mycamera.transform.rotation; // Make the 3dText in the right rotation
//        }
//        else
//        {
//            if (Network.isServer)
//            {
//                pname = PlayerPrefs.GetString("PlayerName");
//                gameObject.transform.FindChild("Player_name").GetComponent<TextMesh>().text = name;
//                gameObject.name = pname;
//            }
//            else
//            {
//                pname = PlayerPrefs.GetString("PlayerName");
//                RpcSetPlayerName(pname, gameObject);
//            }

//        }
//    }

//    [ClientRpc]
//    void RpcSetPlayerName(string name, GameObject ship)
//    {
//        gameObject.transform.FindChild("Player_name").GetComponent<TextMesh>().text = name;
//        ship.name = name;
//    }
//}
