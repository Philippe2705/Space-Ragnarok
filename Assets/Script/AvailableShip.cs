using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AvailableShip : MonoBehaviour
{

    public Text ExperienceText;
    public Image ShipPreview;
    public Transform ShipContainer;
    public GameObject ShipBuyPrefab;

    void Update()
    {
        if (ExperienceText.IsActive())
        {
            ExperienceText.text = "Credits : " + UserData.GetCredits().ToString();

            if (ShipContainer.childCount != Constants.ShipsCount)
            {
                for (int i = 0; i < ShipContainer.childCount; i++)
                {
                    Destroy(ShipContainer.GetChild(i).gameObject);
                }
                for (int i = 0; i < Constants.ShipsCount; i++)
                {
                    var s = Instantiate(ShipBuyPrefab) as GameObject;
                    s.transform.SetParent(ShipContainer);
                    s.transform.localScale = Vector3.one;
                    s.transform.localRotation = Quaternion.identity;
                    s.transform.localPosition = Vector3.zero;
                    s.transform.SetAsLastSibling();
                    s.GetComponentInChildren<Text>().text = ShipProperties.GetShip(i).ShipName;
                    s.name = ShipProperties.GetShip(i).ShipName;
                    int x = i;
                    s.GetComponent<Button>().onClick.AddListener(() => OnShipId(x));
                    s.GetComponent<Button>().interactable = true;
                }
            }


            for (int i = 0; i < Constants.ShipsCount; i++)
            {
                SetShipActive(i, UserData.HasBoughtShip(i));
                SetShipLocked(i, !(UserData.CanBuyShip(i) || UserData.HasBoughtShip(i)));
            }
        }
    }
    public void AddXP(int xp)
    {
        UserData.AddExperience(xp);
    }

    void SetShipActive(int shipId, bool active)
    {
        GameObject ship = GameObject.Find(ShipProperties.GetShip(shipId).ShipName);
        ship.GetComponentInChildren<Text>().text = active ? ShipProperties.GetShip(shipId).ShipName : ShipProperties.GetShip(shipId).Price.ToString();
        ship.GetComponentInChildren<Text>().color = active ? Color.white : Color.gray;
    }

    void SetShipLocked(int shipId, bool locked)
    {
        GameObject ship = GameObject.Find(ShipProperties.GetShip(shipId).ShipName);
        ship.GetComponentInChildren<Text>().color = locked ? Color.gray : Color.white;
        ship.transform.Find("Locked").gameObject.SetActive(locked);
    }
    void SetShipID(int id)
    {
        UserData.SetShipId(id);
        ShipPreview.sprite = ShipProperties.GetShip(id).ShipSprite;
    }

    public void OnShipId(int id)
    {
        if (!UserData.HasBoughtShip(id))
        {
            if (UserData.CanBuyShip(id))
            {
                UserData.BuyShip(id);
            }
        }
    }
}
