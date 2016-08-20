using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{
    public GameObject ExplosionMobile;

    [SyncVar]
    public float speed;
    [SyncVar]
    public Quaternion direction;

    float autoDestruct;


    // Use this for initialization
    void Start()
    {
        autoDestruct = 25;
        transform.rotation = direction;
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            autoDestruct -= Time.deltaTime;
            if (autoDestruct <= 0)
            {
                NetworkServer.Destroy(gameObject);
            }
        }
        transform.Translate(-transform.up * Time.deltaTime * speed);
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
