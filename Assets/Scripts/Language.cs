using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Language : MonoBehaviour
{
    public Text languageText;

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
        languageText.text = lang;
    }

    void OnLevelWasLoaded()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            UpdateButtons();
        }
    }

    public void SetLanguage(string lang)
    {
        PlayerPrefs.SetString("lang", lang);
        I18N.LoadLanguage(lang);
        languageText.text = lang;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void UpdateButtons()
    {
        foreach (var s in new string[] { "english", "french", "spanish", "russian", "german", "portugese", "italian", "korean" })
        {
            var x = s;
            GameObject.Find("Canvas").transform.Find("Settings/SettingsPanels/LanguageWindow/Panel/LanguagesListSubPanel/LanguagesList/" + s).GetComponent<Button>().onClick.AddListener(() => { SetLanguage(x); });
        }
    }
}