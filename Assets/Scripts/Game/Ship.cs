using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Ship : NetworkBehaviour
{

    public const float MinPosX = -60f, MaxPosX = 60f, MinPosY = -30f, MaxPosY = 30f;

    [HideInInspector]
    public ShipProperty ShipProperty;
    [HideInInspector]
    public new Rigidbody2D rigidbody2D;

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


    float[] reloadTimes;
    Transform[] guns;
    EffectsHandler effectsHandler;


    /*
     * Start
     */
    protected virtual void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        Utility.AddChildsToArray(out guns, "Guns", transform);
        reloadTimes = new float[guns.Length];

        if (effectsHandler == null)
        {
            effectsHandler = gameObject.AddComponent<EffectsHandler>();
        }

        OnHealth(health);
        OnPseudo(Pseudo);
        OnShipId(ShipId);
        OnBotLevel(BotLevel);
    }

    protected virtual void OnLevelWasLoaded()
    {
        if (isServer)
        {
            DelayedScoreBoard();
        }
    }

    /*
     * Fixed Update
     */
    private void FixedUpdate()
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
    [Server]
    protected virtual void FixedUpdateServer()
    {

    }
    [Client]
    protected virtual void FixedUpdateClient()
    {

    }

    /*
     * Update
     */
    private void Update()
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
    [Server]
    protected virtual void UpdateServer()
    {
        /*
         * Reload times
         */
        for (var i = 0; i < reloadTimes.Length; i++)
        {
            reloadTimes[i] -= Time.deltaTime;
        }

        /*
         * Death
         */
        if (health <= 0 && !IsDead)
        {
            Explode();
        }
    }
    [Client]
    protected virtual void UpdateClient()
    {
        /*
         * Update minimap
         */
        if (!hasUpdatedMinimap)
        {
            CmdUpdateMinimap();
        }
    }


    /*
     * Commands
     */
    [Command]
    private void CmdRespawnPlayer()
    {
        var player = Instantiate(NetworkManager.singleton.playerPrefab) as GameObject;
        player.GetComponent<SpawnPlayer>().Pseudo = Pseudo;
        player.GetComponent<SpawnPlayer>().ShipId = ShipId;
        player.GetComponent<SpawnPlayer>().BotLevel = BotLevel;
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
        rigidbody2D.angularVelocity = horizontal * ShipProperty.TurnRate * ShipProperties.GetBotProperties(BotLevel).TurnRateMultiplier;

        /*
         * Move ship
         */
        rigidbody2D.velocity = transform.up * Mathf.Max(vertical, ShipProperty.MinSpeed) * ShipProperty.SpeedFactor * ShipProperties.GetBotProperties(BotLevel).SpeedMultiplier;
    }

    [Command]
    protected void CmdFire(Vector2 fireVector)
    {
        if (IsDead)
        {
            return;
        }

        if (fireVector.magnitude > Constants.FireTrigger)
        {
            var direction = transform.TransformVector(fireVector);

            for (int i = 0; i < guns.Length; i++)
            {
                var gun = guns[i];
                if (reloadTimes[i] < 0)
                {
                    if (Vector2.Angle(direction, gun.right) < ShipProperty.FireAngleTolerance)
                    {
                        reloadTimes[i] = ShipProperty.ReloadTime;
                        //Create bullet
                        var bullet = Instantiate(ShipProperty.BulletPrefab, gun.transform.position, Quaternion.identity) as GameObject;
                        bullet.GetComponent<Bullet>().speed = ShipProperty.BulletSpeed * Random.Range(1 - Constants.BulletSpeedDispersion, 1 + Constants.BulletSpeedDispersion);
                        bullet.GetComponent<Bullet>().direction = gun.transform.rotation * Quaternion.Euler(0, 0, Random.Range(-ShipProperty.BulletDispersion, ShipProperty.BulletDispersion));
                        bullet.GetComponent<Bullet>().damage = ShipProperty.Damage * ShipProperties.GetBotProperties(BotLevel).DamageMultiplier;
                        bullet.GetComponent<Bullet>().playerName = Pseudo;
                        bullet.GetComponent<Bullet>().bulletName = ShipProperties.GetShip(ShipId).BulletPrefab.name;
                        NetworkServer.Spawn(bullet);
                    }
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
    public void HitByBullet(Vector3 position, Quaternion rotation, float damage, string playerName, string bulletName)
    {
        if (!IsDead && playerName != Pseudo)
        {
            health -= damage * Random.Range(1 - Constants.DamageDispersion, 1 + Constants.DamageDispersion) / (ShipProperty.Armor * ShipProperties.GetBotProperties(BotLevel).ArmorMultiplier);
            RpcAddXpToPlayer(playerName, health <= 0, Pseudo);
        }
        RpcHit(position, rotation, bulletName);
    }


    /*
     * Rpcs
     */
    [ClientRpc]
    void RpcHit(Vector3 position, Quaternion rotation, string bulletName)
    {
        effectsHandler.HitByBullet(position, rotation, bulletName, ShipId);
    }

    [ClientRpc]
    void RpcUpdateMinimap()
    {
        FindObjectOfType<MinimapSync>().SearchForPlayers();
        hasUpdatedMinimap = true;
    }

    [ClientRpc]
    void RpcStartToExplode()
    {
        effectsHandler.BeginSmallExplosions();
    }

    [ClientRpc]
    void RpcDie()
    {
        effectsHandler.EndSmallExplosions();
        effectsHandler.Die();
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
        if (effectsHandler == null)
        {
            effectsHandler = gameObject.AddComponent<EffectsHandler>();
            effectsHandler.Start();
        }
        effectsHandler.SetPlayerName(isLocalPlayer, IsDead, Pseudo);
        Pseudo = value;
    }

    void OnShipId(int value)
    {
        ShipId = value;
        ShipProperty = ShipProperties.GetShip(value);
        OnShipIdDelegate(value);
    }

    void OnBotLevel(int value)
    {
        BotLevel = value;
    }

    void OnHealth(float value)
    {
        health = value;
        effectsHandler.UpdateSmokes(value);
        OnHealthDelegate(value);
    }

    /*
     * Others
     */
    protected virtual void OnHealthDelegate(float value)
    {

    }

    protected virtual void OnShipIdDelegate(int id)
    {

    }

    [Server]
    void DelayedScoreBoard()
    {
        FindObjectOfType<ScoreBoard>().AddPlayerOnAll(Pseudo);
    }

    public void Respawn()
    {
        CmdRespawnPlayer();
    }


    [Server]
    void Explode()
    {
        IsDead = true;
        RpcStartToExplode();
        Invoke("Die", Constants.ExplosionDurationBeforeDeath);
    }

    [Server]
    void Die()
    {
        RpcDie();
        /*
         * Check if round over
         */
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
}