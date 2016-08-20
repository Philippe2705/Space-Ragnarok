using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using CnControls;

public class PlayerShip : Ship
{
    Slider RightGunReloadingBar;
    Slider LeftGunReloadingBar;
    HealthBar HealthBar;

    protected override void Start()
    {
        base.Start();
        RightGunReloadingBar = GameObject.Find("RightReloading").GetComponent<Slider>();
        LeftGunReloadingBar = GameObject.Find("LeftReloading").GetComponent<Slider>();
        HealthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        GetComponentInChildren<Camera>().enabled = isLocalPlayer;
    }

    protected override void UpdateClient()
    {
        base.UpdateClient();
        if (isLocalPlayer)
        {
            RightGunReloadingBar.value = 1 - GetReloadTimeR() / ReloadingTime;
            LeftGunReloadingBar.value = 1 - GetReloadTimeL() / ReloadingTime;

            var fireSide = CnInputManager.GetAxisRaw("Horizontal1");
            if (Mathf.Abs(fireSide) > 0.2f)
            {
                CmdFire(fireSide);
            }
            float horizontal = CnInputManager.GetAxis("Horizontal");
            float vertical = CnInputManager.GetAxis("Vertical");
            CmdMove(vertical, horizontal);
        }
    }

    protected override void UpdateServer()
    {
        base.UpdateServer();
        RpcUpdateHealthUI(GetVie());
    }

    [ClientRpc]
    void RpcUpdateHealthUI(float vie)
    {
        HealthBar.UpdateVie(vie);
    }
}
