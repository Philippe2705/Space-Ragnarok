using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AvailableShip : MonoBehaviour
{

    public Text ExperienceText;

    void Update()
    {
        if (ExperienceText.IsActive())
        {
            ExperienceText.text = "Experience : " + UserData.GetExperience().ToString();
            var ships = new string[] { "Frigate", "Cruiser", "BattleShip", "Ragnarok" };

            foreach (var ship in ships)
            {
                SetShipActive(ship, false);
            }
            if (UserData.GetExperience() >= 50)
            {
                //Frigate Available
                SetShipActive(ships[0], true);
            }
            if (UserData.GetExperience() >= 200)
            {
                //Cruiser Available
                SetShipActive(ships[1], true);
            }
            if (UserData.GetExperience() >= 600)
            {
                //BatleShip Available
                SetShipActive(ships[2], true);
            }
            if (UserData.GetExperience() >= 1200)
            {
                //Ragnarok Available
                SetShipActive(ships[3], true);
            }
        }
    }

    public void AddXP(int xp)
    {
        UserData.AddExperience(xp);
    }

    void SetShipActive(string shipName, bool active)
    {
        GameObject ship = GameObject.Find(shipName);
        ship.GetComponent<Button>().interactable = active;
        ship.GetComponentInChildren<Text>().color = active ? Color.white : Color.gray;
        ship.transform.FindChild("Locked").gameObject.SetActive(!active);
    }
}
