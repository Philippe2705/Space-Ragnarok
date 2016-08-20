using UnityEngine;
using System.Collections;

public class AutoDestruct : MonoBehaviour {

	public float timer = 5;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0)
		{
			Network.Destroy(gameObject);
		}
	}
}
