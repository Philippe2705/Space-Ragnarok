using UnityEngine;

public class AutoDestruct : MonoBehaviour
{

    public float timer = 5;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Network.Destroy(gameObject);
        }
    }
}
