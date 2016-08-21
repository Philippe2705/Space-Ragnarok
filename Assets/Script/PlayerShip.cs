using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using CnControls;

public class PlayerShip : Ship
{
    Slider RightGunReloadingBar;
    Slider LeftGunReloadingBar;
    HealthBar HealthBar;
    float localReloadTimeR;
    float localReloadTimeL;

    protected override void Start()
    {
        base.Start();
        RightGunReloadingBar = GameObject.Find("RightReloading").GetComponent<Slider>();
        LeftGunReloadingBar = GameObject.Find("LeftReloading").GetComponent<Slider>();
        if (isLocalPlayer)
        {
            HealthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        }
        GetComponentInChildren<Camera>().enabled = isLocalPlayer;
        GetComponentInChildren<AudioListener>().enabled = isLocalPlayer;

        //TODO: Amélorier
        if (isServer && NetworkManager.singleton.numPlayers == 1)
        {
            var bot = ShipProperties.GetShip(-1).ShipPrefab;
            var botGO = Instantiate(bot) as GameObject;
            NetworkServer.Spawn(botGO);
        }
        if (isLocalPlayer)
        {
            var ss = FindObjectOfType<StaticScript>();
            Pseudo = ss.pseudo;
            ShipId = ss.shipId;
            CmdUpdatePseudoAndShipId(ss.pseudo, ss.shipId);
            //Update minimap
        }
        FindObjectOfType<MinimapSync>().SearchForPlayers();
    }

    protected override void UpdateClient()
    {
        base.UpdateClient();
        if (isLocalPlayer)
        {
            /*
             * Update reload sliders
             */
            if (localReloadTimeR > 0)
            {
                localReloadTimeR -= Time.deltaTime;
            }
            if (localReloadTimeL > 0)
            {
                localReloadTimeL -= Time.deltaTime;
            }

            RightGunReloadingBar.value = 1 - localReloadTimeR / ReloadingTime;
            LeftGunReloadingBar.value = 1 - localReloadTimeL / ReloadingTime;

            /*
             * Fire
             */
            var fireSide = CnInputManager.GetAxisRaw("Horizontal1");
            if (Mathf.Abs(fireSide) > 0.2f)
            {
                if (fireSide > 0 && localReloadTimeR <= 0)
                {
                    localReloadTimeR = ReloadingTime;
                }
                else if (fireSide < 0 && localReloadTimeL <= 0)
                {
                    localReloadTimeL = ReloadingTime;
                }
                CmdFire(fireSide);
            }

            /*
             * Move
             */
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
        if (isLocalPlayer && HealthBar != null)
        {
            HealthBar.UpdateVie(vie);
        }
    }
}
