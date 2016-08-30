using UnityEngine;
using UnityEngine.Networking;

class NetworkShield : NetworkBehaviour
{
    public void SendRpcHitByBullet(string name)
    {
        print(name);
        RpcHitByBullet(name);
    }

    [ClientRpc]
    void RpcHitByBullet(string name)
    {
        transform.Find("Shields").Find(name).GetComponent<Shield>().HitByBullet();
    }
}
