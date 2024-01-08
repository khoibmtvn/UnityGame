using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

	[SerializeField]
	string mouseHoverSound = "ButtonHover";

	[SerializeField]
	string buttonPressSound = "ButtonPress";

	//string sceneName = "MainLevel";
	string sceneName = "MainMenu";

	bool pause = false;

	AudioManager audioManager;

	GameMaster gm;

	void Start ()
	{
		//caching
		audioManager = AudioManager.instance;
		if (audioManager == null) 
		{
			Debug.LogError ("FREAK OUT! No AudioManager found in the scene.");
		}
	}


	public void Quit ()
	{
		audioManager.PlaySound (buttonPressSound);

		Debug.Log ("APPLICATION QUIT !!");
		Application.Quit ();
	}

	public void Retry ()
	{
		audioManager.PlaySound (buttonPressSound);

		//Application.LoadLevel(Application.loadedLevel);
		//SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);

		SceneManager.LoadScene (sceneName);

	}

	public void Resume ()
	{
		audioManager.PlaySound (buttonPressSound);


	}

	public void OnMouseOver ()
	{
		audioManager.PlaySound (mouseHoverSound);

	}

}
