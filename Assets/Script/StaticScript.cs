using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StaticScript : MonoBehaviour
{

    int playerCount;
    int botCount;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("MainMenu");
    }


    void OnLevelWasLoaded()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "MainMenu":
                FindObjectOfType<ServerConf>().BotCountSlider.onValueChanged.AddListener(OnBotCountChanged);
                FindObjectOfType<ServerConf>().ClientCountSlider.onValueChanged.AddListener(OnPlayerCountChanged);
                break;
            case "Battle":
                break;

        }
    }

    public void OnBotCountChanged(float number)
    {
        botCount = (int)number;
    }

    public void OnPlayerCountChanged(float number)
    {
        playerCount = (int)number;
    }
}
