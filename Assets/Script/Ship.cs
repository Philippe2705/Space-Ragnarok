using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;

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

    public ShipProperty shipProperty;

    [SyncVar(hook = "UpdateShipId")]
    public int ShipId;
    [SyncVar(hook = "UpdatePseudo")]
    public string Pseudo;


    float reloadTimeR;
    float reloadTimeL;
    float explosionTimer = 5;
    bool isDead = false;
    GameObject[] rightGuns;
    GameObject[] leftGuns;
    GameObject pseudoGO;

    new Rigidbody2D rigidbody2D;


    float vie = 100;


    protected virtual void Start()
    {
        pseudoGO = transform.FindChild("Player_name").gameObject;
        rigidbody2D = GetComponent<Rigidbody2D>();
        /*
         * Guns
         */
        var rightGun = transform.FindChild("RightGuns");
        var leftGun = transform.FindChild("LeftGuns");
        rightGuns = new GameObject[rightGun.childCount];
        leftGuns = new GameObject[leftGun.childCount];

        for (int i = 0; i < rightGun.childCount; i++)
        {
            rightGuns[i] = rightGun.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < leftGun.childCount; i++)
        {
            leftGuns[i] = leftGun.transform.GetChild(i).gameObject;
        }
        if (isServer && GetType() == typeof(BotShip))
        {
            CmdUpdatePseudoAndShipId("Bot", -1);
        }
    }

    public override void OnStartClient()
    {
        if (!isLocalPlayer)
        {
            SetDirtyBit(syncVarDirtyBits);
        }
    }


    /*
     * Update
     */
    void FixedUpdate()
    {
        if (shipProperty.Armor == 0) //Update needed
        {
            UpdatePseudo(Pseudo);
            UpdateShipId(ShipId);
        }
        if (isServer)
        {
            UpdateServer();
        }
        if (isClient)
        {
            UpdateClient();
        }
        //Borders
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, MinPosX, MaxPosX), Mathf.Clamp(transform.position.y, MinPosY, MaxPosY), transform.position.z);
    }

    protected virtual void UpdateServer()
    {
        /*
         * Update reloads times
         */
        if (reloadTimeR > 0)
        {
            reloadTimeR -= Time.deltaTime;
        }
        if (reloadTimeL > 0)
        {
            reloadTimeL -= Time.deltaTime;
        }
        /*
         * Death
         */
        if (vie <= 0 && !isDead)
        {
            isDead = true;
            RpcBeginSmallExplosions();
        }
        if (isDead && explosionTimer > 0)
        {
            explosionTimer -= Time.deltaTime;
            if (explosionTimer <= 0)
            {
                RpcBigExplosion();
                RpcEndSmallExplosions();
                RpcDie();
            }
        }
    }

    protected virtual void UpdateClient()
    {
        if (pseudoGO != null && Camera.main != null)
        {
            pseudoGO.transform.rotation = Camera.main.transform.rotation;
        }
    }


    /*
     * Commands
     */
    [Command]
    protected void CmdMove(float vertical, float horizontal)
    {
        if (isDead)
        {
            return;
        }
        /*
         * Rotate along Z axis 
         */
        rigidbody2D.angularVelocity = horizontal * -15 * shipProperty.TurnRate;

        /*
         * Move ship
         */
        rigidbody2D.velocity = transform.up * Mathf.Max(vertical, VitesseMinimum) * shipProperty.SpeedFactor;
    }


    [Command]
    protected void CmdFire(float fireSide)
    {
        if (isDead)
        {
            return;
        }
        GameObject[] guns = new GameObject[0];

        if (fireSide > 0 && reloadTimeR <= 0)
        {
            guns = rightGuns;
            reloadTimeR = ReloadingTime;
        }
        else if (fireSide < 0 && reloadTimeL <= 0)
        {
            guns = leftGuns;
            reloadTimeL = ReloadingTime;
        }

        foreach (var gun in guns)
        {
            //Create bullet
            var bullet = Instantiate(bulletPrefab, gun.transform.position, gun.transform.rotation) as GameObject;
            bullet.GetComponent<Bullet>().speed = Random.Range(2.7f, 3.3f);
            bullet.GetComponent<Bullet>().direction = gun.transform.rotation * Quaternion.Euler(0, Random.Range(-10f, 10f), 0);
            bullet.GetComponent<Bullet>().color = shipProperty.Color;
            bullet.GetComponent<Bullet>().damage = shipProperty.Damage;
            NetworkServer.Spawn(bullet);
        }
    }

    [Command]
    protected void CmdUpdatePseudoAndShipId(string pseudo, int shipId)
    {
        Pseudo = pseudo;
        ShipId = shipId;

        RpcUpdateMinimap();
    }

    /*
     * Hit
     */
    [Server]
    public void HitByBullet(Vector3 position, Quaternion rotation, float damage)
    {
        vie -= damage * Random.Range(0.5f, 2) / shipProperty.Armor;
        print(Pseudo + " touché");
        print("Vie restante pour " + Pseudo + " : " + vie.ToString());

        //Hit effect
        RpcHitByBullet(position, rotation);
    }


    /*
     * Rpcs
     */

    [ClientRpc]
    void RpcUpdateMinimap()
    {
        FindObjectOfType<MinimapSync>().SearchForPlayers();
    }

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
        Vector3 randomPos = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(-0.5f, 0.5f), -1);
        Instantiate(explosion, randomPos, transform.rotation);
        Instantiate(smallExplosionSound, randomPos, transform.rotation);
    }

    [ClientRpc]
    void RpcBigExplosion()
    {
        Instantiate(bigExplosion, transform.position, transform.rotation);
        Instantiate(bigExplosionSound, transform.position, transform.rotation);
        print(gameObject.name + "A été détruit");
        if (!isLocalPlayer && shipProperty.ShipID != -1) //Pas le joueur tué, ni le bot
        {
            Experience.AddExperience(1);
        }
    }

    [ClientRpc]
    void RpcDie()
    {
        GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/Death");
        foreach (var s in GetComponentsInChildren<ParticleSystemRenderer>())
        {
            s.enabled = false;
        }
        if (!(isServer && GetType() != typeof(BotShip)) && !isLocalPlayer)
        {
            Experience.AddExperience(1);
        }
    }

    /*
     * Gets
     */
    protected float GetVie()
    {
        return vie;
    }

    /*
     * Hooks
     */
    void UpdatePseudo(string value)
    {
        pseudoGO.GetComponent<TextMesh>().text = value;
        pseudoGO.GetComponent<TextMesh>().characterSize = isLocalPlayer ? 0f : 0.01f;
    }

    void UpdateShipId(int value)
    {
        shipProperty = ShipProperties.GetShip(value);
    }
}