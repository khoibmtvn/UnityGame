using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour {

	public int levelLoad = 1;

	public string haveMoney = "HaveMoney";
	public string noMoney = "NoMoney";

	public int moneySpent = 100;

	GameMaster gm;
	AudioManager audioManager;

	// Use this for initialization
	void Start () 
	{
		gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster> ();
		audioManager = AudioManager.instance;
		if (audioManager == null)
		{
			Debug.LogError ("No audioManager reference to AudioManager.instance !!!!");
		}
			
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag ("Player") && GameMaster.enemyCounter >= GameMaster.enemyCounterKillCondition) {
			SaveScore ();
			gm.InputText.text = ("Press E and spent " + moneySpent + " to find BOSS !!");
		}
		else 
		{
			gm.InputText.text = ("Not Kill enough enemy or Money");
		}
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if(col.CompareTag("Player") && GameMaster.enemyCounter >= GameMaster.enemyCounterKillCondition)
		{
			if (Input.GetKey(KeyCode.E))
			{
				if (GameMaster.Money >= moneySpent) {
					audioManager.PlaySound (haveMoney);
					GameMaster.Money -= moneySpent;
					SaveScore ();
					SceneManager.LoadScene (levelLoad);
				} 
				else 
				{
					audioManager.PlaySound (noMoney);
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if(col.CompareTag("Player"))
		{
			gm.InputText.text = ("");
		}
	}

	void SaveScore()
	{
		PlayerPrefs.SetInt ("Money", gm.moneyThroughScene);
	}
}
