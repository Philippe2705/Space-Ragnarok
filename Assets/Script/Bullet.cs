using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour {
	public GameObject Explosion;
	public GameObject ExplosionMobile;
	float speedSurprise;
	float autoDestruct;
	public AudioClip shot;
	// Use this for initialization
	void Start () {
		autoDestruct = 25;
		transform.Rotate (0,Random.Range(-10f,10f), 0);
		speedSurprise = Random.Range (0.9f, 1.1f);
	}
	
	// Update is called once per frame
	void Update () {
		autoDestruct -= Time.deltaTime;
		transform.Translate (Vector3.forward * Time.fixedDeltaTime * speedSurprise * 3);
		if (autoDestruct <= 0)
		{
			Network.Destroy(gameObject);
		}
	}
	void OnTriggerEnter2D(Collider2D Other) {
		if (Other.gameObject.name == "ShipBot(Clone)") {
			Other.gameObject.GetComponent<Bot> ().hitByBullet(transform.position, transform.rotation);
		} 
		else {
			Other.gameObject.GetComponent<PlayerController> ().hitByBullet(transform.position, transform.rotation);

		}
		Instantiate (ExplosionMobile, transform.position, transform.rotation);
		Destroy(gameObject);
	}
}
