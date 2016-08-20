using UnityEngine;
using UnityEngine.Networking;

public class Ship : NetworkBehaviour
{
    public GameObject explosion;
    public GameObject bigExplosion;
    public GameObject explosionSound;
    public GameObject smallExplosionSound;
    public GameObject bigExplosionSound;
    public GameObject bulletPrefab;

    public const float MinPosX = -60f, MaxPosX = 60f, MinPosY = -30f, MaxPosY = 30f;
    public const float VitesseMinimum = 0.1f;
    public const float VitesseMaximum = 2;
    public const float ReloadingTime = 3;

    float reloadTimeR;
    float reloadTimeL;
    float explosionTimer = 2;
    bool isExploding = false;
    GameObject[] rightGuns;
    GameObject[] leftGuns;

    int vie = 100;
    float speed = 100f;
    new Rigidbody2D rigidbody2D;
    ShipProperty shipSettings;


    protected virtual void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        var rightGun = transform.FindChild("RightGuns");
        var leftGun = transform.FindChild("LeftGuns");
        var gunCount = rightGun.childCount;
        rightGuns = new GameObject[gunCount];
        leftGuns = new GameObject[gunCount];

        for (int i = 0; i < rightGun.childCount; i++)
        {
            rightGuns[i] = rightGun.transform.GetChild(i).gameObject;
        }
        if (GameObject.Find("NetworkSyncer").GetComponent<PlayerConf>().shipSettings.shipID != 4)  //Les cannons du ragnarok ne se comporte pas pareil
        {
            for (int i = 0; i < leftGun.childCount; i++)
            {
                leftGuns[i] = leftGun.transform.GetChild(i).gameObject;
            }
        }
            shipSettings = GameObject.Find("NetworkSyncer").GetComponent<PlayerConf>().shipSettings;
    }


    /*
     * Update
     */
    void Update()
    {
        if (isServer)
        {
            UpdateServer();
        }
        if (isClient)
        {
            UpdateClient();
        }
    }

    protected virtual void UpdateServer()
    {
        if (reloadTimeR > 0)
        {
            reloadTimeR -= Time.deltaTime;
        }
        if (reloadTimeL > 0)
        {
            reloadTimeL -= Time.deltaTime;
        }
        if (vie <= 0)
        {
            isExploding = true;
            RpcBeginSmallExplosions();
        }
        if (isExploding)
        {
            explosionTimer -= Time.deltaTime;
            if (explosionTimer <= 0)
            {
                RpcBigExplosion();
                RpcEndSmallExplosions();
                NetworkServer.Destroy(gameObject);
            }
        }
    }

    protected virtual void UpdateClient()
    {

    }


    /*
     * Commands
     */
    [Command]
    protected void CmdMove(float vertical, float horizontal)
    {
        vertical = Mathf.Max(0, vertical);

        if (rigidbody2D.velocity.magnitude <= VitesseMaximum)
        {
            rigidbody2D.AddForce(transform.TransformDirection(0, 1, 0) * Time.deltaTime * Mathf.Max(vertical, VitesseMinimum) * speed * shipSettings.speedFactor);
        }

        Vector3 rotation = new Vector3(0, 0, horizontal * 1.5f);
        transform.Rotate(rotation * Time.deltaTime * -15);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, MinPosX, MaxPosX), Mathf.Clamp(transform.position.y, MinPosY, MaxPosY), transform.position.z);
    }


    [Command]
    protected void CmdFire(float fireSide)
    {
        if (fireSide > 0 && reloadTimeR <= 0)
        {
            foreach(var rightGun in rightGuns)
            {
                var bullet = Instantiate(bulletPrefab, rightGun.transform.position, rightGun.transform.rotation) as GameObject;
                bullet.GetComponent<Bullet>().speed = Random.Range(2.7f, 3.3f);
                bullet.GetComponent<Bullet>().direction = rightGun.transform.rotation * Quaternion.Euler(0, Random.Range(-10f, 10f), 0);
                NetworkServer.Spawn(bullet);
            }
            reloadTimeR = ReloadingTime;
        }
        else if (fireSide < 0 && reloadTimeL <= 0)
        {
            foreach(var leftGun in leftGuns)
            {
                var bullet = Instantiate(bulletPrefab, leftGun.transform.position, leftGun.transform.rotation) as GameObject;
                bullet.GetComponent<Bullet>().speed = Random.Range(2.7f, 3.3f);
                bullet.GetComponent<Bullet>().direction = leftGun.transform.rotation * Quaternion.Euler(0, Random.Range(-10f, 10f), 0);
                NetworkServer.Spawn(bullet);
            }
            reloadTimeL = ReloadingTime;
        }
    }

    /*
     * Hit
     */
    [Server]
    public void HitByBullet(Vector3 position, Quaternion rotation)
    {
        vie -= 2 + Random.Range(0, 4);
        print("Touché");
        print("Vie restante : " + vie.ToString());

        RpcHitByBullet(position, rotation);
    }


    /*
     * Rpcs
     */
    [ClientRpc]
    void RpcHitByBullet(Vector3 position, Quaternion rotation)
    {
        Instantiate(explosionSound, position, rotation);
        Instantiate(explosion, position, rotation);
    }

    [ClientRpc]
    void RpcBeginSmallExplosions()
    {
        InvokeRepeating("SmallExplosion", 0, 0.1f);
    }

    [ClientRpc]
    void RpcEndSmallExplosions()
    {
        CancelInvoke("SmallExplosion");
    }

    void SmallExplosion()
    {
        Vector3 randomPos = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), -1);
        Instantiate(explosion, randomPos, transform.rotation);
        Instantiate(smallExplosionSound, randomPos, transform.rotation);
    }

    [ClientRpc]
    void RpcBigExplosion()
    {
        Instantiate(bigExplosion, new Vector3(transform.position.x, transform.position.z, -1), transform.rotation);
        Instantiate(bigExplosionSound, transform.position, transform.rotation);
        print(gameObject.name + "A été détruit");
    }

    /*
     * Gets
     */
    protected float GetVie()
    {
        return vie;
    }
    public void rotateGuns(Vector2 direction) //Used for ragnarok only
    {
        if (GameObject.Find("NetworkSyncer").GetComponent<PlayerConf>().shipSettings.shipID == 4)
        {

            foreach (var rightGun in rightGuns)
            {
                rightGun.transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}