using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Rigidbody))]
public class ThingyPhysics : MonoBehaviour
{
    [SerializeField] private List<CollisionDetector> collisionDetectors = new List<CollisionDetector>();
    private bool IsOnGround
    {
        get
        {
            foreach(CollisionDetector collisionDetector in collisionDetectors)
                if(collisionDetector.IsColliding)
                    return true;
            return false;
        }
    }
    [SerializeField] private float standingForce = 3f;
    [SerializeField] private float standingTorque = 1f;

    private Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(IsOnGround)
        {
            rigidbody.AddForce(Vector3.up * standingForce);
        }
    }
}
