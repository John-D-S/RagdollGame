using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalSceneManager : MonoBehaviour
{
    public delegate void DoThing();
    
    public void ReloadScene()
    {
        int currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
    }
    
    public void ReloadSceneAfterSeconds(float _seconds)
    {
        if(!dothingAfterSecondsRunning)
        {
            StartCoroutine(DoThingAfterSeconds(_seconds, ReloadScene));
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void QuitGameAfterSeconds(float _seconds)
    {
        if(!dothingAfterSecondsRunning)
        {
            StartCoroutine(DoThingAfterSeconds(_seconds, QuitGame));
        }
    }

    public void LoadScene(int _sceneIndex)
    {
        if(_sceneIndex < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings && _sceneIndex > -1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneIndex);
        }
    }

    private bool dothingAfterSecondsRunning = false;
    private IEnumerator DoThingAfterSeconds(float _seconds, DoThing _thingToDo)
    {
        dothingAfterSecondsRunning = true;
        yield return new WaitForSeconds(_seconds);
        dothingAfterSecondsRunning = false;
        _thingToDo.Invoke();
    }
}