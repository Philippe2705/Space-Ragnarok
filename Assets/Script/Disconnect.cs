using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Disconnect : NetworkBehaviour {
    
    public void StartDisconnect()
    {
        NetworkManager.singleton.StopClient();
        if (isServer)
        {
            NetworkManager.singleton.StopServer();
        }
        SceneManager.LoadScene(0);
    }
}
