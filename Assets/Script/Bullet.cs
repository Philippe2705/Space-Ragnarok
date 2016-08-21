using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{
    public GameObject ExplosionMobile;

    [SyncVar]
    public float speed;
    [SyncVar]
    public Quaternion direction;
    [SyncVar]
    public Color color;

    //Server
    public float damage;

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
        transform.position -= Vector3.forward * transform.position.z;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!isServer)
        {
            return;
        }
        var player = other.gameObject.GetComponent<PlayerShip>();
        var bot = other.gameObject.GetComponent<BotShip>();
        Ship ship;
        if (player != null)
        {
            ship = player;
        }
        else
        {
            ship = bot;
        }
        ship.HitByBullet(transform.position, transform.rotation, damage);
        RpcSpawnExplosion();
        NetworkServer.Destroy(gameObject);
    }

    [ClientRpc]
    void RpcSpawnExplosion()
    {
        Instantiate(ExplosionMobile, transform.position, transform.rotation);
    }
}
