using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class PanelManager : MonoBehaviour {

	public Animator initiallyOpen;

	private int m_OpenParameterId;
	private Animator m_Open;
	private GameObject m_PreviouslySelected;
	
	public GameObject Netsyncer;
	public string localIp;

	const string k_OpenTransitionName = "Open";
	const string k_ClosedStateName = "Closed";
	public InputField nameInput;
	public string pseudo = "";
	void Start()
	{
	}
	public void OnEnable()
	{
		m_OpenParameterId = Animator.StringToHash (k_OpenTransitionName);

		if (initiallyOpen == null)
			return;

		OpenPanel(initiallyOpen);
	}

	public void OpenPanel (Animator anim)
	{
		if (m_Open == anim)
			return;
		else {
			anim.gameObject.SetActive (true);
			var newPreviouslySelected = EventSystem.current.currentSelectedGameObject;

			anim.transform.SetAsLastSibling ();

			CloseCurrent ();

			m_PreviouslySelected = newPreviouslySelected;

			m_Open = anim;
			m_Open.SetBool (m_OpenParameterId, true);

			GameObject go = FindFirstEnabledSelectable (anim.gameObject);

			SetSelected (go);
		}
	}

	static GameObject FindFirstEnabledSelectable (GameObject gameObject)
	{
		GameObject go = null;
		var selectables = gameObject.GetComponentsInChildren<Selectable> (true);
		foreach (var selectable in selectables) {
			if (selectable.IsActive () && selectable.IsInteractable ()) {
				go = selectable.gameObject;
				break;
			}
		}
		return go;
	}

	public void CloseCurrent()
	{
		if (m_Open == null)
			return;

		m_Open.SetBool(m_OpenParameterId, false);
		SetSelected(m_PreviouslySelected);
		StartCoroutine(DisablePanelDeleyed(m_Open));
		m_Open = null;
	}

	IEnumerator DisablePanelDeleyed(Animator anim)
	{
		bool closedStateReached = false;
		bool wantToClose = true;
		while (!closedStateReached && wantToClose)
		{
			if (!anim.IsInTransition(0))
				closedStateReached = anim.GetCurrentAnimatorStateInfo(0).IsName(k_ClosedStateName);

			wantToClose = !anim.GetBool(m_OpenParameterId);

			yield return new WaitForEndOfFrame();
		}

		if (wantToClose)
			anim.gameObject.SetActive(false);
	}

	private void SetSelected(GameObject go)
	{
		EventSystem.current.SetSelectedGameObject(go);
	}
	public void hostserver()
	{

		pseudo = GameObject.Find ("PlayerNameWritten").GetComponent<Text> ().text;
		print (pseudo);
		if (pseudo.Length >= 0) {
			Application.LoadLevel (1);
			PlayerPrefs.SetString ("ClientType", "server");
			PlayerPrefs.SetString("PlayerName", pseudo);
			PlayerPrefs.SetString("IpToConnect", "localhost");
		} else {
			GameObject.Find("ErrorFieldHost").GetComponent<Text>().text = "Le pseudo doit faire entre 3 et 14 caracteres";
		}
	}
	public GameObject ipToConnectField;
	public void connectToServer()
	{
		string ip = ipToConnectField.GetComponent<Text> ().text;
		pseudo = GameObject.Find ("PlayerNameWrittenJoin").GetComponent<Text> ().text;
		if (pseudo.Length >= 0 && ip.Length > 0) 
		{
			PlayerPrefs.SetString ("ClientType", "client");
			PlayerPrefs.SetString("PlayerName", pseudo);
			if (ipToConnectField.GetComponent<Text> ().text == "") {
				PlayerPrefs.SetString ("IpToConnect", "192.168.1.27");
			} else {
				PlayerPrefs.SetString ("IpToConnect", ip);
			}
			Application.LoadLevel (1);
		} 
		else if (ip.Length == 0) 
		{
			GameObject.Find("ErrorFieldJoin").GetComponent<Text>().text = "Ip Incorrecte";
		}
		else {
			GameObject.Find("ErrorFieldJoin").GetComponent<Text>().text = "Le pseudo doit faire entre 3 et 14 caracteres";
		}
	}
    public void savePlayerPrefIPAndUsernameHost() // With these 2 funct, we do not need anymore to re-type the IP and the pseudo
    {
        PlayerPrefs.SetString("LastUsername", GameObject.Find("PlayerNameWritten").GetComponent<Text>().text);
    }
    public void savePlayerPrefIPAndUsernameClient() // With these 2 funct, we do not need anymore to re-type the IP and the pseudo
    {
        if (ipToConnectField.GetComponent<Text>().text != "")
        {
            PlayerPrefs.SetString("LastIp", ipToConnectField.GetComponent<Text>().text);
        }
        PlayerPrefs.SetString("LastUsername", GameObject.Find("PlayerNameWrittenJoin").GetComponent<Text>().text);
    }
    public void LoadPlayerPrefIPAndUsername(int number)
    {
        if (PlayerPrefs.GetString("LastUsername") != "")
        {
            if (number == 1)
            {
                GameObject.Find("PlayerNameWrittenJoin").GetComponent<Text>().text = PlayerPrefs.GetString("LastUsername");
            }
            else if (number == 2)
            {
                GameObject.Find("PlayerNameWritten").GetComponent<Text>().text = PlayerPrefs.GetString("LastUsername");
            }
        }
        if (PlayerPrefs.GetString("LastUsername") != "")
        {
            if (number == 1)
            {
                ipToConnectField.GetComponent<Text>().text = PlayerPrefs.GetString("LastUsername");
            }
        }
    }
}
