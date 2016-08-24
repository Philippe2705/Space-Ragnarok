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
        HealthBar = FindObjectOfType<HealthBar>();
        GetComponentInChildren<Camera>().enabled = isLocalPlayer;
        GetComponentInChildren<Camera>().tag = isLocalPlayer ? "MainCamera" : "Untagged";
        GetComponentInChildren<AudioListener>().enabled = isLocalPlayer;

        if (isLocalPlayer)
        {
            GetComponentInChildren<Camera>().orthographicSize = ShipProperties.GetShip(ShipId).ViewDistance;
        }
    }

    protected override void UpdateClient()
    {
        base.UpdateClient();
        if (isLocalPlayer)
        {
            /*
             * Update reload sliders
             */
            RightGunReloadingBar.value = 1 - reloadTimeR / shipProperty.ReloadTime;
            LeftGunReloadingBar.value = 1 - reloadTimeL / shipProperty.ReloadTime;

            /*
             * Fire
             */
            var fireSide = CnInputManager.GetAxisRaw("Horizontal1");
            if (Mathf.Abs(fireSide) > 0.2f)
            {
                CmdFire(fireSide);
            }

            /*
             * Move
             */
            float horizontal = -CnInputManager.GetAxis("Horizontal");
            float vertical = CnInputManager.GetAxis("Vertical");
            CmdMove(vertical, horizontal);
        }
    }


    //protected override void OnHealth(float value)
    //{
    //    base.OnHealth(value);
    //    if (isLocalPlayer && HealthBar != null)
    //    {
    //        HealthBar.UpdateHealth(value);
    //    }
    //}
}
