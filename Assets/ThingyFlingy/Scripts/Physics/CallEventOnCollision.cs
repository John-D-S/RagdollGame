using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CallEventOnCollision : MonoBehaviour
{
	[SerializeField] private UnityEvent activationEvent;

	private void OnCollisionEnter(Collision other)
	{
		if(other.collider.gameObject.CompareTag("Collision Activator"))
		{
			activationEvent.Invoke();
		}
	}
}
