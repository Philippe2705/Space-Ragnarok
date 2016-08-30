using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{
    [HideInInspector, SyncVar]
    public float speed;
    [HideInInspector, SyncVar]
    public Quaternion direction;

    //Server
    [HideInInspector]
    public float damage;
    [HideInInspector]
    public string playerName;
    [HideInInspector]
    public string bulletName;

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
        var player = other.collider.gameObject.GetComponent<PlayerShip>();
        var bot = other.collider.gameObject.GetComponent<BotShip>();
        var shield = other.collider.gameObject.GetComponent<Shield>();

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
            ship.HitByBullet(transform.position, transform.rotation, damage, playerName, bulletName);
        }
        else if (shield != null)
        {
            shield.transform.root.GetComponent<NetworkShield>().SendRpcHitByBullet(shield.name);
        }

        NetworkServer.Destroy(gameObject);
    }
}
