using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Ship : NetworkBehaviour
{

    public const float MinPosX = -60f, MaxPosX = 60f, MinPosY = -30f, MaxPosY = 30f;
    public const float VitesseMinimum = 0.1f;
    public const float VitesseMaximum = 2;

    public ShipProperty shipProperty;
    public new Rigidbody2D rigidbody2D;

    public bool IsBot;
    public int BotLevel = -1;

    [SyncVar]
    public bool IsDead = false;
    [SyncVar(hook = "OnShipId")]
    public int ShipId;
    [SyncVar(hook = "OnPseudo")]
    public string Pseudo;

    //[SyncVar(hook = "OnHealth")]
    float health = 100;
    [SyncVar]
    protected float reloadTimeR;
    [SyncVar]
    protected float reloadTimeL;

    bool hasUpdatedMinimap;
    float explosionTimer = Constants.ExplosionDurationBeforeDeath;
    bool isExploding = false;
    GameObject[] rightGuns;
    GameObject[] leftGuns;
    GameObject pseudoGO;


    GameObject smallExplosion;
    GameObject bigExplosion;
    GameObject explosionSound;
    GameObject smallExplosionSound;
    GameObject bigExplosionSound;
    GameObject bulletPrefab;
    //ParticleSystem particleSystem;




    protected virtual void Start()
    {
        Invoke("DelayedScoreBoard", 0.1f);
        IsBot = GetType() == typeof(BotShip);
        rigidbody2D = GetComponent<Rigidbody2D>();
        //particleSystem = transform.Find("Smoke").GetComponent<ParticleSystem>();
        /*
         * Prefabs
         */
        smallExplosion = Resources.Load<GameObject>("Prefabs/SmallExplosion");
        bigExplosion = Resources.Load<GameObject>("Prefabs/BigExplosion");
        explosionSound = Resources.Load<GameObject>("Prefabs/ExplosionSound");
        smallExplosionSound = Resources.Load<GameObject>("Prefabs/SmallExplosionSound");
        bigExplosionSound = Resources.Load<GameObject>("Prefabs/BigExplosionSound");
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
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
        if (isExploding && explosionTimer > 0)
        {
            explosionTimer -= Time.deltaTime;
            if (explosionTimer <= 0)
            {
                Die();
            }
        }
    }

    protected virtual void UpdateClient()
    {
        /*
         * Update minimap
         */
        if (!hasUpdatedMinimap)
        {
            CmdUpdateMinimap();
        }

        /*
         * Update pseudo rotation
         */
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
        if (IsDead)
        {
            return;
        }

        horizontal = Mathf.Clamp(horizontal, -1, 1);
        vertical = Mathf.Clamp(vertical, -1, 1);
        /*
         * Rotate along Z axis 
         */
        rigidbody2D.angularVelocity = horizontal * shipProperty.TurnRate * ShipProperties.GetBotProperties(BotLevel).TurnRateMultiplier;

        /*
         * Move ship
         */
        rigidbody2D.velocity = transform.up * Mathf.Max(vertical, VitesseMinimum) * shipProperty.SpeedFactor * ShipProperties.GetBotProperties(BotLevel).SpeedMultiplier;
    }


    [Command]
    protected void CmdFire(float fireSide)
    {
        if (IsDead)
        {
            return;
        }
        GameObject[] guns = new GameObject[0];

        if (fireSide > 0 && reloadTimeR <= 0)
        {
            guns = rightGuns;
            reloadTimeR = shipProperty.ReloadTime;
        }
        else if (fireSide < 0 && reloadTimeL <= 0)
        {
            guns = leftGuns;
            reloadTimeL = shipProperty.ReloadTime;
        }

        foreach (var gun in guns)
        {
            //Create bullet
            var bullet = Instantiate(bulletPrefab, gun.transform.position, gun.transform.rotation) as GameObject;
            bullet.GetComponent<Bullet>().speed = Random.Range(2.7f, 3.3f);
            bullet.GetComponent<Bullet>().direction = gun.transform.rotation * Quaternion.Euler(0, Random.Range(-shipProperty.BulletDispersion, shipProperty.BulletDispersion), 0);
            bullet.GetComponent<Bullet>().color = shipProperty.MaterialColor;
            bullet.GetComponent<Bullet>().damage = shipProperty.Damage * ShipProperties.GetBotProperties(BotLevel).DamageMultiplier;
            bullet.GetComponent<Bullet>().playerName = Pseudo;
            NetworkServer.Spawn(bullet);
        }
    }

    [Command]
    void CmdUpdateMinimap()
    {
        RpcUpdateMinimap();
    }

    /*
     * Hit
     */
    [Server]
    public void HitByBullet(Vector3 position, Quaternion rotation, float damage, string playerName)
    {
        if (!IsDead && playerName != Pseudo)
        {
            health -= damage * Random.Range(1 - Constants.DamageDispersion, 1 + Constants.DamageDispersion) / (shipProperty.Armor * ShipProperties.GetBotProperties(BotLevel).ArmorMultiplier);


            if (health <= 0)
            {
                BeginDeath();
            }


            RpcAddXpToPlayer(playerName, health <= 0, Pseudo);
        }
        //Hit effect
        RpcHitByBullet(position, rotation);
    }

    void BeginDeath()
    {
        IsDead = true;
        isExploding = true;
        RpcBeginSmallExplosions();
    }

    void Die()
    {
        IsDead = true;
        RpcDie();
        int shipsLeft = 0;
        foreach (var playerShip in FindObjectsOfType<PlayerShip>())
        {
            if (!playerShip.IsDead)
            {
                shipsLeft++;
            }
        }
        foreach (var botShip in FindObjectsOfType<BotShip>())
        {
            if (!botShip.IsDead)
            {
                shipsLeft++;
                break;
            }
        }
        if (shipsLeft <= 1)
        {
            FindObjectOfType<ScoreBoard>().ShowScoreBoardOnAll(true);
        }
    }


    /*
     * Rpcs
     */

    [ClientRpc]
    protected void RpcUpdateMinimap()
    {
        FindObjectOfType<MinimapSync>().SearchForPlayers();
        hasUpdatedMinimap = true;
    }

    [ClientRpc]
    void RpcHitByBullet(Vector3 position, Quaternion rotation)
    {
        Instantiate(explosionSound, position, rotation);
        Instantiate(smallExplosion, position, rotation);
    }

    [ClientRpc]
    void RpcBeginSmallExplosions()
    {
        InvokeRepeating("SmallExplosion", 0, 0.1f);
    }

    void SmallExplosion()
    {
        Vector3 randomPos = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(-0.5f, 0.5f), -1);
        Instantiate(smallExplosion, randomPos, transform.rotation);
        Instantiate(smallExplosionSound, randomPos, transform.rotation);
    }

    [ClientRpc]
    void RpcDie()
    {
        CancelInvoke("SmallExplosion");
        Instantiate(bigExplosion, transform.position, transform.rotation);
        Instantiate(bigExplosionSound, transform.position, transform.rotation);

        GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/Death");

        foreach (var s in GetComponentsInChildren<ParticleSystemRenderer>())
        {
            s.enabled = false;
        }
        pseudoGO = transform.FindChild("Player_name").gameObject;
        pseudoGO.GetComponent<TextMesh>().text = Pseudo;
        pseudoGO.GetComponent<TextMesh>().characterSize = Constants.PseudoSize;
    }

    [ClientRpc]
    void RpcAddXpToPlayer(string playerName, bool death, string playerHit)
    {
        List<Ship> ships = new List<Ship>(FindObjectsOfType<PlayerShip>());
        ships.AddRange(FindObjectsOfType<BotShip>());
        foreach (var ship in ships)
        {
            if ((ship.isLocalPlayer || ship.IsBot) && ship.Pseudo == playerName)
            {
                if (isLocalPlayer)
                {
                    var xp = death ? Constants.XpForKill : 0 + Constants.XpForHit;
                    //UserData.AddExperience(xp);
                }
                FindObjectOfType<ScoreBoard>().AddHit(ship.Pseudo);
                if (death)
                {
                    FindObjectOfType<ScoreBoard>().AddKill(ship.Pseudo, playerHit);
                }
            }
        }
    }

    /*
     * Hooks
     */
    void OnPseudo(string value)
    {
        pseudoGO = transform.FindChild("Player_name").gameObject;
        pseudoGO.GetComponent<TextMesh>().text = value;
        pseudoGO.GetComponent<TextMesh>().characterSize = isLocalPlayer ? 0f : Constants.PseudoSize;
    }

    void OnShipId(int value)
    {
        shipProperty = ShipProperties.GetShip(value);
    }

    //protected virtual void OnHealth(float value)
    //{
    //    particleSystem.emissionRate = (100 - value) * 250;
    //}


    void DelayedScoreBoard()
    {
        if (isServer)
        {
            FindObjectOfType<ScoreBoard>().AddPlayerOnAll(Pseudo);
        }
    }
}