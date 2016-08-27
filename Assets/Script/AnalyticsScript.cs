using UnityEngine.Analytics;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.UI;


public class AnalyticsScript : MonoBehaviour {
    [System.Runtime.InteropServices.DllImport("KERNEL32.DLL")]
    private static extern int GetSystemDefaultLCID();
    public Text languagueUI;

    public string language;
    // Use this for initialization
    void Start () {

        var currentCulture = new CultureInfo(GetSystemDefaultLCID());
        language = currentCulture.ToString();
        languagueUI.text = language;
    }

    // Update is called once per frame
    void Update () {
	}
    
    public void SendSystemInfos()
    {
        if (!Application.isEditor)
        {
            Analytics.CustomEvent("SystemInfos", new Dictionary<string, object>
        {
            { "Date", System.DateTime.Now.ToString() },
            { "Device Model ", SystemInfo.deviceModel },
            { "Device Name", SystemInfo.deviceName },
            { "Device Type", SystemInfo.deviceType },
            { "Memory Size", SystemInfo.graphicsMemorySize },
            { "Device Vendor", SystemInfo.graphicsDeviceVendor },
            { "Operating System", SystemInfo.operatingSystem },
            { "Precessor count", SystemInfo.processorCount },
            { "Processor Frequency", SystemInfo.processorFrequency },
            { "Operating System", language },
            { "Total credits", UserData.GetCredits()},
            { "Total Experience", UserData.GetExperience()}
          });
        }
    }
    
}
