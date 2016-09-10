using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using CnControls;

public class PlayerShip : Ship
{
    HealthBar healthBar;
    new GameObject camera;
    GameObject background;
    Vector2 backgroundSize;

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
                camera.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
                camera.GetComponent<Camera>().orthographic = true;
                camera.GetComponent<Camera>().orthographicSize = ShipProperties.GetShip(ShipId).ViewDistance;
                camera.tag = "MainCamera";
            }
            background = GameObject.Find("Background");
            backgroundSize = background.GetComponent<SpriteRenderer>().bounds.extents;
            var s = camera.GetComponent<Camera>().orthographicSize / background.GetComponent<SpriteRenderer>().bounds.extents.x;
            background.transform.localScale *= s * 6;
            backgroundSize *= s / 2;
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
            float horizontal = -ETCInput.GetAxis("Horizontal");
            float vertical = ETCInput.GetAxis("Vertical");
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
            camera.transform.position = new Vector3(transform.position.x, transform.position.y, camera.transform.position.z);
            camera.transform.rotation = transform.rotation;
            /*
             * Background position
             */
            background.transform.position = -new Vector3(backgroundSize.x * transform.position.x / MaxPosX, backgroundSize.y * transform.position.y / MaxPosY, -100) + transform.position;
            /*
             * Fire
             */
            var fireVector = ETCInput.GetAxis("HorizontalFire") * Vector2.right + ETCInput.GetAxis("VerticalFire") * Vector2.up;
            if (fireVector.magnitude > Constants.FireTrigger)
            {
                CmdFire(fireVector);
            }
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
