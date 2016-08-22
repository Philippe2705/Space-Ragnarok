using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PseudoSave : MonoBehaviour
{


    public InputField playerNameInputServer;
    public InputField playerNameInputClient;

    // Use this for initialization
    void Start()
    {
        playerNameInputServer.onEndEdit.AddListener(OnPseudoChange);
        playerNameInputClient.onEndEdit.AddListener(OnPseudoChange);

        OnPseudoChange(PlayerPrefs.GetString("Pseudo"));
    }

    public void OnPseudoChange(string newPseudo)
    {
        PlayerPrefs.SetString("Pseudo", newPseudo);
        playerNameInputServer.text = newPseudo;
        playerNameInputClient.text = newPseudo;
    }
}
