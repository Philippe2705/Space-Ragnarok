using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{
    public GameObject ExplosionMobile;

    float speed;
    float autoDestruct;


    // Use this for initialization
    void Start()
    {
        if (!isServer)
        {
            return;
        }
        autoDestruct = 25;
        transform.Rotate(0, Random.Range(-10f, 10f), 0);
        speed = Random.Range(0.9f, 1.1f) * 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isServer)
        {
            return;
        }
        autoDestruct -= Time.deltaTime;
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if (autoDestruct <= 0)
        {
            NetworkServer.Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isServer)
        {
            return;
        }
        var player = other.GetComponent<PlayerShip>();
        var bot = other.GetComponent<BotShip>();
        Ship ship;
        if (player != null)
        {
            ship = player;
        }
        else
        {
            ship = bot;
        }
        ship.HitByBullet(transform.position, transform.rotation);
        var explosion = Instantiate(ExplosionMobile, transform.position, transform.rotation) as GameObject;
        NetworkServer.Spawn(explosion);
        NetworkServer.Destroy(gameObject);
    }
}
