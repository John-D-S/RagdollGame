using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// unused.
/// </summary>
[RequireComponent(typeof(Hover))]
public class EyeboolEnemy : Enemy
{
	[SerializeField, Tooltip("The force applied to the eyebool to move it.")] private float moveForce;
	[SerializeField, Tooltip("The torque applied to the eyebool to rotate it.")] private float rotTorque;
	[SerializeField, Tooltip("The maximum distance the eyebool will be from the player before its AI will cause it to travel towards it.")] private float maxPlayerDistance;
	private GameObject player;
	private Rigidbody rb;

	/// <summary>
	/// apply torque to turn towards the given position.
	/// </summary>
	private void TurnTowards(Vector3 _position)
	{
		Quaternion rotToPosition = Quaternion.FromToRotation(transform.forward, _position);
		Quaternion rotToKeepUpright = Quaternion.FromToRotation(transform.up, Vector3.up);
		rb.AddRelativeTorque(new Vector3(rotToPosition.x, rotToPosition.y, rotToKeepUpright.z) * rotTorque);
	}

	/// <summary>
	/// apply force to travel towards a given position.
	/// </summary>
	private void TravelTowards(Vector3 _position)
	{
		rb.AddForce((_position - transform.position).normalized * moveForce);
	}
	
	/// <summary>
	/// set the player and the rigidbody.
	/// </summary>
	protected override void OnStart()
	{
		player = GameObject.FindWithTag("Player");
		rb = gameObject.GetComponent<Rigidbody>();
	}
}
