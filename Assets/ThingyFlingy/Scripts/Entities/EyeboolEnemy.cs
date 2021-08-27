using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Hover))]
public class EyeboolEnemy : Enemy
{
	[SerializeField] private float moveForce;
	[SerializeField] private float rotTorque;
	[SerializeField] private float maxPlayerDistance;
	private GameObject player;
	private Rigidbody rb;

	private void TurnTowards(Vector3 _position)
	{
		Quaternion rotToPosition = Quaternion.FromToRotation(transform.forward, _position);
		Quaternion rotToKeepUpright = Quaternion.FromToRotation(transform.up, Vector3.up);
		rb.AddRelativeTorque(new Vector3(rotToPosition.x, rotToPosition.y, rotToKeepUpright.z) * rotTorque);
	}

	private void TravelTowards(Vector3 _position)
	{
		rb.AddForce((_position - transform.position).normalized * moveForce);
	}

	private Vector3 currentWanderPosition;
	private void Wander()
	{
		
	}
	
	protected override void OnStart()
	{
		player = GameObject.FindWithTag("Player");
		rb = gameObject.GetComponent<Rigidbody>();
	}
}
