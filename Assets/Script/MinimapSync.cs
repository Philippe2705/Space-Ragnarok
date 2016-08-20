//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class MinimapSync : MonoBehaviour
//{
//    public GameObject camera1;
//    public GameObject joueur;
//    public GameObject miniMapIcon;
//    public GameObject botIcon;
//    public GameObject bot;
//    Vector2 pos1; //For background
//    Vector2 pos2; //For minimap

//    // Use this for initialization
//    void Start()
//    {
//        GameObject.Find("NetworkSyncer1").GetComponent<NetworkSync>().isAlive = false;
//        GameObject.Find("NetworkSyncer1").GetComponent<NetworkSync>().GameInit();
//    }

//    // Update is called once per frame
//    void FixedUpdate()
//    {
//        bot = GameObject.Find("ShipBot(Clone)");
//        if (GameObject.Find("NetworkSyncer1").GetComponent<NetworkSync>().isAlive == true)
//        {
//            pos1.x = joueur.transform.position.x * -0.25f;
//            pos1.y = joueur.transform.position.y * -0.25f;
//            transform.position = pos1;
//            camera1.transform.rotation = joueur.transform.rotation; // Sync la rotation y des deux caméras

//            //Minimap Sync
//            pos2.x = ((joueur.transform.position.x * 120) / 60);
//            pos2.y = ((joueur.transform.position.y * 60) / 30);
//            miniMapIcon.GetComponent<RectTransform>().localPosition = pos2;
//            miniMapIcon.GetComponent<RectTransform>().localRotation = joueur.transform.rotation;

//            botIcon.GetComponent<RectTransform>().localPosition = new Vector2(((bot.transform.position.x * 120) / 60), ((bot.transform.position.y * 60) / 30));
//            botIcon.GetComponent<RectTransform>().localRotation = bot.transform.rotation;
//        }
//    }

//    public void Disconnect()
//    {
//        Network.Disconnect();
//        SceneManager.LoadScene(0);
//    }
//}
