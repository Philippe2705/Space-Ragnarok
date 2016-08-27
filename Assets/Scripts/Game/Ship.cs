using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Ship : NetworkBehaviour
{

    public const float MinPosX = -60f, MaxPosX = 60f, MinPosY = -30f, MaxPosY = 30f;

    [HideInInspector]
    public ShipProperty shipProperty;
    [HideInInspector]
    public new Rigidbody2D rigidbody2D;

    [HideInInspector, SyncVar]
    public bool IsBot;
    [HideInInspector, SyncVar]
    public int BotLevel = -1;
    [HideInInspector, SyncVar]
    public bool IsDead = false;
    [HideInInspector, SyncVar(hook = "OnShipId")]
    public int ShipId;
    [HideInInspector, SyncVar(hook = "OnPseudo")]
    public string Pseudo;

    [SyncVar(hook = "OnHealth")]
    float health = 100;

    bool hasUpdatedMinimap;
    float explosionTimer = Constants.ExplosionDurationBeforeDeath;
    bool isExploding = false;

    Transform playerName;
    Transform[] guns;
    ParticleSystem[] smokes;
    ParticleSystem[] motors;
    Transform[] explosions;


    GameObject smallExplosion;
    GameObject bigExplosion;
    GameObject smallExplosionSound;
    GameObject bigExplosionSound;


    float reloadTime;



    protected virtual void Start()
    {
        Invoke("DelayedScoreBoard", 1f);
        IsBot = GetType() == typeof(BotShip);
        rigidbody2D = GetComponent<Rigidbody2D>();

        playerName = transform.FindChild("Player_name");

        guns = new Transform[transform.FindChild("Guns").childCount];
        for (int i = 0; i < transform.FindChild("Guns").childCount; i++)
        {
            guns[i] = transform.FindChild("Guns").GetChild(i);
        }

        smokes = new ParticleSystem[transform.FindChild("Smokes").childCount];
        for (int i = 0; i < transform.FindChild("Smokes").childCount; i++)
        {
            smokes[i] = transform.FindChild("Smokes").GetChild(i).GetComponent<ParticleSystem>();
        }

        motors = new ParticleSystem[transform.FindChild("Motors").childCount];
        for (int i = 0; i < transform.FindChild("Motors").childCount; i++)
        {
            motors[i] = transform.FindChild("Motors").GetChild(i).GetComponent<ParticleSystem>();
        }

        explosions = new Transform[transform.FindChild("Explosions").childCount];
        for (int i = 0; i < transform.FindChild("Explosions").childCount; i++)
        {
            explosions[i] = transform.FindChild("Explosions").GetChild(i);
        }

        /*
         * Prefabs
         */
        smallExplosion = Resources.Load<GameObject>("Prefabs/Sounds/SmallExplosion");
        bigExplosion = Resources.Load<GameObject>("Prefabs/Sounds/BigExplosion");
        smallExplosionSound = Resources.Load<GameObject>("Prefabs/Sounds/SmallExplosionSound");
        bigExplosionSound = Resources.Load<GameObject>("Prefabs/Sounds/BigExplosionSound");

        OnHealth(health);
    }

    public override void OnStartClient()
    {
        if (!isLocalPlayer)
        {
            SetDirtyBit(syncVarDirtyBits);
            syncVarHookGuard = false;
        }
    }

    /*
     * Update
     */
    void FixedUpdate()
    {
        if (isServer)
        {
            FixedUpdateServer();
        }
        if (isClient)
        {
            FixedUpdateClient();
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, MinPosX, MaxPosX), Mathf.Clamp(transform.position.y, MinPosY, MaxPosY), transform.position.z);
    }

    protected virtual void FixedUpdateServer()
    {

    }

    protected virtual void FixedUpdateClient()
    {

    }

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
        /*
         * Temp
         */
        reloadTime -= Time.deltaTime;

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
        if (playerName != null && Camera.main != null)
        {
            playerName.transform.rotation = Camera.main.transform.rotation;
        }
        /*
         * Effects
         */
        var rate = 50f * Vector3.Project(rigidbody2D.velocity, transform.up).magnitude / (shipProperty.SpeedFactor * ShipProperties.GetBotProperties(BotLevel).SpeedMultiplier) + 10f;

        foreach (var motor in motors)
        {
            ParticleSystemExtension.SetEmissionRate(motor, rate);
        }
    }


    /*
     * Commands
     */
    [Command]
    void CmdRespawnPlayer()
    {
        var player = Instantiate(NetworkManager.singleton.playerPrefab) as GameObject;
        player.GetComponent<SpawnPlayer>().Pseudo = Pseudo;
        player.GetComponent<SpawnPlayer>().ShipId = ShipId;
        player.GetComponent<SpawnPlayer>().BotLevel = BotLevel;
        player.GetComponent<SpawnPlayer>().IsBot = IsBot;
        NetworkServer.ReplacePlayerForConnection(connectionToClient, player, playerControllerId);
        if (!IsDead)
        {
            NetworkServer.Destroy(gameObject);
        }

    }

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
        rigidbody2D.velocity = transform.up * Mathf.Max(vertical, shipProperty.MinSpeed) * shipProperty.SpeedFactor * ShipProperties.GetBotProperties(BotLevel).SpeedMultiplier;
    }


    [Command]
    protected void CmdFire(Vector2 fireVector)
    {
        if (IsDead)
        {
            return;
        }

        if (reloadTime < 0 && fireVector.magnitude > Constants.FireTrigger)
        {
            reloadTime = 2;

            var direction = guns[0].parent.InverseTransformVector(transform.TransformVector(fireVector));

            foreach (var gun in guns)
            {
                print(Vector2.Angle(direction, gun.right));
                if (Vector2.Angle(direction, gun.right) < shipProperty.FireAngleTolerance)
                {
                    //Create bullet
                    var bullet = Instantiate(shipProperty.BulletPrefab, gun.transform.position, Quaternion.identity) as GameObject;
                    bullet.GetComponent<Bullet>().speed = shipProperty.BulletSpeed * Random.Range(1 - Constants.BulletSpeedDispersion, 1 + Constants.BulletSpeedDispersion);
                    bullet.GetComponent<Bullet>().direction = gun.transform.rotation * Quaternion.Euler(0, 0, Random.Range(-shipProperty.BulletDispersion, shipProperty.BulletDispersion));
                    bullet.GetComponent<Bullet>().damage = shipProperty.Damage * ShipProperties.GetBotProperties(BotLevel).DamageMultiplier;
                    bullet.GetComponent<Bullet>().playerName = Pseudo;
                    NetworkServer.Spawn(bullet);
                }
            }
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
            FindObjectOfType<ScoreBoard>().InvokeShowScoreBoardOnAll(Constants.TimeBeforeScoreBoardShows);
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
    void RpcBeginSmallExplosions()
    {
        InvokeRepeating("SmallExplosion", 0, 0.25f);
    }

    void SmallExplosion()
    {
        var randomPos = explosions[Random.Range(0, explosions.Length)].position;
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

        foreach (var motor in motors)
        {
            motor.EnableEmission(false);
        }
        playerName.GetComponent<TextMesh>().text = Pseudo;
        playerName.GetComponent<TextMesh>().characterSize = Constants.PseudoSize;
    }

    [ClientRpc]
    void RpcAddXpToPlayer(string playerName, bool death, string playerHit)
    {
        List<Ship> ships = new List<Ship>(FindObjectsOfType<PlayerShip>());
        ships.AddRange(FindObjectsOfType<BotShip>());
        foreach (var ship in ships)
        {
            if (ship.Pseudo == playerName)
            {
                if (isLocalPlayer)
                {
                    UserData.AddHit();
                    if (death)
                    {
                        UserData.AddKill();
                    }
                }
                if (isServer)
                {
                    FindObjectOfType<ScoreBoard>().AddHit(ship.Pseudo);
                    if (death)
                    {
                        FindObjectOfType<ScoreBoard>().AddKill(ship.Pseudo, playerHit);
                    }
                }
            }
        }
    }

    /*
     * Hooks
     */
    void OnPseudo(string value)
    {
        playerName = transform.FindChild("Player_name");
        playerName.GetComponent<TextMesh>().text = value;
        playerName.GetComponent<TextMesh>().characterSize = isLocalPlayer ? 0f : Constants.PseudoSize;
    }

    void OnShipId(int value)
    {
        shipProperty = ShipProperties.GetShip(value);
    }

    void OnHealth(float value)
    {
        foreach (var smoke in smokes)
        {
            ParticleSystemExtension.SetEmissionRate(smoke, (100 - value) / 5 * Constants.SmokeDensity);
        }
        OnHealthDelegate(value);
    }

    protected virtual void OnHealthDelegate(float value)
    {

    }


    void DelayedScoreBoard()
    {
        if (isServer)
        {
            FindObjectOfType<ScoreBoard>().AddPlayerOnAll(Pseudo);
        }
    }

    public void Respawn()
    {
        CmdRespawnPlayer();
    }
}