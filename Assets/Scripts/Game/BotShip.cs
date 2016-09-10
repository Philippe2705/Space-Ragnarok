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
        UpdatePlayers();
        InvokeRepeating("UpdatePlayers", 0, 1);
    }


    protected override void FixedUpdateServer()
    {
        base.FixedUpdateServer();
        fireSide = 0;
        horizontal = 0;
        vertical = 0;

        GetNearestPlayer();
        if (currentPlayer != null && !currentPlayer.IsDead)
        {
            IA();
        }
        CmdMove(vertical, horizontal);
        Fire(fireSide * Vector2.right);
    }

    void IA()
    {
        vertical = 1;
        fireSide = 0;

        var deltaPos = currentPlayer.transform.position - transform.position;

        var forwardAngle = Vector3.Angle(transform.up, deltaPos) * Mathf.Sign(Vector3.Dot(Vector3.Cross(transform.up, deltaPos), Vector3.forward));
        var rightAngle = Vector3.Angle(transform.right, deltaPos) * Mathf.Sign(Vector3.Dot(Vector3.Cross(transform.right, deltaPos), Vector3.forward));
        var leftAngle = Vector3.Angle(-transform.right, deltaPos) * Mathf.Sign(Vector3.Dot(Vector3.Cross(-transform.right, deltaPos), Vector3.forward));


        if (deltaPos.magnitude < 30 && currentPlayer.rigidbody2D != null)
        {
            deltaPos += currentPlayer.transform.up * currentPlayer.rigidbody2D.velocity.magnitude / 3 * deltaPos.magnitude;
        }


        if (deltaPos.magnitude > 15)
        {
            var angle = forwardAngle;

            horizontal = angle / ShipProperty.TurnRate / Time.fixedDeltaTime;
        }
        else
        {

            var angle = Mathf.Min(deltaPos.magnitude > 5f ? forwardAngle : Mathf.Infinity, Mathf.Min(rightAngle, leftAngle));

            horizontal = angle / ShipProperty.TurnRate / Time.fixedDeltaTime;
        }

        if (Vector3.Angle(transform.right, deltaPos) < Vector3.Angle(-transform.right, deltaPos))
        {
            if (Vector3.Angle(transform.right, deltaPos) < 10)
            {
                fireSide = 1;
            }
        }
        else
        {
            if (Vector3.Angle(-transform.right, deltaPos) < 10)
            {
                fireSide = -1;
            }
        }
    }

    void GetNearestPlayer()
    {
        if (currentPlayer == null)
        {
            UpdatePlayers();
            if (players.Length > 0)
            {
                currentPlayer = players[0];
            }
        }
        if (currentPlayer != null)
        {
            PlayerShip nearestPlayer = currentPlayer;
            foreach (var ps in players)
            {
                if (!ps.IsDead && ps != null)
                {
                    if (Vector3.Distance(ps.transform.position, transform.position) < Vector3.Distance(nearestPlayer.transform.position, transform.position) || nearestPlayer.IsDead)
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
    }

    public void UpdatePlayers()
    {
        players = FindObjectsOfType<PlayerShip>();
    }
}
