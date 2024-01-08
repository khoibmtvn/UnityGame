using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float maxspeed = 1.5f, jumpPow = 220f;
	public bool grounded = true, faceright = true, doublejump = false; 

	//Initialize this to preresent the class we want
	public Rigidbody2D r2;
	public Animator anim;

	Transform playerGraphics;										//Reference to the graphics so we can change direction

	[SerializeField]
	string landingSoundName = "LandingFootSteps";

	AudioManager audioManager;

	void Awake()
	{
		//Setting up references
		playerGraphics = transform.Find ("Graphics");
		if(playerGraphics == null)
		{
			Debug.LogError ("Let's freak out! There is no 'graphics' object as a child of the player");
		}

	}
		
	// Use this for initialization
	void Start () {
		//Get all the components in class Rigidbody2D
		r2 = gameObject.GetComponent<Rigidbody2D>();
		anim = gameObject.GetComponent<Animator>();

		//Kiem tra xem da tham chieu duoc audioManager hay chua
		audioManager = AudioManager.instance;
		if (audioManager == null)
		{
			Debug.LogError ("THIS IS WHY WE WRITE ERROR MESSAGES: No audioManager found.");
		}

	}
	
	// Update is called once per frame
	void Update () {
		//Use this to make Grounded to become grounded from above
		//To check that player is stand on the ground
		anim.SetBool("Grounded", grounded);
		//Set speed for player
		anim.SetFloat("Speed", Mathf.Abs(r2.velocity.x));
		bool wasGrounded = grounded;

		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			
			if (grounded) {
				grounded = false;
				doublejump = true;
				r2.AddForce (Vector2.up * jumpPow);
			} 
			else 
			{
				if (doublejump) 
				{
					doublejump = false;
					//Dung de co dinh nguoi choi
					r2.velocity = new Vector2 (r2.velocity.x, 0);
					r2.AddForce (Vector2.up * jumpPow * 0.5f);
				}
			}
		}
			
		//Neu nhu nhan vat nhay len va dap xuong mat dat tro lai thi se phat ra tieng dong
		if (wasGrounded != grounded && grounded == true)
		{
			Debug.Log ("Landing Sound Appear !!!");
			audioManager.PlaySound (landingSoundName);
		}

	}

	void FixedUpdate()
    {
		//Initialize variable
		//bool wasGrounded = grounded;
		float h = Input.GetAxisRaw("Horizontal");
		r2.AddForce ((Vector2.right) * PlayerStats.instance.movementSpeed * h);

		//Rang buoc toc do cua nguoi choi
		if (r2.velocity.x > maxspeed)
			r2.velocity = new Vector2 (maxspeed, r2.velocity.y);
		if (r2.velocity.x < - maxspeed)
			r2.velocity = new Vector2 ( - maxspeed, r2.velocity.y);

		//Lam cho nhan vat chuyen huong
		if(h > 0 && !faceright)
		{
			Flip();
		}
		if(h < 0 && faceright)
		{
			Flip();
		}

		//Lam cho van toc nhan vat giam, de tao cam giac chan thuc hon
		if (grounded) 
		{
			r2.velocity = new Vector2 (r2.velocity.x * 0.7f, r2.velocity.y);
		}

    }

	public void Flip()
	{
		//Switch the way player is labelled as facing
		faceright = !faceright;

		//Multiply the player's x local scale by -1
		Vector3 Scale;
		Scale = playerGraphics.localScale;
		Scale.x *= -1;
		playerGraphics.localScale = Scale;
	}
		
}
