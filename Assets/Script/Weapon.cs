﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public static Weapon instance;

	public float fireRate = 0;
	public int Damage = 10;
	public LayerMask whatToHit;

	public Transform BulletTrailPrefab;
    public Transform HitPrefab;
	public Transform MuzzleFlashPrefab;

	float timeToSpawnEffect = 0;
	public float effectSpawnRate = 10;

	float timeToFire = 0;
	Transform firePoint;
	Transform muzzlePos;

    //Handle Camera Shaking
    public float camShakeAmt = 0.1f;
    public float camShakeLength = 0.1f;
    CameraShake camShake;

	public string weaponShootSound = "DefaultShot";

	//Caching
	AudioManager audioManager;

	// Use this for initialization
	void Awake () {
		firePoint = transform.Find ("FirePoint");
		muzzlePos = transform.Find ("MuzzlePos");

		if(firePoint == null)
		{
			Debug.LogError ("No, firepoint? WHAT?!!");
		}
			
		if (muzzlePos == null) 
		{
			Debug.LogError ("No muzzlePos? What?!!");
		}
		if (instance == null)
		{ 
			instance = this;
		}
	}

    void Start()
    {
        camShake = GameMaster.gm.GetComponent<CameraShake>();
        if (camShake == null)
        {
            Debug.LogError ("No CameraShake script found on the GM object.");
        }

		audioManager = AudioManager.instance;
		if (audioManager == null)
		{
			Debug.Log ("FREAK OUT! No audioManager found in scene.");
		}
    }
	
	// Update is called once per frame
	void Update () 
	{
		//Shoot ();
		if (fireRate == 0)
		{
			if (Input.GetButtonDown ("Fire1")) {
				Shoot ();
			}
		} 
		else 
		{
			if (Input.GetButton ("Fire1") && Time.time > timeToFire) 
			{
				timeToFire = Time.time + 1/fireRate;
				Shoot ();
			}
		}


	} //End of Update ()

	void Shoot ()
	{
		//Debug.Log ("Test");
		Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
		Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, mousePosition-firePointPosition, 100, whatToHit);
		
		Debug.DrawLine (firePointPosition, (mousePosition - firePointPosition) * 100, Color.cyan);
		if (hit.collider != null) {
			Debug.DrawLine (firePointPosition, hit.point, Color.red);
			//Debug.Log ("We Hit " + hit.collider.name + "and did " + Damage + " Damage.");
			Enemy enemy = hit.collider.GetComponent<Enemy>();
			if (enemy != null){
				enemy.DamageEnemy (Damage);
				//Debug.Log ("We Hit " + hit.collider.name + "and did " + Damage + " Damage.");
			}
		}

        if (Time.time >= timeToSpawnEffect) 
        {
            Vector3 hitPos;
            Vector3 hitNormal;

            if (hit.collider == null) 
            {
                hitPos = (mousePosition - firePointPosition) * 30;
                hitNormal = new Vector3 (9999, 9999, 9999);
            }
            else
            {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }

			Effect (hitPos, hitNormal);
			timeToSpawnEffect = Time.time + 1/effectSpawnRate;
		}


	} //End of void Shoot

	//This is the effect that will make Bullet Trail appear
	void Effect(Vector3 hitPos, Vector3 hitNormal)
    {
		Transform trail = Instantiate (BulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
        LineRenderer lr = trail.GetComponent<LineRenderer>();

        if (lr != null) 
        {
            // SET POSITIONS
            lr.SetPosition (0, firePoint.position);
            lr.SetPosition(1, hitPos);
        }

        Destroy (trail.gameObject, 0.05f);

        if (hitNormal != new Vector3(9999, 9999, 9999) )
        {
            Transform hitParticle = Instantiate (HitPrefab, hitPos, Quaternion.FromToRotation (Vector3.right, hitNormal) ) as Transform;
            Destroy (hitParticle.gameObject, 1f);
        }

		Transform clone = Instantiate (MuzzleFlashPrefab, muzzlePos.position, muzzlePos.rotation) as Transform;
		clone.parent = muzzlePos;
		float size = Random.Range(0.6f, 0.9f);
		clone.localScale = new Vector3 (size, size, size);
		Destroy (clone.gameObject, 0.02f);

        //Shake the camera
        camShake.Shake (camShakeAmt, camShakeLength);

		//Play Shoot sound
		audioManager.PlaySound (weaponShootSound);

	}// End of void Effect(Vector3 hitPos, Vector3 hitNormal)

}