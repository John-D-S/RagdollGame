using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public delegate void DoThing();
    
    public void ReloadScene()
    {
        int currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
    }
    
    public void RestartGameAfterSeconds(float _seconds)
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

    private bool dothingAfterSecondsRunning = false;
    private IEnumerator DoThingAfterSeconds(float _seconds, DoThing _thingToDo)
    {
        dothingAfterSecondsRunning = true;
        yield return new WaitForSeconds(_seconds);
        dothingAfterSecondsRunning = false;
        _thingToDo.Invoke();
    }
}
