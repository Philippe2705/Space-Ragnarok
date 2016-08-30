using UnityEngine;
using UnityEngine.Networking;

class Shield : NetworkBehaviour
{
    _2dxFX_PlasmaShield plasmaShield;
    float alpha = 1;

    void Start()
    {
        plasmaShield = GetComponent<_2dxFX_PlasmaShield>();
    }

    [Server]
    public void HitByBullet()
    {
        RpcHitByBullet();
    }

    [ClientRpc]
    void RpcHitByBullet()
    {
        alpha = 1;
        plasmaShield.enabled = true;
    }

    void Update()
    {
        if (alpha > 0)
        {
            alpha -= Time.deltaTime / 5;
            if (alpha > 0)
            {
                plasmaShield._Alpha = alpha;
            }
            else
            {
                plasmaShield.enabled = false;
            }
        }
    }
}

