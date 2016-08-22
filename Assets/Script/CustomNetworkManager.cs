using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Net;

public class CustomNetworkManager : MonoBehaviour
{
    public InputField IpField;
    public Text IpText;
    public InputField ServerPortField;
    public InputField ClientPortField;
    public Button SetupLocalServerButton;
    public Button SetupClientButton;

    string ip;
    int port = 4444;

    void Start()
    {
        SetupLocalServerButton.onClick.AddListener(SetupLocalServer);
        SetupClientButton.onClick.AddListener(SetupClient);

        ip = PlayerPrefs.GetString("Ip", "localhost");

        IpField.text = ip;
        ServerPortField.text = port.ToString();
        ClientPortField.text = port.ToString();
        IpText.text = GetLocalIp();
    }

    public void SetupLocalServer()
    {
        PlayerPrefs.SetString("Ip", ip);
        NetworkManager.singleton.networkAddress = ip;
        NetworkManager.singleton.networkPort = port;
        NetworkManager.singleton.StartHost();
    }


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