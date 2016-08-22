using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Disconnect : NetworkBehaviour
{

    public void StartDisconnect()
    {
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
