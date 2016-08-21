using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Net;

public class MyNetworkManager : MonoBehaviour
{
    public bool isAtStartup = true;
    public InputField ipField;
    public Text ipText;
    public InputField serverPortField;
    public InputField clientPortField;

    string ip;
    int port = 4444;
    NetworkClient myClient;

    void Start()
    {
        ip = PlayerPrefs.GetString("Ip", "localhost");
        ipField.text = ip;
        serverPortField.text = port.ToString();
        clientPortField.text = port.ToString();
        ipText.text = GetLocalIp();
    }

    public void SetupLocalServer()
    {
        PlayerPrefs.SetString("Ip", ip);
        NetworkManager.singleton.networkAddress = ip;
        NetworkManager.singleton.networkPort = port;
        NetworkManager.singleton.StartHost();
    }

    // Create a client and connect to the server port
    public void SetupClient()
    {
        PlayerPrefs.SetString("Ip", ip);
        NetworkManager.singleton.networkAddress = ip;
        NetworkManager.singleton.networkPort = port;
        NetworkManager.singleton.StartClient();
    }

    public string GetLocalIp()
    {
        IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
        foreach (IPAddress addr in localIPs)
        {
            if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return addr.ToString();

            }

        }
        return "error";
    }
}