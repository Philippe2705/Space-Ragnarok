using UnityEngine;

public class BotShip : Ship
{
    PlayerShip[] players;
    PlayerShip currentPlayer;

    const float nearestPlayerTrigger = 10;

    float horizontal, vertical, fireSide;

    protected override void Start()
    {
        base.Start();
        GetComponentInChildren<Camera>().enabled = false;
        GetComponentInChildren<AudioListener>().enabled = false;
        UpdatePlayers();
        currentPlayer = players[0];
        InvokeRepeating("UpdatePlayers", 0, 1);
    }


    protected override void UpdateServer()
    {
        base.UpdateServer();
        fireSide = 0;
        horizontal = 0;
        vertical = 0;

        GetNearestPlayer();
        if (currentPlayer != null && !currentPlayer.IsDead)
        {
            IA();
        }
        CmdMove(vertical, horizontal);
        CmdFire(fireSide);
    }

    void IA()
    {
        vertical = 1;
        fireSide = 0;

        var deltaPos = currentPlayer.transform.position - transform.position;

        var forwardAngle = Vector3.Angle(transform.up, deltaPos) * Mathf.Sign(Vector3.Dot(Vector3.Cross(transform.up, deltaPos), Vector3.forward));
        var rightAngle = Vector3.Angle(transform.right, deltaPos) * Mathf.Sign(Vector3.Dot(Vector3.Cross(transform.right, deltaPos), Vector3.forward));
        var leftAngle = Vector3.Angle(-transform.right, deltaPos) * Mathf.Sign(Vector3.Dot(Vector3.Cross(-transform.right, deltaPos), Vector3.forward));


        if (deltaPos.magnitude < 15)
        {
            deltaPos += currentPlayer.transform.up * currentPlayer.rigidbody2D.velocity.magnitude / 3 * deltaPos.magnitude;
        }


        if (deltaPos.magnitude > 5)
        {
            var angle = forwardAngle;

            horizontal = angle / shipProperty.TurnRate / Time.fixedDeltaTime;
        }
        else
        {

            var angle = Mathf.Min(deltaPos.magnitude > 2.5f ? forwardAngle : Mathf.Infinity, Mathf.Min(rightAngle, leftAngle));

            horizontal = angle / shipProperty.TurnRate / Time.fixedDeltaTime;
        }

        if (Vector3.Angle(transform.right, deltaPos) < Vector3.Angle(-transform.right, deltaPos))
        {
            if (Vector3.Angle(transform.right, deltaPos) < 25)
            {
                fireSide = 1;
            }
        }
        else
        {
            if (Vector3.Angle(-transform.right, deltaPos) < 25)
            {
                fireSide = -1;
            }
        }
    }

    void GetNearestPlayer()
    {
        PlayerShip nearestPlayer = currentPlayer;
        foreach (var ps in players)
        {
            if (!ps.IsDead)
            {
                if (Vector3.Distance(ps.transform.position, transform.position) < Vector3.Distance(nearestPlayer.transform.position, transform.position))
                {
                    nearestPlayer = ps;
                }
            }
        }
        if (Vector3.Distance(currentPlayer.transform.position, nearestPlayer.transform.position) > nearestPlayerTrigger)
        {
            currentPlayer = nearestPlayer;
        }
    }

    public void UpdatePlayers()
    {
        players = FindObjectsOfType<PlayerShip>();
    }
}
