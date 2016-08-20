using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using CnControls;

public class PlayerShip : Ship
{
    public GameObject bot;

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
            var botGO = Instantiate(bot) as GameObject;
            NetworkServer.Spawn(botGO);
        }
    }

    protected override void UpdateClient()
    {
        base.UpdateClient();
        if (isLocalPlayer)
        {
            if (localReloadTimeR > 0)
            {
                localReloadTimeR -= Time.deltaTime;
            }
            if (localReloadTimeL > 0)
            {
                localReloadTimeL -= Time.deltaTime;
            }

            if (GameObject.Find("NetworkSyncer").GetComponent<PlayerConf>().shipSettings.shipID != 4) //Les cannons du ragnarok ne se comporte pas pareil
            {

                RightGunReloadingBar.value = 1 - localReloadTimeR / ReloadingTime;
                LeftGunReloadingBar.value = 1 - localReloadTimeL / ReloadingTime;
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
                float horizontal = CnInputManager.GetAxis("Horizontal");
                float vertical = CnInputManager.GetAxis("Vertical");
                CmdMove(vertical, horizontal);
            }
            else
            {
                GameObject.Find("JoystickRight").GetComponent<SimpleJoystick>().JoystickMoveAxis = ControlMovementDirection.Both;
                Vector2 directionShoot = new Vector2(CnInputManager.GetAxis("Horizontal1"), CnInputManager.GetAxis("Vertical1"));
                if (directionShoot != Vector2.zero)
                {
                    rotateGuns(directionShoot);
                    CmdFire(1); //C'est bugué mais il y a de l'idée --> Le ship s'auto hit et tir dans le mauvais plan
                }
            }
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
