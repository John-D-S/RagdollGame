using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class PointBlock : MonoBehaviour
{
	[SerializeField, Tooltip("The mesh renderer to generate a color for.")] private MeshRenderer meshRenderer;
	[SerializeField, Tooltip("A random color will be generated between this and Color 2")] private Color color1;
	[SerializeField, Tooltip("A random color will be generated between this and Color 1")] private Color color2;
	
	private void Start()
	{
		// if the meshrenederer is assigned, set its color to a random color between color1 and color2
		if(meshRenderer)
		{
			meshRenderer.material.color = Color.Lerp(color1, color2, Random.Range(0f, 1f)); 
		}
		
		// add one to the total number of blocks remaining.
		if(PointBlockManager.thePointBlockManager)
		{
			PointBlockManager.thePointBlockManager.BlocksRemaining++;
		}
	}

	private void OnDestroy()
	{
		// remove one from the total number of blocks remaining.
		if(PointBlockManager.thePointBlockManager)
		{
			PointBlockManager.thePointBlockManager.BlocksRemaining--;
		}
	}
}
