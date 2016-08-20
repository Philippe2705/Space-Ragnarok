using UnityEngine;
using System.Collections;

public class Bot : MonoBehaviour {
	Transform myTransform;
	public float minposx = -60f, maxposx = 60f, minposy = -30f, maxposy = 30f;
	float explosionTimer = 2;
	float explosionTimerEffect;
	bool isExploding = false;
	public int vie = 100;
	bool hasExploded = false;
	public float speed = 100f;
	public GameObject explosion;
	public GameObject bigExplosion;
	float mouveHorizontal = 0;
	float mouveVertical = 0.6f;
	float hasAlreadyTurn = 4;
	bool playTimer = false;
	float timer;
	public GameObject explosionSounds;
	
	public GameObject smallexplosionSounds;
	
	public GameObject bigexplosionSounds;
	// Use this for initialization
	void Start () {

		myTransform = GetComponent<Transform> ();
		transform.position = new Vector2 (Random.Range (-50, 50), Random.Range (-20, 20));
		transform.Rotate(0, 0, Random.Range(0,360));
	}
	
	// Update is called once per frame
	void Update () {
		iaMoves ();
		if (vie <= 0) 
		{
			isExploding = true;
		}
		if (playTimer) 
		{
			timer -= Time.deltaTime;
			if (timer <= 0)
			{
				playTimer = false;
				mouveHorizontal = 0;
			}
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

		if (vitesse () <= 2) { //vitesse maximum
			GetComponent<Rigidbody2D> ().AddForce (transform.TransformDirection (0, 1, 0) * Time.deltaTime * mouveVertical * speed);
		} 
		GetComponent<Rigidbody2D> ().AddForce (transform.TransformDirection (0, 1, 0) * Time.deltaTime * 0.1f * speed); //Vitesse minimum
		
		Vector3 rotatation = new Vector3 (0, 0, mouveHorizontal * 1.5f);
		transform.Rotate (rotatation * Time.deltaTime * -15);
		myTransform.position = new Vector3 (Mathf.Clamp (myTransform.position.x, minposx, maxposx), Mathf.Clamp (myTransform.position.y, minposy, maxposy), myTransform.position.z);
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
		Network.Instantiate (bigexplosionSounds, transform.position, transform.rotation, 0);
		print (gameObject.name + "A été détruit");
		Network.Destroy (gameObject);
	}
	void iaMoves()
	{
		if (transform.localPosition.x >= 50) 
		{
			if (transform.localRotation.eulerAngles.z >= 270 || transform.localRotation.eulerAngles.z <= 90) 
			{
				//tourne a gauche
				mouveHorizontal = -1;
				hasAlreadyTurn = 4;
			} 
			else 
			{
				//tourne a droite
				mouveHorizontal = 1;
				hasAlreadyTurn = 4;
			}
		} 
		else if (transform.localPosition.x <= -50) 
		{
			
			if (transform.localRotation.eulerAngles.z <= 90 || transform.localRotation.eulerAngles.z >= 270) 
			{
				//tourne a gauche
				mouveHorizontal = 1;
				hasAlreadyTurn = 4;
			} 
			else 
			{
				//tourne a droite
				mouveHorizontal = -1;
				hasAlreadyTurn = 4;
			}
		} 
		else if (transform.localPosition.y >= 20) 
		{
			
			if (transform.localRotation.eulerAngles.z >= 0) 
			{
				//tourne a gauche
				mouveHorizontal = -1;
				hasAlreadyTurn = 4;
			} 
			else 
			{
				//tourne a droite
				mouveHorizontal = 1;
				hasAlreadyTurn = 4;
			}
		} else if (transform.localPosition.y <= -20) 
		{
			
			if (transform.localRotation.eulerAngles.z >= 180) 
			{
				//tourne a gauche
				mouveHorizontal = -1;
				hasAlreadyTurn = 4;
			} 
			else 
			{
				//tourne a droite
				mouveHorizontal = 1;
				hasAlreadyTurn = 4;
			}
		} else if (hasAlreadyTurn <= 0) 
		{
			randomMove();
			hasAlreadyTurn = 4;
		}
		else 
		{
			hasAlreadyTurn -= Time.deltaTime;
			if (!playTimer)
			{
			mouveHorizontal = 0;
			}
		}
	}
	void randomMove()
	{
		float nombre = Random.Range (-1f, 1f);
		if (nombre >= 0) 
		{
			mouveHorizontal = 1;
		}
		else
		{
			mouveHorizontal = -1;
		}
		playTimer = true;
		timer = Random.Range(1f,2f);
	}
}
