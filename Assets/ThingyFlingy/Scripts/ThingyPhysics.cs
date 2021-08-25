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

    
    [SerializeField] private float maxStandingForce = 3f;
    [SerializeField] private float standingForceDepletionTime = 1f;
    private float currentStandingForce;
    [SerializeField, Tooltip("I assume this means torque")] private float UprightTorque = 1f;
    private float currentUprightTorque;

    private ConfigurableJoint configurableJoint; 
    private Rigidbody rb;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        configurableJoint = GetComponent<ConfigurableJoint>();
        //initialising hingeJointMotors
    }

    private void FixedUpdate()
    {
        
        if(IsOnGround)
        {
            currentStandingForce = maxStandingForce;
            if(currentUprightTorque < UprightTorque)
                currentUprightTorque += UprightTorque * Time.fixedDeltaTime / standingForceDepletionTime;
            else
                currentUprightTorque = UprightTorque;
        }
        else
        {
            if(currentUprightTorque > 0)
                currentUprightTorque -= UprightTorque * Time.fixedDeltaTime / standingForceDepletionTime;
            else
                currentUprightTorque = 0;
            
            if(currentStandingForce > 0)
                currentStandingForce -= maxStandingForce * Time.fixedDeltaTime / standingForceDepletionTime;
            else
                currentStandingForce = 0;    
        }
        
        Quaternion rot = Quaternion.FromToRotation(transform.up, Vector3.up);
        rb.AddRelativeTorque(new Vector3(rot.x, rot.y, rot.z) * currentUprightTorque);
        if(currentStandingForce > 0)
            rb.AddForce(Vector3.up * currentStandingForce);
    }
}
