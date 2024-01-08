using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[RequireComponent(typeof(Player))]
public class SecondPlayerScript : MonoBehaviour 
{

	public int fallBoundary = -20;

	[SerializeField]
	private StatusIndicator statusIndicator;

	public string deathSoundName = "DeathVoice";
	public string damageSoundName = "Grunt";

	private AudioManager audioManager;

	private PlayerStats stats;

	void Start ()
	{
		stats = PlayerStats.instance;

		if (statusIndicator == null) 
		{
			Debug.LogError ("No status Indicator Referenced on Player");
		} 
		else 
		{
			statusIndicator.SetHealth (stats.curHealth, stats.maxHealth);
		}

		audioManager = AudioManager.instance;
		if (audioManager == null)
		{
			Debug.LogError ("PANIC! No audioManager in scene.");
		}
	
		GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

		InvokeRepeating ("RegenHealth", 1f/stats.healthRegenRate, 1f/stats.healthRegenRate);
	}

	void RegenHealth ()
	{
		stats.curHealth += 1;
		statusIndicator.SetHealth (stats.curHealth, stats.maxHealth);
	}

	void Update () 
	{
		if (transform.position.y <= fallBoundary) 
		{
			DamagePlayer (9999999);
		}
	}

	//Handle what happens when the upgrade menu is toggles
	void OnUpgradeMenuToggle (bool active)
	{
		GetComponent<Player>().enabled = !active;
		Weapon _weapon = GetComponentInChildren<Weapon> ();
		if (_weapon != null) 
		{
			_weapon.enabled = !active;
		}
	}

	//Ngan khong cho xay ra loi khi ke dich hoac nguoi choi bi tieu diet khi bat UpgradeMenu
	void OnDestroy ()
	{
		GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
	}

	public void DamagePlayer (int damage)
	{
		stats.curHealth -= damage;
		if (stats.curHealth <= 0) 
		{
			//Play death sound
			audioManager.PlaySound (deathSoundName);
			Debug.Log ("KILL PLAYER");
			//Kill player
			GameMaster.KillPlayer (this);
		} 
		else 
		{
			//Play damage sound
			audioManager.PlaySound (damageSoundName);
		}

		statusIndicator.SetHealth (stats.curHealth, stats.maxHealth);
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag ("Medicine")) 
		{
			Destroy (col.gameObject);
			stats.ItemCondition = true;
		}
	}
		

}
