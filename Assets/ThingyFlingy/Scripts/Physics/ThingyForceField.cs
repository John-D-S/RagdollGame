using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class ThingyForceField : MonoBehaviour
{
    [SerializeField, Tooltip("How far away are thingies attracted by the forcefield.")] private float outerFieldRadius = 5;
    [SerializeField, Tooltip("How powerful is the attraction force at the edge of th outer field radius.")] private float outerFieldAttractionStrength = 1;
    [SerializeField, Tooltip("The radius at which thingies are repulsed by the forceField")] private float innerFieldRadius = 2;
    [SerializeField, Tooltip("The strength of the repulsion force at the edge of the inner field radius")] private float innerFieldRepulsionStrength = 1;
    [SerializeField, Tooltip("The radius within which any thingies will experience no force")] private float noForceRadius = 0.25f;
    [SerializeField, Tooltip("How fast thingies are shot out of the forceField")] private float shootVelocity = 10f;
    [SerializeField, Tooltip("How long after shooting a thingy until another thingy can be shot.")] private float coolDownTime = 0.1f;
    [SerializeField, Tooltip("The maximum number of thingies that can be affected by the force field at a given time")] private int maxThingies = 30;
    [SerializeField, Tooltip("The strength of the force applied to each thingy to prevent it escaping from the force field.")] private float antiEscapeForceMultiplier;
    [SerializeField, Tooltip("The Camera used for aiming.")] private GameObject usedCamera;

    //a list of thingies that will not be affected by the thingyForcefield.
    private List<ThingyPhysics> ignoredThingies = new List<ThingyPhysics>();
    
    private Vector3 thisFamePosition;
    private Vector3 lastFramePosition;
    private Vector3 thisFrameVelocity;
    private Vector3 lastFrameVelocity;
    private Vector3 accelleration;
    
    /// <summary>
    /// update the position, velocity and accelleration by finding the deltas of each 
    /// </summary>
    private void UpdateAccelleration()
    {
        lastFramePosition = thisFamePosition;
        thisFamePosition = transform.position;
        lastFrameVelocity = thisFrameVelocity;
        thisFrameVelocity = thisFamePosition - lastFramePosition;
        accelleration = (thisFrameVelocity - lastFrameVelocity) / Time.fixedDeltaTime;
    }
    
    /// <summary>
    /// calculate the repulsion force at a given position
    /// </summary>
    private Vector3 RepulsionForce(Vector3 _position)
    {
        Vector3 positionRelativeToFieldCenter = gameObject.transform.position - _position;
        Vector3 directionToApplyForceIn = -positionRelativeToFieldCenter.normalized;
        float distanceToCenter = positionRelativeToFieldCenter.magnitude;
        return directionToApplyForceIn * (innerFieldRadius * innerFieldRepulsionStrength / distanceToCenter);
    }
    
    /// <summary>
    /// calculate the attraction force at a given position.
    /// </summary>
    private Vector3 AttractionForce(Vector3 _position)
    {
        Vector3 positionRelativeToFieldCenter = gameObject.transform.position - _position;
        Vector3 directionToApplyForceIn = positionRelativeToFieldCenter.normalized;
        float distanceToCenter = positionRelativeToFieldCenter.magnitude;
        return directionToApplyForceIn * (outerFieldRadius * outerFieldAttractionStrength / distanceToCenter);
    }

    /// <summary>
    /// The force required to nullify gravity
    /// </summary>
    private Vector3 AntiGravityForce()
    {
        return -Physics.gravity;
    }

    /// <summary>
    /// The Force required to keep the given rigidbody within the force field.
    /// </summary>
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
    
    /// <summary>
    /// apply the force field to the given rigidbody
    /// </summary>s
    private void ApplyForceField(ref Rigidbody _rigidbody)
    {
        Vector3 rbPosition = _rigidbody.position;
        Vector3 forceToAdd = new Vector3();
        float distanceToRB = Vector3.Distance(transform.position, rbPosition);
        //calculate and add all forces to the forceToAddVariable
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
        //apply the force to the rigidbody
        _rigidbody.AddForce(forceToAdd);
        //add accelleration to the rigidbody's velocity to keep it moving along with the forcefield.
        _rigidbody.velocity += accelleration;
    }

    /// <summary>
    /// return a list of all thingies in the given radius
    /// </summary>
    private List<ThingyPhysics> GetThingiesInForceField(float _radius)
    {
        List<ThingyPhysics> returnValue = new List<ThingyPhysics>();
        List<Collider> collidersWithinForceField = Physics.OverlapSphere(transform.position, _radius).ToList();
        //order the colliders by distance to the center of the forcefield. this is because, if the number of thingies exceeds the maximum, we only return the thingies closest to the center.
        List<Collider> orderedColliders = collidersWithinForceField.OrderBy(_c => (transform.position - _c.transform.position).sqrMagnitude).ToList();
        foreach(Collider collider in orderedColliders)
        {
            if(returnValue.Count < maxThingies && ThingyPhysics.gameObjectToThingyMap.ContainsKey(collider.gameObject) && !returnValue.Contains(ThingyPhysics.gameObjectToThingyMap[collider.gameObject]))
            {
                if(!ignoredThingies.Contains(ThingyPhysics.gameObjectToThingyMap[collider.gameObject]))
                {
                    returnValue.Add(ThingyPhysics.gameObjectToThingyMap[collider.gameObject]);
                }
            }
        }
        return returnValue;
    }

    private void Start()
    {
        //initialise the positions
        thisFamePosition = lastFramePosition = transform.position;
    }

    /// <summary>
    /// put the given thingy in the ignoredThingies list for the given amount of seconds, then remove it back from the list.
    /// </summary>s
    private IEnumerator IgnoreThingyForSeconds(ThingyPhysics _thingyToIgnore, float _secondsToIgnore)
    {
        ignoredThingies.Add(_thingyToIgnore);
        yield return new WaitForSeconds(_secondsToIgnore);
        ignoredThingies.Remove(_thingyToIgnore);
    }
    
    /// <summary>
    /// shoot the thingy closest to the target toward the target by adding to its velocity.
    /// </summary>
    public void ShootThingy()
    {
        //the list of thingies within innerFieldRadius.
        List<ThingyPhysics> thingysInForceField = GetThingiesInForceField(innerFieldRadius);
        //Go over the thingies within innerfield radius and get the one closest to the target as determined by the direction the aiming camera is facing.
        //Then add to that thingies velocity and add it to the ignoredThingies list.
        if(thingysInForceField.Count > 0)
        {
            Vector3 shootTarget = Vector3.one;
            RaycastHit hit = new RaycastHit();
            if(usedCamera && Physics.Raycast(usedCamera.transform.position, usedCamera.transform.forward, 1000, LayerMask.GetMask("IgnoredByHover", "Hover")))
            {
                shootTarget = hit.point;
            }
            else if(usedCamera)
            {
                shootTarget = transform.position + usedCamera.transform.forward * (outerFieldRadius * 5f);
            }
            List<ThingyPhysics> orderedThingiesInField = thingysInForceField.OrderBy(c => (shootTarget - c.transform.position).sqrMagnitude).ToList(); 
            ThingyPhysics thingyPhysicsToShoot = orderedThingiesInField[0];
            StartCoroutine(IgnoreThingyForSeconds(thingyPhysicsToShoot, 1f));
            Vector3 shootDirection = (shootTarget - thingyPhysicsToShoot.transform.position).normalized;
            foreach(Rigidbody connectedRigidbody in thingyPhysicsToShoot.ConnectedRigidbodies)
            {
                connectedRigidbody.velocity += shootDirection * shootVelocity;
            }
        }
    }
    
    private float coolDown;
    private void Update()
    {
        // Update the cooldown time.
        if(coolDown <= coolDownTime)
        {
            coolDown += Time.deltaTime;
        }
        // Shoot a thingy and reset the cooldown if the left mouse button is held down and the cooldown is greater than or equal to the cooldown time.
        if(Input.GetMouseButton(0) && coolDown >= coolDownTime)
        {
            ShootThingy();
            coolDown = 0;
        }
    }

    private void FixedUpdate()
    {
        UpdateAccelleration();
        List<ThingyPhysics> thingysInForceField = GetThingiesInForceField(outerFieldRadius);
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
        //draw the outer and inner radius in green in red respectively.
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, outerFieldRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, innerFieldRadius);
    }
}
