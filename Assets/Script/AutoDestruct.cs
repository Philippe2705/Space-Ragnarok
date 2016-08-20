using UnityEngine;
using UnityEngine.Networking;

public class AutoDestruct : NetworkBehaviour
{

    public float timer = 5;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            NetworkServer.Destroy(gameObject);
        }
    }
}
