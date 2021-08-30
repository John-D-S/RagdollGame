using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroySecondsAfterSceneReload : MonoBehaviour
{
	[SerializeField] private float secondsAfterSceneLoadToChange;

	private float lastFrameTimeSinceLevelLoad;
	void Start()
	{
		DontDestroyOnLoad(gameObject);
	}
	
	private void Update()
	{
		if(Time.timeSinceLevelLoad < lastFrameTimeSinceLevelLoad)
		{
			StartCoroutine(DestroyAfterTime());
		}
		lastFrameTimeSinceLevelLoad = Time.timeSinceLevelLoad;
	}

	private IEnumerator DestroyAfterTime()
	{
		yield return secondsAfterSceneLoadToChange;
		Destroy(gameObject);
	}
}