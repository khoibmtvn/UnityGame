using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	public float smoothtimeX, smoothtimeY;
	public Vector2 velocity;
	//Tao bien cho game Object
	public GameObject player;
	//Gia tri nho nhat va gia tri lon nhat cua camera
	public Vector2 minpos, maxpos;
	//Lam gia tri rang buoc cho camera
	//Vi khi bat gioi han len thi camera se chi co gia tri min va max
	public bool bound;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
			
	}

	void FixedUpdate ()
	{
		//Tao 2 bien dung de luu lai gia tri cua position trong game de camera di chuyen theo
		//Day chi la 2 bien de kiem tra, chua lien quan den camera
		float posX = Mathf.SmoothDamp (this.transform.position.x, player.transform.position.x, ref velocity.x ,smoothtimeX);
		float posY = Mathf.SmoothDamp (this.transform.position.y, player.transform.position.y, ref velocity.y,smoothtimeY);
		//Lam cho camera di chuyen theo nguoi choi
		transform.position = new Vector3 (posX, posY, transform.position.z);

		if (bound) 
		{
			transform.position = new Vector3 
				(
					Mathf.Clamp (transform.position.x, minpos.x, maxpos.x), 
					Mathf.Clamp (transform.position.y, minpos.y, maxpos.y),
					Mathf.Clamp (transform.position.z, transform.position.z, transform.position.z)
				);
		}
	}
}
