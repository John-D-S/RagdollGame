using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroySecondsAfterSceneReload : MonoBehaviour
{
	[SerializeField, Tooltip("The number of seconds after the scene changes to destroy this Object")] private float secondsAfterSceneLoadToDestroy;

	private float lastFrameTimeSinceLevelLoad;
	void Start()
	{
		//set this gameobject to not be destroyed when a scene loads.
		DontDestroyOnLoad(gameObject);
	}
	
	private void Update()
	{
		//if lastFrameTimeSinceLevelLoad is greater than time.timeSinceLevelLoad, that means that the scene has changed, so start counting down
		if(Time.timeSinceLevelLoad < lastFrameTimeSinceLevelLoad)
		{
			StartCoroutine(DestroyAfterTime());
		}
		lastFrameTimeSinceLevelLoad = Time.timeSinceLevelLoad;
	}

	/// <summary>
	/// destroy this gameobject after secondsAfterSceneLoadToChange
	/// </summary>
	/// <returns></returns>
	private IEnumerator DestroyAfterTime()
	{
		yield return secondsAfterSceneLoadToDestroy;
		Destroy(gameObject);
	}
}