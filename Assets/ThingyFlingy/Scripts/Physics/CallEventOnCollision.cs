using UnityEngine;
using UnityEngine.Events;

public class CallEventOnCollision : MonoBehaviour
{
	[SerializeField, Tooltip("If true, the activation event can only be called once.")] private bool canOnlyBeCalledOnce;
	[SerializeField, Tooltip("The event to call when this object collides with an object with the Collision Activator tag.")] private UnityEvent activationEvent;

	private bool hasBeenActivated = false;
	private void OnCollisionEnter(Collision other)
	{
		//if the gameobject collides with an object with another object with the tag "Collision Activator", invoke the event.
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
