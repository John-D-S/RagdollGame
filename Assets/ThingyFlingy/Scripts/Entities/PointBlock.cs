using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class PointBlock : MonoBehaviour
{
	[SerializeField] private MeshRenderer meshRenderer;
	[SerializeField] private Color color1;
	[SerializeField] private Color color2;
	
	private void Start()
	{
		if(meshRenderer)
		{
			meshRenderer.material.color = Color.Lerp(color1, color2, Random.Range(0f, 1f)); 
		}
		
		if(PointBlockManager.thePointBlockManager)
		{
			PointBlockManager.thePointBlockManager.BlocksRemaining++;
		}
	}

	private void OnDestroy()
	{
		if(PointBlockManager.thePointBlockManager)
		{
			PointBlockManager.thePointBlockManager.BlocksRemaining--;
		}
	}
}
