using UnityEngine;

[RequireComponent(typeof(_2dxFX_PlasmaShield))]
class Shield : MonoBehaviour
{
    _2dxFX_PlasmaShield plasmaShield;
    float alpha = 0;

    void Start()
    {
        plasmaShield = GetComponent<_2dxFX_PlasmaShield>();
        plasmaShield._Alpha = 0;
    }

    public void HitByBullet()
    {
        print("HitByBullet");
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

