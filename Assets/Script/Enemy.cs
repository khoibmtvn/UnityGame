using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class Enemy : MonoBehaviour {

	[System.Serializable]
	public class EnemyStats {
		public int maxHealth = 100;

		private int _curHealth;
		public int curHealth
		{
			get { return _curHealth; }
			set { _curHealth = Mathf.Clamp (value, 0, maxHealth);}
		}

		public int damage = 40;

		public void Init ()
		{
			curHealth = maxHealth;
		}
	}

	public EnemyStats stats = new EnemyStats();

	public Transform deathParticle;

	public float shakeAmt = 0.1f;
	public float shakeLength = 0.1f;

	public string deathSoundName = "Explosion";

	public int moneyDrop = 50;

	[Header("Optional: ")]
	[SerializeField]
	private StatusIndicator statusIndicator;

	void Start ()
	{
		stats.Init ();

		if (statusIndicator != null) 
		{
			statusIndicator.SetHealth (stats.curHealth, stats.maxHealth);
		}

		GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

		if (deathParticle == null)
		{
			Debug.LogError ("No death particle referenced on enemy !!");
		}
			
	}

	//Handle what happens when the upgrade menu is toggles
	void OnUpgradeMenuToggle (bool active)
	{
		GetComponent<EnemyAI>().enabled = !active;

	}

	public void DamageEnemy (int damage)
	{
		stats.curHealth -= damage;
		if (stats.curHealth <= 0) 
		{
			GameMaster.KillEnemy (this);
		}

		if (statusIndicator != null) 
		{
			statusIndicator.SetHealth (stats.curHealth, stats.maxHealth);
		}
	}

	void OnCollisionEnter2D (Collision2D _colInfo)
	{
		SecondPlayerScript _secondPlayerScript = _colInfo.collider.GetComponent<SecondPlayerScript> ();
		if (_secondPlayerScript != null) 
		{
			if (GameObject.FindGameObjectWithTag ("EnemyBoss")) 
			{
				_secondPlayerScript.DamagePlayer (stats.damage);
				DamageEnemy (100);
			} 
			else 
			{
				_secondPlayerScript.DamagePlayer (stats.damage);
				DamageEnemy (300);
			}
		}
			
	}

	void OnDestroy ()
	{
		GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
	}
}
