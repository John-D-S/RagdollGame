using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Rigidbody), typeof(ConfigurableJoint))]
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
    [SerializeField] private float standingBalanceSpring = 1f;
    [SerializeField] private float standingBalanceDamper = 1f;

    private Rigidbody rb;
    private ConfigurableJoint configurableJoint;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        configurableJoint = GetComponent<ConfigurableJoint>();
    }

    private void UpdateJoint( JointDrive _jointDrive, SoftJointLimitSpring _softJointLimitSpring)
    {
        configurableJoint.angularXLimitSpring = _softJointLimitSpring;
        //configurableJoint.xDrive = _jointDrive;
        //configurableJoint.yDrive = _jointDrive;
        //configurableJoint.zDrive = _jointDrive;
    }

    private void UpdateJointSettings(ref JointDrive _jointDrive, ref SoftJointLimitSpring _softJointLimitSpring)
    {
        //linear drive stuff
        //configurableJoint.targetPosition = transform.position + Vector3.up * 100;
        //configurableJoint.targetVelocity = Vector3.one * standingSpeed;
        //_jointDrive.positionSpring = standingSpring;
        //Debug.Log(_jointDrive.positionSpring);
        //_jointDrive.positionDamper = 1;
        //_jointDrive.maximumForce = standingForce;
        //angular balancing stuff
        _softJointLimitSpring.damper = standingBalanceDamper;
        _softJointLimitSpring.spring = standingBalanceSpring;
    }

    private void FixedUpdate()
    {
        SoftJointLimitSpring standingBalancer = new SoftJointLimitSpring();
        JointDrive standingForceDrive = new JointDrive();
        UpdateJointSettings(ref standingForceDrive, ref standingBalancer);
        UpdateJoint(standingForceDrive, standingBalancer);
        if(IsOnGround)
        {
            currentStandingForce = maxStandingForce;
            configurableJoint.angularXMotion = ConfigurableJointMotion.Limited;
            configurableJoint.angularZMotion = ConfigurableJointMotion.Limited;
        }
        else
        {
            if(currentStandingForce > 0)
                currentStandingForce -= maxStandingForce * Time.fixedDeltaTime / standingForceDepletionTime;
            else
                currentStandingForce = 0;
            configurableJoint.angularXMotion = ConfigurableJointMotion.Free;
            configurableJoint.angularZMotion = ConfigurableJointMotion.Free;
        }
        if(currentStandingForce > 0)
            rb.AddForce(Vector3.up * currentStandingForce);
    }
}
