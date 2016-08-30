using UnityEngine;
using UnityEngine.Networking;
using ProgressBar;
using CnControls;

class Reload : MonoBehaviour
{
    public GameObject reloadUI;

    float[] reloadTimes;
    GameObject go;
    ShipProperty shipProperty;
    Transform shipTransform;
    ProgressRadialBehaviour[] progressBars;
    Transform[] guns;


    public void SetTransform(int id, Transform shipTransform)
    {
        var go = Instantiate(ShipProperties.GetShip(id).PlayerShipPrefab);
        foreach (var s in new string[] { "Shields", "Explosions", "Smokes", "Motors" })
        {
            if (go.transform.Find(s))
            {
                Destroy(go.transform.Find(s).gameObject);
            }
        }

        Destroy(go.GetComponent<PlayerShip>());
        if (go.GetComponent<NetworkShield>() != null)
        {
            Destroy(go.GetComponent<NetworkShield>());
        }
        Destroy(go.GetComponent<NetworkTransform>());
        Destroy(go.GetComponent<NetworkIdentity>());
        Destroy(go.GetComponent<NetworkIdentity>());

        go.AddComponent<_2dxFX_GrayScale>()._Alpha = 0.5f;

        Utility.AddChildsToArray(out guns, "Guns", shipTransform);
        shipProperty = ShipProperties.GetShip(id);
        this.shipTransform = shipTransform;

        progressBars = new ProgressRadialBehaviour[guns.Length];
        for (var i = 0; i < guns.Length; i++)
        {
            var g = Instantiate(reloadUI) as GameObject;
            g.transform.SetParent(guns[i]);
            g.transform.localScale = Vector3.one;
            g.transform.position = Vector3.zero;
            g.transform.rotation = Quaternion.identity;
            progressBars[i] = g.GetComponent<ProgressRadialBehaviour>();
        }
    }

    void Update()
    {
        if (go != null)
        {
            /*
             * Update reloads times
             */
            for (var i = 0; i < reloadTimes.Length; i++)
            {
                reloadTimes[i] -= Time.deltaTime;
            }

            /*
             * Simulate fire
             */
            var fireVector = CnInputManager.GetAxisRaw("HorizontalFire") * Vector2.right + CnInputManager.GetAxisRaw("VerticalFire") * Vector2.up;
            if (fireVector.magnitude > Constants.FireTrigger)
            {
                var direction = shipTransform.TransformVector(fireVector);

                for (int i = 0; i < guns.Length; i++)
                {
                    var gun = guns[i];
                    if (reloadTimes[i] < 0)
                    {
                        if (Vector2.Angle(direction, gun.right) < shipProperty.FireAngleTolerance)
                        {
                            reloadTimes[i] = shipProperty.ReloadTime;
                        }
                    }
                }
            }

            /*
             * Update UI
             */
            for (var i = 0; i < reloadTimes.Length; i++)
            {
                progressBars[i].SetFillerSizeAsPercentage(100 * reloadTimes[i] / shipProperty.ReloadTime);
            }
        }
    }
}