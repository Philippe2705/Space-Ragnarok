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

        if (transform.rotation.eulerAngles.z < playerShip.transform.rotation.eulerAngles.z)
        {
            horizontal = 1;
        } else
        {
            horizontal = -1;
        }

        vertical = 1;
        horizontal = 1;

        CmdMove(vertical, horizontal);
    }
}
