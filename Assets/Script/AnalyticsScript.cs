using UnityEngine.Analytics;
using UnityEngine;
using System.Collections;
using UnityEngine.Purchasing;
using System.Collections.Generic;

public class AnalyticsScript : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
        Purchase();
	}
	
	// Update is called once per frame
	void Update () {
	}
    

    public void Purchase() //test
    {
        print("purchase...");
        Analytics.Transaction("yolo", 0.99m, "EUR", null, null);
    }
}
