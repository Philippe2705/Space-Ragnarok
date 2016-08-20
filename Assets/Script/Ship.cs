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

    public const float MinPosX = -80f, MaxPosX = 80f, MinPosY = -30f, MaxPosY = 30f;
    public const float VitesseMinimum = 0.1f;
    public const float VitesseMaximum = 2;
    public const float ReloadingTime = 3;

    float reloadTimeR;
    float reloadTimeL;
    float explosionTimer = 2;
    bool isExploding = false;
    GameObject[] rightGuns = new GameObject[7];
    GameObject[] leftGuns = new GameObject[7];

    int vie = 100;
    float speed = 100f;
    new Rigidbody2D rigidbody2D;


    protected virtual void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        var rightGun = transform.FindChild("RightGuns");
        var leftGun = transform.FindChild("LeftGuns");
        for (int i = 0; i < 7; i++)
        {
            rightGuns[i] = rightGun.transform.GetChild(i).gameObject;
            leftGuns[i] = leftGun.transform.GetChild(i).gameObject;
        }
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
            rigidbody2D.AddForce(transform.TransformDirection(0, 1, 0) * Time.deltaTime * Mathf.Max(vertical, VitesseMinimum) * speed);
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
            for (int i = 0; i <= 6; i++)
            {
                var bullet = Instantiate(bulletPrefab, rightGuns[i].transform.position, rightGuns[i].transform.rotation) as GameObject;
                NetworkServer.Spawn(bullet);
            }
            reloadTimeR = ReloadingTime;
        }
        else if (fireSide < 0 && reloadTimeL <= 0)
        {
            for (int i = 0; i <= 6; i++)
            {
                var bullet = Instantiate(bulletPrefab, leftGuns[i].transform.position, leftGuns[i].transform.rotation) as GameObject;
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

        var expSnd = Instantiate(explosionSound, position, rotation) as GameObject;
        var exp = Instantiate(explosion, position, rotation) as GameObject;
        NetworkServer.Spawn(expSnd);
        NetworkServer.Spawn(exp);
    }


    /*
     * Explosions
     */
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
}