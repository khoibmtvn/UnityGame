using UnityEngine;
using UnityEngine.UI;

public class EnemyKillUI : MonoBehaviour {

	public Text enemyKillText;

	void Awake ()
	{
		enemyKillText = GetComponent<Text> ();
	}

	void Update ()
	{
		enemyKillText.text = "Enemy Kill: " + GameMaster.enemyCounter.ToString () + "/" + GameMaster.enemyCounterKillCondition.ToString ();
	}

}
