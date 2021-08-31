using UnityEngine;

public class PauseMenuHandler : MonoBehaviour
{
	[SerializeField, Tooltip("The UI pause menu.")] private GameObject pauseMenu;
	private bool isPaused = false;
	public static bool IsPaused => thePauseMenuHandler.isPaused;
	//the pause menu handler singleton.
	private static PauseMenuHandler thePauseMenuHandler;
	
	private void Start()
	{
		//set the pauseMenuHandler singleton.
		thePauseMenuHandler = this;
		UnPause();
	}

	/// <summary>
	/// UnPauses the game
	/// </summary>
	private void UnPause()
	{
		Time.timeScale = 1;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		pauseMenu.SetActive(false);
		isPaused = false;
	}

	/// <summary>
	/// Pauses the game
	/// </summary>
	private void Pause()
	{
		Time.timeScale = 0;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		pauseMenu.SetActive(true);
		isPaused = true;
	}
	
	/// <summary>
	/// Toggle the game being paused
	/// </summary>
	public void TogglePause()
	{
		if(isPaused)
		{
			UnPause();
		}
		else
		{
			Pause();
		}
	}
	
	private void Update()
	{
		//Toggle pause when the escape key is pressed.
		if(Input.GetButtonDown("Cancel") && pauseMenu)
		{
			TogglePause();
		}
	}
}
