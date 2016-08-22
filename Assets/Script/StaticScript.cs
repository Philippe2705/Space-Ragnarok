using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StaticScript : MonoBehaviour
{

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
                break;
        }
    }
}
