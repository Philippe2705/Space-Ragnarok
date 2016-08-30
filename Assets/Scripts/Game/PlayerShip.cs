using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using CnControls;

public class PlayerShip : Ship
{
    HealthBar healthBar;
    new GameObject camera;

    protected override void Start()
    {
        base.Start();

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
            var fireVector = CnInputManager.GetAxisRaw("HorizontalFire") * Vector2.right + CnInputManager.GetAxisRaw("VerticalFire") * Vector2.up;
            if (fireVector.magnitude > Constants.FireTrigger)
            {
                CmdFire(fireVector);
            }
        }
    }

    protected override void OnShipIdDelegate(int id)
    {
        if (isLocalPlayer)
        {
            FindObjectOfType<Reload>().SetTransform(ShipId, transform);
        }
    }

    protected override void OnHealthDelegate(float value)
    {
        if (isLocalPlayer && playerControllerId == 0)
        {
            if (healthBar == null)
            {
                healthBar = FindObjectOfType<HealthBar>();
            }
            healthBar.UpdateHealth(value);
        }
    }
}
