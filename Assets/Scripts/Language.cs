using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Language : MonoBehaviour
{
    void Awake()
    {
        UpdateButtons();
        string lang;
        switch (Application.systemLanguage)
        {
            case SystemLanguage.French:
                lang = "french";
                break;
            case SystemLanguage.English:
                lang = "english";
                break;
            case SystemLanguage.Russian:
                lang = "russian";
                break;
            case SystemLanguage.German:
                lang = "german";
                break;
            case SystemLanguage.Spanish:
                lang = "spanish";
                break;
            case SystemLanguage.Portuguese:
                lang = "portuguese";
                break;
            case SystemLanguage.Italian:
                lang = "italian";
                break;
            case SystemLanguage.Korean:
                lang = "korean";
                break;
            default:
                lang = "english";
                break;
        }
        lang = PlayerPrefs.GetString("lang", lang);
        I18N.LoadLanguage(lang);
        transform.GetChild(0).Find("Current Language").GetComponent<Text>().text = lang;
    }

    public void SetLanguage(string lang)
    {
        PlayerPrefs.SetString("lang", lang);
        I18N.LoadLanguage(lang);
        GameObject.Find("Current Language").GetComponent<Text>().text = lang;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void UpdateButtons()
    {
        foreach (var s in new string[] { "english", "french", "spanish", "russian", "german", "portugese", "italian", "korean" })
        {
            var x = s;
            GameObject.Find("Canvas").transform.Find("Settings/SettingsPanels/LanguageWindow/Panel/LanguagesListSubPanel/LanguagesList/" + s).GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayerPrefs.SetString("lang", x);
                I18N.LoadLanguage(x);
                GameObject.Find("Current Language").GetComponent<Text>().text = x;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            });
        }
    }
}