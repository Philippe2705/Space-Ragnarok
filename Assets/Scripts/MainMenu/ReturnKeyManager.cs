using UnityEngine;
using System.Collections;

public class ReturnKeyManager : MonoBehaviour
{

    public Animator mainMenu;

    public GameObject profileMenu;
    public GameObject infoWindow;
    public GameObject shopWindow;

    public GameObject settingsMenu;
    public GameObject infoWindow1;

    public GameObject connectToGoogle;

    void Start()
    {
        //var hostName = System.Net.Dns.GetHostName();
        //var hostEntry = System.Net.Dns.GetHostEntry(hostName);
        //print("Country : " + hostEntry);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (connectToGoogle.activeSelf)
            {
                connectToGoogle.SetActive(false);
            }
            else if (profileMenu.activeSelf)
            {
                if (infoWindow.activeSelf && !shopWindow.activeSelf)
                {
                    profileMenu.transform.GetChild(0).GetComponent<PanelManager>().CloseCurrent();
                    GetComponent<PanelManager>().OpenPanel(mainMenu);
                    shopWindow.SetActive(false);
                }
                else if (shopWindow.activeSelf)
                {
                    profileMenu.transform.GetChild(0).GetComponent<PanelManager>().OpenPanel(infoWindow.GetComponent<Animator>());
                }
            }
            else if (settingsMenu.activeSelf)
            {
                settingsMenu.transform.GetChild(0).GetComponent<PanelManager>().CloseCurrent();
                GetComponent<PanelManager>().OpenPanel(mainMenu);
                infoWindow1.SetActive(false);
            }
        }
    }
}
