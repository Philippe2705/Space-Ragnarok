using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadSceneScript : MonoBehaviour
{

    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
