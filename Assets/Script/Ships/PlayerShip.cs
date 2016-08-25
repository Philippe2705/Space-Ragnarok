using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using CnControls;

public class PlayerShip : Ship
{
    Slider rightGunReloadingBar;
    Slider leftGunReloadingBar;
    HealthBar healthBar;
    new GameObject camera;

    protected override void Start()
    {
        base.Start();
        rightGunReloadingBar = GameObject.Find("RightReloading").GetComponent<Slider>();
        leftGunReloadingBar = GameObject.Find("LeftReloading").GetComponent<Slider>();
        healthBar = FindObjectOfType<HealthBar>();
        GetComponentInChildren<Camera>().enabled = isLocalPlayer && false;
        GetComponentInChildren<Camera>().tag = isLocalPlayer && false ? "MainCamera" : "Untagged";
        GetComponentInChildren<AudioListener>().enabled = isLocalPlayer;


        if (isLocalPlayer)
        {
            camera = Instantiate(new GameObject(), Vector3.forward * -10, Quaternion.identity) as GameObject;
            camera.AddComponent<Camera>();
            camera.GetComponent<Camera>().orthographic = true;
            camera.GetComponent<Camera>().orthographicSize = ShipProperties.GetShip(ShipId).ViewDistance;
            camera.tag = "MainCamera";
            rigidbody2D.interpolation = RigidbodyInterpolation2D.Interpolate;
        }
    }

    protected override void FixedUpdateClient()
    {
        base.FixedUpdateClient();
        if (isLocalPlayer)
        {
            /*
             * Update reload sliders
             */
            rightGunReloadingBar.value = 1 - reloadTimeR / shipProperty.ReloadTime;
            leftGunReloadingBar.value = 1 - reloadTimeL / shipProperty.ReloadTime;

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

    protected override void UpdateClient()
    {
        base.UpdateClient();
        var camPos = camera.transform.position;
        camPos.x = Mathf.Lerp(camPos.x, transform.position.x, Constants.CameraStabilization);
        camPos.y = Mathf.Lerp(camPos.y, transform.position.y, Constants.CameraStabilization);
        camera.transform.position = camPos;
        camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, transform.rotation, Constants.CameraStabilization);
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
