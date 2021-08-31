using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuHandler : MonoBehaviour
{
	[SerializeField] private GameObject pauseMenu;
	private bool isPaused = false;
	public static bool IsPaused => thePauseMenuHandler.isPaused;
	private static PauseMenuHandler thePauseMenuHandler;
	
	private void Start()
	{
		thePauseMenuHandler = this;
		pauseMenu.SetActive(isPaused);
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	public void TogglePause()
	{
		if(isPaused)
		{
			Time.timeScale = 1;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			pauseMenu.SetActive(false);
			isPaused = false;
		}
		else
		{
			Time.timeScale = 0;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			pauseMenu.SetActive(true);
			isPaused = true;
		}
	}
	
	private void Update()
	{
		if(Input.GetButtonDown("Cancel") && pauseMenu)
		{
			TogglePause();
		}
	}
}
