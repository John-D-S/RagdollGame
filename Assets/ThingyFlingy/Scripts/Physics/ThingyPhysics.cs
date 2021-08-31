using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThingyPhysics : MonoBehaviour
{
    // a dictionary that maps gameobjects to a thingyPhysics. it is used to find out which thingyPhysics a particular limb/rigidbody belongs to;
    public static Dictionary<GameObject, ThingyPhysics> gameObjectToThingyMap = new Dictionary<GameObject, ThingyPhysics>();
    
    [SerializeField, Tooltip("A list of collision detectors attatched to this ThingyPhysics.")] private List<CollisionDetector> collisionDetectors = new List<CollisionDetector>();

    // a list of rigidbodies connected to this thingyPhysics
    private List<Rigidbody> connectedRigidbodies = new List<Rigidbody>();
    /// <summary>
    /// All rigidbodies connected to this thingyPhysics
    /// </summary>
    public List<Rigidbody> ConnectedRigidbodies
    {
        get
        {
            if(connectedRigidbodies.Count == 0)
            {
                List<Rigidbody> returnValue = new List<Rigidbody>();
                foreach(CollisionDetector collisionDetector in collisionDetectors)
                {
                    Rigidbody collisionDetectorRigidbody = collisionDetector.gameObject.GetComponent<Rigidbody>();
                    if(collisionDetectorRigidbody)
                    {
                        returnValue.Add(collisionDetectorRigidbody);
                    }
                }
                connectedRigidbodies = returnValue;
            }
            return connectedRigidbodies;
        }
    }
    
    /// <summary>
    /// returns whether any one of the thingies is colliding
    /// </summary>
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
    
    [SerializeField, Tooltip("The maximum upwardForce applied to the thingy while it is on the ground")] private float maxStandingForce = 3f;
    [SerializeField, Tooltip("How quickly the standing force stops being applied once the thingy is no longer touching the ground.")] private float standingForceDepletionTime = 1f;
    private float currentStandingForce;
    [SerializeField, Tooltip("I assume this means torque")] private float UprightTorque = 1f;
    private float currentUprightTorque;

    private Rigidbody rb;
    
    /// <summary>
    /// adds all the Collision detectors to the gameObjectToThingy dict 
    /// </summary>
    private void AddAllCollisionDetectorsToDict()
    {
        foreach(CollisionDetector collisionDetector in collisionDetectors)
        {
            if(collisionDetector && collisionDetector.gameObject)
            {
                AddGameObjectToDict(collisionDetector.gameObject);
            }
        }
    }
    /// <summary>
    /// adds the given gameobject to the gameObjectToThingyMap dictionary
    /// </summary>
    private void AddGameObjectToDict(GameObject _gameObject) => gameObjectToThingyMap[_gameObject] = this;
    
    /// <summary>
    /// removes all connected colliders from the gameObjectToThinyMap
    /// </summary>
    private void RemoveAllCollisionDetectorsFromDict()
    {
        foreach(CollisionDetector collisionDetector in collisionDetectors)
        {
            gameObjectToThingyMap.Remove(collisionDetector.gameObject);
        }
    }

    /// <summary>
    /// hits the enemy (unused)
    /// </summary>
    private void HitEnemy(float _velocity, ref Enemy _enemy)
    {
        _enemy.Hit(1);
    }
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        AddAllCollisionDetectorsToDict();
        foreach(CollisionDetector collisionDetector in collisionDetectors)
        {
            collisionDetector.connectedThingyPhysics = this;
        }
    }

    private void FixedUpdate()
    {
        //if the thingy is on the ground, increase the torque and force up to maximum and if it isn't, decrease them down to zero
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
        //calculate the torque
        Quaternion rot = Quaternion.FromToRotation(transform.up, Vector3.up);
        // add the torque
        rb.AddRelativeTorque(new Vector3(rot.x, rot.y, rot.z) * currentUprightTorque);
        //add the force.
        if(currentStandingForce > 0)
            rb.AddForce(Vector3.up * currentStandingForce);
    }

    private void OnDestroy()
    {
        RemoveAllCollisionDetectorsFromDict();
    }
}