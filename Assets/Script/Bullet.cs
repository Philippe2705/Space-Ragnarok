using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{
    public GameObject explosionPrefab;
    public GameObject explosionSound;

    [HideInInspector, SyncVar]
    public float speed;
    [HideInInspector, SyncVar]
    public Quaternion direction;

    //Server
    [HideInInspector]
    public float damage;
    [HideInInspector]
    public string playerName;

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
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!isServer)
        {
            return;
        }
        var player = other.gameObject.GetComponent<PlayerShip>();
        var bot = other.gameObject.GetComponent<BotShip>();
        Ship ship = null;
        if (player != null)
        {
            ship = player;
        }
        else if (bot != null)
        {
            ship = bot;
        }
        if (ship != null)
        {
            ship.HitByBullet(transform.position, transform.rotation, damage, playerName);
        }
        RpcSpawnExplosion();
        NetworkServer.Destroy(gameObject);
    }

    [ClientRpc]
    void RpcSpawnExplosion()
    {
        Instantiate(explosionSound, transform.position, transform.rotation);
        Instantiate(explosionPrefab, transform.position, transform.rotation);
    }
}
