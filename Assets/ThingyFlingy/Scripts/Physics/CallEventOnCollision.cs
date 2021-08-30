using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CallEventOnCollision : MonoBehaviour
{
	[SerializeField] private bool canOnlyBeCalledOnce;
	[SerializeField] private UnityEvent activationEvent;

	private bool hasBeenActivated = false;
	private void OnCollisionEnter(Collision other)
	{
		if(other.collider.gameObject.CompareTag("Collision Activator") && !hasBeenActivated)
		{
			if(canOnlyBeCalledOnce)
			{
				hasBeenActivated = true;
			}
			activationEvent.Invoke();
		}
	}
}
