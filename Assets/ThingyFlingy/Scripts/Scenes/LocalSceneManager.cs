using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalSceneManager : MonoBehaviour
{
    public delegate void DoThing();

    /// <summary>
    /// Reloads the current scene
    /// </summary>
    public void ReloadScene()
    {
        int currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
    }
    
    /// <summary>
    /// Reloads the current scen after the given number of seconds
    /// </summary>
    public void ReloadSceneAfterSeconds(float _seconds)
    {
        if(!dothingAfterSecondsRunning)
        {
            StartCoroutine(DoThingAfterSeconds(_seconds, ReloadScene));
        }
    }

    /// <summary>
    /// Exits the game.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
    
    /// <summary>
    /// Waits for the given number of seconds before quitting the game.
    /// </summary>
    public void QuitGameAfterSeconds(float _seconds)
    {
        if(!dothingAfterSecondsRunning)
        {
            StartCoroutine(DoThingAfterSeconds(_seconds, QuitGame));
        }
    }

    /// <summary>
    /// Loads the scene with the given scene index
    /// </summary>
    public void LoadScene(int _sceneIndex)
    {
        if(_sceneIndex < SceneManager.sceneCountInBuildSettings && _sceneIndex > -1)
        {
            SceneManager.LoadScene(_sceneIndex);
        }
    }

    private bool dothingAfterSecondsRunning = false;
    /// <summary>
    /// performs the given function after the given number of seconds.
    /// </summary>
    private IEnumerator DoThingAfterSeconds(float _seconds, DoThing _thingToDo)
    {
        dothingAfterSecondsRunning = true;
        yield return new WaitForSeconds(_seconds);
        dothingAfterSecondsRunning = false;
        _thingToDo.Invoke();
    }
}
