using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollisionDetector : MonoBehaviour
{
	private bool isColliding = false;
	public bool IsColliding => isColliding;
	public ThingyPhysics connectedThingyPhysics;
	
	private void OnCollisionEnter(Collision other)
	{
		if(connectedThingyPhysics)
		{
			
		}
	}

	private void OnCollisionStay(Collision other)
	{
		// if the tag of the other collider is not the same as the tag for this gameobject,set isColliding to true
		if(!other.collider.CompareTag(tag))
		{
			isColliding = true;
		}
	}

	private void OnCollisionExit(Collision other)
	{
		// if the tag of the other collider is not the same as the tag for this gameobject,set isColliding to false
		if(!other.collider.CompareTag(tag))
		{
			isColliding = false;
		}
	}
}
