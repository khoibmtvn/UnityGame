using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour 
{
	[SerializeField]
	string hoverOverSound = "ButtonHover";

	[SerializeField]
	string pressButtonSound = "ButtonPress";

	[SerializeField]
	string resumeSceneName = "MainMenu";

	[SerializeField]
	string howToPlaySceneName = "HowToPlay";

	[SerializeField]
	string storySceneName = "Story";
	
	AudioManager audioManager;

	void Start ()
	{
		audioManager = AudioManager.instance;
		if (audioManager == null) 
		{
			Debug.Log ("No audioManager Found!!");
		}
	}

	public void StartGame ()
	{
		audioManager.PlaySound (pressButtonSound);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void HowToPlay ()
	{
		audioManager.PlaySound (pressButtonSound);
		SceneManager.LoadScene(howToPlaySceneName);
	}

	public void Resume ()
	{
		audioManager.PlaySound (pressButtonSound);
		SceneManager.LoadScene(resumeSceneName);
	}

	public void Story ()
	{
		audioManager.PlaySound (pressButtonSound);
		SceneManager.LoadScene(storySceneName);
	}

	public void QuitGame()
	{
		audioManager.PlaySound (pressButtonSound);
		Debug.Log("WE QUIT THE GAME!");
		Application.Quit();
	}

	public void OnMouseOver ()
	{
		audioManager.PlaySound (hoverOverSound);
	}


}
