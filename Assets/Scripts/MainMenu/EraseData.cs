using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class EraseData : MonoBehaviour
{
    public InputField eraseConfirmation;
    public GameObject confirmPanel;

    public void ShowConfirm(bool show)
    {
        confirmPanel.SetActive(show);
        eraseConfirmation.text = "";
    }

    public void OnConfirm()
    {
        if (eraseConfirmation.text == "Sure")
        {
            UserData.Reset();
            SceneManager.LoadScene(0);
        }
    }
}
