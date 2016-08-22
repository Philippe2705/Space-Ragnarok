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
        float vertical;
        float horizontal;

        var deltaPos = playerShip.transform.position - transform.position;

        if (transform.rotation.eulerAngles.z > Mathf.Atan2(deltaPos.y, deltaPos.x))
        {
            horizontal = 1;
        } else
        {
            horizontal = -1;
        }

        vertical = 0;

        CmdMove(vertical, horizontal);
    }
}
