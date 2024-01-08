using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;

	[SerializeField]
	private int maxLives = 3;

	private static int _remainingLives;
	public static int RemainingLives
	{
		get { return _remainingLives;}

	}
		
	[SerializeField]
	private int startingMoney = 100;
	public static int Money;
	public int moneyThroughScene;


	void Awake () 
	{
		if (gm == null) {
			gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
		}
			
	}

	public Transform playerPrefab;
	public Transform spawnPoint;
	public float spawnDelay = 2; 
	public Transform spawnPrefab;
	public AudioClip spawnSound;
	public string respawnCountdownSoundName = "RespawnCountdown";
	public string spawnSoundName = "Spawn";

	public string gameOverSoundName = "GameOver";

	public string upgradeMenuSoundName = "UpgradeMenu";

	public CameraShake cameraShake;

	[SerializeField]
	private GameObject gameOverUI;
	[SerializeField]
	private GameObject gameWinUI;

	[SerializeField]
	private GameObject upgradeMenu;
	[SerializeField]
	private GameObject pauseMenu;

	public delegate void UpgradeMenuCallBack(bool active);
	public UpgradeMenuCallBack onToggleUpgradeMenu;

	public delegate void PauseMenuCallBack(bool active);
	public PauseMenuCallBack onTogglePauseMenu;

	//Cache
	private AudioManager audioManager;

	public float spawnVolume = 5.5f;

	public int score = 0;

	[SerializeField]
	private WaveSpawner waveSpawner;

	public Text InputText;

	public bool bossDead = false;

	public static int enemyCounter = 0;
	public static int enemyCounterKillCondition;

	public int _enemyCouterKillCondition;

	void Start ()
	{
		if (cameraShake == null)
		{
			Debug.LogError ("No camera shake referenced on GameMaster");
		}

		//Lam cho sinh mang cua nhan vat thay doi khi loadScene
		//Tranh truong hop bi giu nguyen gia tri
		_remainingLives = maxLives;

		Money = startingMoney;
		moneyThroughScene = startingMoney;

		//Enemy Counting
		enemyCounter = 0;
		//Enemy Counter Kill Condition
		enemyCounterKillCondition = _enemyCouterKillCondition;

		//caching
		audioManager = AudioManager.instance;
		if (audioManager == null) 
		{
			Debug.LogError ("FREAK OUT! No AudioManager found in the scene.");
		}

		moneyThroughScene = PlayerPrefs.GetInt ("Money", 0);

		if(PlayerPrefs.HasKey("Money"))
		{
			
			Scene ActiveScene = SceneManager.GetActiveScene ();
			if (ActiveScene.buildIndex == 0) {
				PlayerPrefs.DeleteKey ("Money");
				Money = startingMoney;
			}
			else 
			{
				moneyThroughScene = PlayerPrefs.GetInt ("Money");
				Money = moneyThroughScene;
			}
		}

	}

	void Update ()
	{
		//scoreText.text = ("Score: " + score.ToString() );

		//Neu an vao phim "U" thi UpgradeMenu se hien ra
		if (Input.GetKeyDown(KeyCode.U))
		{
			audioManager.PlaySound (upgradeMenuSoundName);
			ToggleUpgradeMenu ();
		}

		//Neu an vao nut P thi PauseMenu se hien ra
		if (Input.GetKeyDown (KeyCode.P))
		{
			audioManager.PlaySound (upgradeMenuSoundName);
			TogglePauseMenu ();
		}
			
		BossCheck ();
			
	}

	void BossCheck ()
	{
		string curScene = SceneManager.GetActiveScene().name;
		string finalSceneName = "MainLevel 2";

		GameObject BossTag = GameObject.FindGameObjectWithTag ("EnemyBoss");
		if (BossTag != null) 
		{
			bossDead = false;
		}
		if (BossTag == null)
		{
			bossDead = true;
		}

		if (bossDead == true && curScene == finalSceneName) 
		{
			waveSpawner.enabled = false;
			onToggleUpgradeMenu.Invoke (true);
			EndGameWhenBossDie ();

		}
	}


	private void TogglePauseMenu ()
	{
		pauseMenu.SetActive (!pauseMenu.activeSelf);
		waveSpawner.enabled = !pauseMenu.activeSelf;
		//onTogglePauseMenu.Invoke (pauseMenu.activeSelf);
		onToggleUpgradeMenu.Invoke (pauseMenu.activeSelf);
	}


	private void ToggleUpgradeMenu ()
	{
		upgradeMenu.SetActive (!upgradeMenu.activeSelf);
		waveSpawner.enabled = !upgradeMenu.activeSelf;
		onToggleUpgradeMenu.Invoke (upgradeMenu.activeSelf);
	}

	public void EndGameWhenBossDie ()
	{
		Debug.Log ("YOU WIN THE GAME");
		gameWinUI.SetActive (true);
		//enemyCounter = 0;
	}


	public void EndGame ()
	{
		audioManager.PlaySound (gameOverSoundName);

		Debug.Log ("GAMEOVER");
		gameOverUI.SetActive (true);
	}

	public IEnumerator RespawnPlayer ()
	{
		audioManager.PlaySound (respawnCountdownSoundName);
		yield return new WaitForSeconds (spawnDelay);

		audioManager.PlaySound (spawnSoundName);
		Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);
		Transform clone = Instantiate (spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
		Destroy (clone.gameObject, 3f);
	}

	public static void KillPlayer (SecondPlayerScript secondPlayerScript)
	{
		Destroy (secondPlayerScript.gameObject);
		_remainingLives -= 1;
		if (_remainingLives <= 0) 
		{
			GameObject BossTag = GameObject.FindGameObjectWithTag ("EnemyBoss");

			if (gm.bossDead == true) 
			{
				_remainingLives += 1;
				if (BossTag == null) 
				{
					gm.EndGame ();
					//enemyCounter = 0;
				}
			}

			if (gm.bossDead == false) 
			{
				gm.EndGame ();
				//enemyCounter = 0;
			}
		} 
		else
		{
			//Debug.Log ("Player Killed !!");
			gm.StartCoroutine (gm.RespawnPlayer() );
			//Debug.Log ("Player Respawn !!");
		}

	}

	public static void KillEnemy (Enemy enemy)
	{
		gm._KillEnemy (enemy);
	}

	public void _KillEnemy (Enemy _enemy)
	{
		//Let's play some sound
		audioManager.PlaySound (_enemy.deathSoundName);

		//Gain some Money
		Money += _enemy.moneyDrop;
		audioManager.PlaySound ("Money");

		//Enemy Counting
		enemyCounter += 1;

		//Get score when kill enemy
		score += 10;

		//Add Particle
		Transform _clone = Instantiate (_enemy.deathParticle, _enemy.transform.position, Quaternion.identity);
		Destroy (_clone.gameObject, 5f);

		//Go CameraShake
		cameraShake.Shake (_enemy.shakeAmt, _enemy.shakeLength);
		Destroy (_enemy.gameObject);
	}

}
