using UnityEngine;
using System.Collections;

public class ReturnKeyManager : MonoBehaviour {

    public Animator mainMenu;

    public GameObject profileMenu;
    public GameObject infoWindow;
    public GameObject shopWindow;

    public GameObject settingsMenu;
    public GameObject infoWindow1;

    public GameObject connectToGoogle;
    // Use this for initialization
    void Start () {
        var hostName = System.Net.Dns.GetHostName();
        var hostEntry = System.Net.Dns.GetHostEntry(hostName);
        print("Country : " + hostEntry);
    }
	
	// Update is called once per frame
	void Update () {
	if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(connectToGoogle.active)
            {
                connectToGoogle.SetActive(false);
            }
            else if (profileMenu.active)
            {
                if (infoWindow.active && !shopWindow.active)
                {
                    profileMenu.transform.GetChild(0).GetComponent<PanelManager>().CloseCurrent();
                    GetComponent<PanelManager>().OpenPanel(mainMenu);
                    shopWindow.SetActive(false);
                }
                else if (shopWindow.active)
                {
                    profileMenu.transform.GetChild(0).GetComponent<PanelManager>().OpenPanel(infoWindow.GetComponent<Animator>());
                }
            }
            else if (settingsMenu.active)
            {
                settingsMenu.transform.GetChild(0).GetComponent<PanelManager>().CloseCurrent();
                GetComponent<PanelManager>().OpenPanel(mainMenu);
                infoWindow1.SetActive(false);
            }
        }
	}
}
