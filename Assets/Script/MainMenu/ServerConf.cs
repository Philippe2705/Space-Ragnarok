using UnityEngine;
using UnityEngine.UI;
using System;

public class ServerConf : MonoBehaviour
{
    public Slider BotCountSlider;
    public Slider ClientCountSlider; //min client
    public Slider MaxClientCountSlider;

    void Start() //playerPrefs
    {
        BotCountSlider.value = PlayerPrefs.GetInt("BotCountPref");
        ClientCountSlider.value = PlayerPrefs.GetInt("ClientCountPref");
        MaxClientCountSlider.value = PlayerPrefs.GetInt("MaxClientCountPref");

    }

    void Update()
    {
        BotCountSlider.transform.FindChild("text").GetComponent<Text>().text = BotCountSlider.value.ToString();
        ClientCountSlider.transform.FindChild("text").GetComponent<Text>().text = ClientCountSlider.value.ToString();
        MaxClientCountSlider.transform.FindChild("text").GetComponent<Text>().text = MaxClientCountSlider.value.ToString();

    }
    public void serverStarted()
    {

        PlayerPrefs.SetInt("BotCountPref", Convert.ToInt32(BotCountSlider.value));
        PlayerPrefs.SetInt("ClientCountPref", Convert.ToInt32(BotCountSlider.value));
        PlayerPrefs.SetInt("MaxClientCountPref", Convert.ToInt32(BotCountSlider.value));
    }
}
