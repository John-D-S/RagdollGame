using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class ThingyForceField : MonoBehaviour
{
    [SerializeField] private float outerFieldRadius = 5;
    [SerializeField] private float outerFieldAttractionStrength = 1;
    [SerializeField] private float innerFieldRadius = 2;
    [SerializeField] private float innerFieldRepulsionStrength = 1;
    [SerializeField] private float noForceRadius = 0.25f;

    [SerializeField] private float antiEscapeForceMultiplier;

    private Vector3 thisFamePosition;
    private Vector3 lastFramePosition;
    private Vector3 thisFrameVelocity;
    private Vector3 lastFrameVelocity;
    private Vector3 accelleration;
    
    private void UpdateAccelleration()
    {
        lastFramePosition = thisFamePosition;
        thisFamePosition = transform.position;
        lastFrameVelocity = thisFrameVelocity;
        thisFrameVelocity = thisFamePosition - lastFramePosition;
        accelleration = (thisFrameVelocity - lastFrameVelocity) / Time.fixedDeltaTime;
        //Debug.Log($"thisFrameSpeed = {thisFrameVelocity.magnitude}, accelleration = {accelleration.magnitude}");
    }
    
    private Vector3 RepulsionForce(Vector3 _position)
    {
        Vector3 positionRelativeToFieldCenter = gameObject.transform.position - _position;
        Vector3 directionToApplyForceIn = -positionRelativeToFieldCenter.normalized;
        float distanceToCenter = positionRelativeToFieldCenter.magnitude;
        return directionToApplyForceIn * (innerFieldRadius * innerFieldRepulsionStrength / distanceToCenter);
    }
    
    private Vector3 AttractionForce(Vector3 _position)
    {
        Vector3 positionRelativeToFieldCenter = gameObject.transform.position - _position;
        Vector3 directionToApplyForceIn = positionRelativeToFieldCenter.normalized;
        float distanceToCenter = positionRelativeToFieldCenter.magnitude;
        return directionToApplyForceIn * (outerFieldRadius * outerFieldAttractionStrength / distanceToCenter);
    }

    private Vector3 AntiGravityForce()
    {
        return -Physics.gravity;
    }

    private Vector3 AntiEscapeForce(Rigidbody _rigidbody)
    {
        float dotFromPosToRBPos = Vector3.Dot(_rigidbody.velocity, _rigidbody.position - transform.position);
        Vector3 relativeRigidbodyVelocity = _rigidbody.velocity - thisFrameVelocity;
        if(dotFromPosToRBPos > 0)
        {
            return -relativeRigidbodyVelocity * (antiEscapeForceMultiplier * dotFromPosToRBPos);
        }
        return Vector3.zero;
    }
    
    private void ApplyForceField(ref Rigidbody _rigidbody)
    {
        Vector3 rbPosition = _rigidbody.position;
        Vector3 forceToAdd = new Vector3();
        float distanceToRB = Vector3.Distance(transform.position, rbPosition);
        if(distanceToRB < noForceRadius) { /*do nothing*/ }
        else if(distanceToRB < innerFieldRadius)
        {
            forceToAdd += RepulsionForce(rbPosition);
        }
        else if(distanceToRB < outerFieldRadius)
        {
            forceToAdd += AttractionForce(rbPosition);
        }
        forceToAdd += AntiGravityForce();
        forceToAdd += AntiEscapeForce(_rigidbody);
        forceToAdd *= _rigidbody.mass;
        _rigidbody.AddForce(forceToAdd);
        _rigidbody.velocity += accelleration;
    }

    private List<ThingyPhysics> GetThingiesInForceField()
    {
        List<ThingyPhysics> returnValue = new List<ThingyPhysics>();
        List<Collider> collidersWithinForceField = Physics.OverlapSphere(transform.position, outerFieldRadius).ToList();
        foreach(Collider collider in collidersWithinForceField)
        {
            if(ThingyPhysics.gameObjectToThingyMap.ContainsKey(collider.gameObject) && !returnValue.Contains(ThingyPhysics.gameObjectToThingyMap[collider.gameObject]))
            {
                returnValue.Add(ThingyPhysics.gameObjectToThingyMap[collider.gameObject]);
            }
        }
        return returnValue;
    }

    private void Start()
    {
        thisFamePosition = lastFramePosition = transform.position;
    }

    public void ShootThingy()
    {
        List<ThingyPhysics> thingysInForceField = GetThingiesInForceField();
        ThingyPhysics thingyPhysicsToShoot;
        foreach(ThingyPhysics thingyPhysics in thingysInForceField)
        {
            foreach(Rigidbody iteratedRigidbody in thingyPhysics.ConnectedRigidbodies)
            {
                Rigidbody thingyRigidbody = iteratedRigidbody;
                ApplyForceField(ref thingyRigidbody);
            }
        }
    }
    
    private void FixedUpdate()
    {
        UpdateAccelleration();
        List<ThingyPhysics> thingysInForceField = GetThingiesInForceField();
        foreach(ThingyPhysics thingyPhysics in thingysInForceField)
        {
            foreach(Rigidbody iteratedRigidbody in thingyPhysics.ConnectedRigidbodies)
            {
                Rigidbody thingyRigidbody = iteratedRigidbody;
                ApplyForceField(ref thingyRigidbody);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, outerFieldRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, innerFieldRadius);
    }
}
