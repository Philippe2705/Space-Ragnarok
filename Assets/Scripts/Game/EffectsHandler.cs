using UnityEngine;
using System.Collections;

public class EffectsHandler : MonoBehaviour
{
    new Rigidbody2D rigidbody2D;
    Ship ship;

    Transform playerName;

    ParticleSystem[] smokes;
    ParticleSystem[] motors;
    Transform[] explosions;

    GameObject smallExplosion;
    GameObject bigExplosion;

    public void Start()
    {
        if (rigidbody2D != null)
        {
            return;
        }
        rigidbody2D = GetComponent<Rigidbody2D>();
        ship = GetComponent<PlayerShip>() ?? (Ship)GetComponent<BotShip>();

        Utility.AddChildsToArray(out smokes, "Smokes", transform);
        Utility.AddChildsToArray(out motors, "Motors", transform);
        Utility.AddChildsToArray(out explosions, "Explosions", transform);

        smallExplosion = Resources.Load<GameObject>("Prefabs/Explosions/SmallExplosion");
        bigExplosion = Resources.Load<GameObject>("Prefabs/Explosions/BigExplosion");

        playerName = transform.Find("Player_name");
        SetPlayerName(ship.isLocalPlayer, false, ship.playerControllerId > 0, ship.Pseudo);

        UpdateSmokes(100);
    }

    public void Update()
    {
        /*
         * Motors
         */
        var rate = 50f * Vector3.Project(rigidbody2D.velocity, transform.up).magnitude / (ship.ShipProperty.SpeedFactor * ShipProperties.GetBotProperties(ship.BotLevel).SpeedMultiplier) + 10f;

        foreach (var motor in motors)
        {
            ParticleSystemExtension.SetEmissionRate(motor, rate);
        }
        /*
         * Update pseudo rotation
         */
        if (Camera.main != null)
        {
            playerName.rotation = Camera.main.transform.rotation;
        }
    }

    public void SetPlayerName(bool localPlayer, bool dead, bool isBot, string pseudo)
    {
        playerName.GetComponent<TextMesh>().characterSize = (localPlayer && !dead && !isBot) ? 0 : Constants.PseudoSize;
        playerName.GetComponent<TextMesh>().text = pseudo;
    }

    public void UpdateSmokes(float health)
    {
        foreach (var smoke in smokes)
        {
            ParticleSystemExtension.SetEmissionRate(smoke, (100 - health) / 5 * Constants.SmokeDensity);
        }
    }

    public void BeginSmallExplosions()
    {
        InvokeRepeating("SmallExplosion", 0, Constants.SmallExplosionsRate);
    }

    public void EndSmallExplosions()
    {
        CancelInvoke("SmallExplosion");
    }
    void SmallExplosion()
    {
        var randomPos = explosions[Random.Range(0, explosions.Length)].position;
        Instantiate(smallExplosion, randomPos, transform.rotation);
    }

    public void Explode()
    {
        Instantiate(bigExplosion, transform.position, transform.rotation);
    }

    public void Die()
    {
        GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/Death");
        foreach (var motor in motors)
        {
            motor.EnableEmission(false);
        }
        playerName.GetComponent<TextMesh>().characterSize = Constants.PseudoSize;
    }

    public void HitByBullet(Vector3 position, Quaternion rotation, string bulletName, int shipId)
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Explosions/" + "Class" + ShipProperties.GetClass(shipId).ToString()), position, rotation);
    }
}
