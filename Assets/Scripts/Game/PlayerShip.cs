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


        if (isLocalPlayer)
        {
            camera = GameObject.Find("PlayerCamera");
            if (camera == null)
            {
                camera = Instantiate(new GameObject(), Vector3.forward * -10, Quaternion.identity) as GameObject;
                camera.name = "PlayerCamera";
                camera.AddComponent<AudioListener>();
                camera.AddComponent<Camera>();
                camera.GetComponent<Camera>().orthographic = true;
                camera.GetComponent<Camera>().orthographicSize = ShipProperties.GetShip(ShipId).ViewDistance;
                camera.tag = "MainCamera";
            }
            rigidbody2D.interpolation = RigidbodyInterpolation2D.Interpolate;
            OnHealthDelegate(100);
        }
    }

    protected override void FixedUpdateClient()
    {
        base.FixedUpdateClient();
        if (isLocalPlayer)
        {
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
        if (isLocalPlayer)
        {
            /*
             * Camera position
             */
            var camPos = camera.transform.position;
            camPos.x = Mathf.Lerp(camPos.x, transform.position.x, Constants.CameraStabilization);
            camPos.y = Mathf.Lerp(camPos.y, transform.position.y, Constants.CameraStabilization);
            camera.transform.position = camPos;
            camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, transform.rotation, Constants.CameraStabilization);
            /*
             * Fire
             */
            var fireVector = CnInputManager.GetAxisRaw("Horizontal1") * Vector2.right + CnInputManager.GetAxisRaw("Vertical1") * Vector2.up;
            if (fireVector.magnitude > Constants.FireTrigger)
            {
                CmdFire(fireVector);
            }
        }
    }


    protected override void OnHealthDelegate(float value)
    {
        if (isLocalPlayer && !IsBot && healthBar != null)
        {
            healthBar.UpdateHealth(value);
        }
    }
}
