using UnityEngine;

public class PlayerStats: MonoBehaviour
{
	//ItemGameMaster itemGm;

	public static PlayerStats instance;

	public bool ItemCondition = false;

	public int maxHealth = 100;

	private int _curHealth;
	public int curHealth
	{
		get { return _curHealth; }
		set { 
			if (ItemCondition == true) {
				_curHealth = Mathf.Clamp (1000, 0, maxHealth);
				ItemCondition = false;
			} 
			else 
			{
				_curHealth = Mathf.Clamp (value, 0, maxHealth); 
			}
		}
	}

	public float healthRegenRate = 2f;

	public float movementSpeed = 10f;

	void Awake()
	{
		if (instance == null) 
		{
			instance = this;
		}

		curHealth = maxHealth;
	}

	void Start ()
	{
		//Lay cac Component trong ItemGameMaster
		//itemGm = GameObject.FindGameObjectWithTag ("ItemContainer").GetComponent<ItemGameMaster>();
	
	}

}
