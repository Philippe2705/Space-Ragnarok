using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Disconnect : NetworkBehaviour
{

    public void StartDisconnect()
    {
        GameObject.Find("Canvas").SetActive(false);
        GameObject.Find("LoadingScreen").GetComponent<Canvas>().enabled = true;
        NetworkManager.singleton.StopClient();
        if (isServer)
        {
            NetworkManager.singleton.StopServer();
            InvokeRepeating("DisconnectInvoke", 0, 0.25f);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    void DisconnectInvoke()
    {
        if (!NetworkServer.active)
        {
            SceneManager.LoadScene(0);
        }
    }
}
