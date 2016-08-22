using UnityEngine;

public class BotShip : Ship
{
    PlayerShip playerShip;

    protected override void Start()
    {
        base.Start();
        playerShip = FindObjectOfType<PlayerShip>();
    }

    protected override void UpdateServer()
    {
        base.UpdateServer();
        float vertical = 1;
        float horizontal;
        float fireSide = 0;

        var deltaPos = playerShip.transform.position - transform.position;

        var forwardAngle = Vector3.Angle(transform.up, deltaPos) * Mathf.Sign(Vector3.Dot(Vector3.Cross(transform.up, deltaPos), Vector3.forward));
        var rightAngle = Vector3.Angle(transform.right, deltaPos) * Mathf.Sign(Vector3.Dot(Vector3.Cross(transform.right, deltaPos), Vector3.forward));
        var leftAngle = Vector3.Angle(-transform.right, deltaPos) * Mathf.Sign(Vector3.Dot(Vector3.Cross(-transform.right, deltaPos), Vector3.forward));


        if (deltaPos.magnitude < 15)
        {
            deltaPos += playerShip.transform.up * playerShip.rigidbody2D.velocity.magnitude / 3 * deltaPos.magnitude;
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


        CmdMove(vertical, horizontal);
        CmdFire(fireSide);
    }
}
