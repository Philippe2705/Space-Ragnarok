using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicScript : MonoBehaviour
{

    void Start()
    {
       
    }

    void OnLevelWasLoaded()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            Destroy(gameObject);
        }
    }

    void Awake()
    {
        if (FindObjectsOfType<MusicScript>().Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
