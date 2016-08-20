using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetupClient()
    {
        Network.Connect("192.168.1.27", "7777");
    }
}
