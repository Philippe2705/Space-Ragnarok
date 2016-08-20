using UnityEngine;
using UnityEngine.Networking;

public class AutoDestruct : MonoBehaviour
{

    public float timer = 5;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
