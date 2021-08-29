using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnOutOfBounds : MonoBehaviour
{
	private Vector3 respawnPosition;
	private Rigidbody rb;
	
	private void Start()
	{
		respawnPosition = transform.position;
		rb = gameObject.GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		if(Bounds.theBounds && !Bounds.theBounds.IsInsideBounds(transform.position))
		{
			transform.position = respawnPosition;
			if(rb)
			{
				rb.velocity = Vector3.zero;
			}
		}
	}
}
