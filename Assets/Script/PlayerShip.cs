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
            var ss = FindObjectOfType<StaticScript>();
            Pseudo = ss.pseudo;
            ShipId = ss.shipId;
            CmdUpdatePseudoAndShipId(ss.pseudo, ss.shipId);
            GetComponentInChildren<Camera>().orthographicSize = ShipProperties.GetShip(ShipId).ViewDistance;
        }

        if (isServer && NetworkManager.singleton.numPlayers == 1)
        {
            if (isLocalPlayer)
            {
                var bot = ShipProperties.GetShip(ShipId).ShipPrefab;
                var botGO = Instantiate(bot, new Vector3(Random.Range(-60, 60), Random.Range(-30, 30), 0), Quaternion.identity) as GameObject;
                botGO.GetComponent<BotShip>().ShipId = ShipId;
                NetworkServer.Spawn(botGO);
            }
            else
            {
                Destroy(this);
            }
        }
        else if (isServer && NetworkManager.singleton.numPlayers == 2)
        {
            NetworkServer.Destroy(FindObjectOfType<BotShip>().gameObject);
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

    protected override void UpdateServer()
    {
        base.UpdateServer();
        RpcUpdateHealthUIAndReloadTimes(GetVie(), reloadTimeL, reloadTimeL);
    }

    [ClientRpc]
    void RpcUpdateHealthUIAndReloadTimes(float vie, float reloadTimeL, float reloadTimeR)
    {
        if (isLocalPlayer && HealthBar != null)
        {
            HealthBar.UpdateVie(vie);
        }
        if (!isServer)
        {
            this.reloadTimeL = reloadTimeL;
            this.reloadTimeR = reloadTimeR;
        }
    }
}
