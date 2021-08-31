using UnityEngine;

public class RespawnOutOfBounds : MonoBehaviour
{
	// The position to respawn the gameobject when it moves out of bounds
	private Vector3 respawnPosition;
	// the rigidbody of the gameobject
	private Rigidbody rb;
	
	private void Start()
	{
		// set the respawn position to be the position the gameobject starts out in
		respawnPosition = transform.position;
		// try to get the gameobjects rigidbody
		rb = gameObject.GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		// if the bounds exist and this gamobject is outside of them, teleport it back to the position it started out in
		if(Bounds.theBounds && !Bounds.theBounds.IsInsideBounds(transform.position))
		{
			//set the position to the respawn position.
			transform.position = respawnPosition;
			//set the velocity to zero.
			if(rb)
			{
				rb.velocity = Vector3.zero;
			}
		}
	}
}
