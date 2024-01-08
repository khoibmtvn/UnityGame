using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour {

	[SerializeField]
	private Text healthText;

	[SerializeField]
	private Text speedText;

	[SerializeField]
	private Text damageText;

	[SerializeField]
	private float healthMultiplier = 1.3f;

	[SerializeField]
	private float movementSpeedMultiplier = 1.3f;

	[SerializeField]
	private float damageMultiplier = 1.3f;

	[SerializeField]
	private int upgradeCost = 50; 

	private PlayerStats stats; 
	private Weapon Wstats;

	void OnEnable ()
	{
		stats = PlayerStats.instance;
		Wstats = Weapon.instance;
		UpdateValues ();
	}

	void UpdateValues ()
	{
		healthText.text = "HEALTH: " + stats.maxHealth.ToString ();
		speedText.text = "SPEED: " + stats.movementSpeed.ToString ();
		damageText.text = "DAMAGE: " + Wstats.Damage.ToString ();
	}

	public void UpgradeHealth ()
	{
		if (GameMaster.Money < upgradeCost)
		{
			AudioManager.instance.PlaySound ("NoMoney");
			return;
		}

		Debug.Log ("Health inscreased !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

		stats.maxHealth = (int)(stats.maxHealth * healthMultiplier);

		GameMaster.Money -= upgradeCost;
		AudioManager.instance.PlaySound ("Money");

		UpdateValues ();
	}

	public void UpgradeSpeed ()
	{
		if (GameMaster.Money < upgradeCost)
		{
			AudioManager.instance.PlaySound ("NoMoney");
			return;
		}

		stats.movementSpeed = Mathf.Round(stats.movementSpeed * movementSpeedMultiplier);

		GameMaster.Money -= upgradeCost;
		AudioManager.instance.PlaySound ("Money");

		UpdateValues ();
	}
	public void UpgradeDamage ()
	{
		if (GameMaster.Money < upgradeCost)
		{
			AudioManager.instance.PlaySound ("NoMoney");
			return;
		}

		Wstats.Damage = (int)(Wstats.Damage * damageMultiplier);

		GameMaster.Money -= upgradeCost;
		AudioManager.instance.PlaySound ("Money");

		UpdateValues ();
	}

}
