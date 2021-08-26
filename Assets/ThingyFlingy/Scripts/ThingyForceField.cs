using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingyForceField : MonoBehaviour
{
    [SerializeField] private float outerFieldRadius;
    [SerializeField] private float outerFieldAttractionStrength;
    [SerializeField] private float innerFieldRadius;
    [SerializeField] private float innerFieldRepulsionStrength;

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
    
    private void ApplyForceField(ref Rigidbody _rigidbody)
    {
        Vector3 rbPosition = _rigidbody.position;
        float rbDistanceFromCenterOfForceField;
        Vector3 forceToAdd = new Vector3();
        if(Vector3.Distance(transform.position, rbPosition) < innerFieldRadius)
        {
            forceToAdd += RepulsionForce(rbPosition);
        }
        else if  (Vector3.Distance(transform.position, rbPosition) < outerFieldRadius)
        {
            forceToAdd += AttractionForce(rbPosition);
        }
        forceToAdd += AntiGravityForce();
        forceToAdd *= _rigidbody.mass;
        _rigidbody.AddForce(forceToAdd);
    }
}
