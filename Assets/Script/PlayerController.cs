using UnityEngine;
using System.Collections;
using CnControls;
using UnityEngine.Networking;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Sprites;

public class PlayerController : MonoBehaviour {

	public float speed = 100f;
	Transform myTransform;
	public float minposx = -80f, maxposx = 80f, minposy = -30f, maxposy = 30f;
	public GameObject landscapePrefab;
	public GameObject landscape;
	public GameObject bullet;
	public GameObject explosion;
	public GameObject bigExplosion;
	public Transform rightGun;
	public Transform leftGun;
	GameObject RightGunReloadingBar;
	GameObject LeftGunReloadingBar;
	GameObject HealthBar;
	float reloadingTime = 3f;
	float reloadTimeR;
	float reloadTimeL;
	float explosionTimer = 2;
	float explosionTimerEffect;
	bool isExploding = false;
	public int vie = 100;
	bool hasExploded = false;
	public GameObject explosionSounds;
	
	public GameObject smallexplosionSounds;
	
	public GameObject bigexplosionSounds;
		
	// Use this for initialization
	void Start () {
		vie = 100;
		RightGunReloadingBar = GameObject.Find("RightReloading");
		LeftGunReloadingBar = GameObject.Find("LeftReloading");
		HealthBar = GameObject.Find ("HealthBar");

		reloadTimeR = 0;
		reloadTimeL = 0;
		myTransform = GetComponent<Transform> ();
		//Instantiate(landscapePrefab, transform.position, transform.rotation);
		landscape = GameObject.Find ("Landscape");
		landscape.GetComponentInChildren<BackgroundSync> ().joueur = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		HealthBarManager ();
		if (vie <= 0) 
		{
			isExploding = true;
		}
		if (isExploding) {
			explosionTimer -= Time.deltaTime;
			explosionTimerEffect -= Time.deltaTime;
			if (explosionTimerEffect <= 0)
			{
				StartExplose();
				explosionTimerEffect = 0.1f;
			}
			if (explosionTimer <= 0)
			{
				Explose();
			}
		}
		RightGunReloadingBar.GetComponent<Slider> ().value = (1 -(reloadTimeR / reloadingTime));
		LeftGunReloadingBar.GetComponent<Slider> ().value = (1 -(reloadTimeL / reloadingTime));

		if (CnInputManager.GetAxisRaw("Horizontal1") > 0.2f || CnInputManager.GetAxisRaw("Horizontal1") < -0.2f)
		{
			shootManager();
		}
		float mouveHorizontal =CnInputManager.GetAxis ("Horizontal");
		float mouveVertical = CnInputManager.GetAxis ("Vertical") * speed;
		mouveVertical *= Time.deltaTime;
		if (mouveVertical < 0) { mouveVertical = 0;}
		if (vitesse () <= 2) { //vitesse maximum
			GetComponent<Rigidbody2D> ().AddForce (transform.TransformDirection (0, 1, 0) * Time.deltaTime * mouveVertical * speed);
		} 
		GetComponent<Rigidbody2D> ().AddForce (transform.TransformDirection (0, 1, 0) * Time.deltaTime * 0.1f * speed); //Vitesse minimum

		Vector3 rotatation = new Vector3 (0, 0, mouveHorizontal * 1.5f);
		transform.Rotate (rotatation * Time.deltaTime * -15);
		myTransform.position = new Vector3 (Mathf.Clamp (myTransform.position.x, minposx, maxposx), Mathf.Clamp (myTransform.position.y, minposy, maxposy), myTransform.position.z);
	}
	void FixedUpdate()
	{
		if (reloadTimeR > 0) 
		{
			reloadTimeR -= Time.deltaTime;
		}
		if (reloadTimeL > 0) 
		{
			reloadTimeL -= Time.deltaTime;
		}

	}
	void StartExplose()
	{
		if (hasExploded == false) 
		{
			Vector3 randomPos = new Vector3( transform.position.x + Random.Range(-1f, 1f) ,transform.position.y + Random.Range(-1f, 1f), -1);
			Network.Instantiate (explosion, randomPos, transform.rotation, 0);
			Network.Instantiate (smallexplosionSounds, randomPos, transform.rotation, 0);
		}
	}
	void Explose()
	{
		hasExploded = true;
		Network.Instantiate(bigExplosion, new Vector3(transform.position.x, transform.position.z, -1), transform.rotation, 0);
		GameObject.Find ("NetworkSyncer1").GetComponent<NetworkSync> ().isAlive = false;
		GameObject.Find ("NetworkSyncer1").GetComponent<NetworkSync> ().respawnTime = 0;
		Network.Instantiate (bigexplosionSounds, transform.position, transform.rotation, 0);
		Network.Destroy (gameObject);
	}
	void shootManager()
	{
		if (CnInputManager.GetAxis ("Horizontal1") == 0) {return;}
		//transform.GetChild
		GameObject[] rightGuns = new GameObject[7];
		GameObject[] leftGuns = new GameObject[7];
			for (int i = 0; i <= 6; i++) 
			{
				rightGuns [i] = rightGun.transform.GetChild (i).gameObject;
				leftGuns [i] = leftGun.transform.GetChild (i).gameObject;
			}
			float shootHorizontal = CnInputManager.GetAxis ("Horizontal1");
			if (shootHorizontal > 0 && reloadTimeR <= 0) 
			{
				for (int i = 0; i <= 6; i++) 
				{
					Network.Instantiate (bullet, rightGuns [i].transform.position, rightGuns [i].transform.rotation, 0);
				}
				reloadTimeR = reloadingTime;
			} 
			else if (shootHorizontal < 0 && reloadTimeL <= 0) 
			{
				for (int i = 0; i <= 6; i++) 
				{
					Network.Instantiate (bullet, leftGuns [i].transform.position, leftGuns [i].transform.rotation, 0);
				}
				reloadTimeL = reloadingTime;
			}
	}
	void HealthBarManager()
	{
		HealthBar.GetComponent<Slider> ().value = (vie / 100f);
		Color couleur;
		couleur.r = (1f - (vie / 100f));
		couleur.g = (vie / 100f);
		couleur.b = 0f;
		couleur.a = 1f;
		HealthBar.transform.FindChild ("Background").GetComponent<Image> ().color = couleur;
	}
	float vitesse () //calcule la vitesse
	{
		float resultat;
		float carre = GetComponent<Rigidbody2D> ().velocity.x * GetComponent<Rigidbody2D> ().velocity.x + GetComponent<Rigidbody2D> ().velocity.y * GetComponent<Rigidbody2D> ().velocity.y;
		resultat = Mathf.Sqrt (carre);
		return resultat;
	}
	public void hitByBullet(Vector3 position1, Quaternion rotation1)
	{
		print ("touché");
		vie -= (2 + Random.Range (0, 4));
		print (vie);
		Network.Instantiate (explosionSounds, position1, rotation1, 0);
	}
}
