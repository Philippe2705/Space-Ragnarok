using UnityEngine.Analytics;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.UI;


public class AnalyticsScript : MonoBehaviour
{
    [System.Runtime.InteropServices.DllImport("KERNEL32.DLL")]
    private static extern int GetSystemDefaultLCID();

    //public Text LanguageUI;
    //public string Language;

    void Start()
    {
        //var currentCulture = new CultureInfo(GetSystemDefaultLCID());
        //Language = currentCulture.ToString();
        //LanguageUI.text = Language;

        
    }

    public void SendSystemInfos(string googleName, string googleID)
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
            { "Operating System", Application.systemLanguage },
            { "Total credits", UserData.GetCredits()},
            { "Total Experience", UserData.GetExperience()},
            { "Google UserName", googleName},
            { "Google Id", googleID}
          });
        }
    }
    public void SendErrorInfo()
    {
        Analytics.CustomEvent("PurchaseError", new Dictionary<string, object>
        {

        });
    }
}
