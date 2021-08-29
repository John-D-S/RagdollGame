using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Rigidbody))]
public class ThingyPhysics : MonoBehaviour
{
    public static Dictionary<GameObject, ThingyPhysics> gameObjectToThingyMap = new Dictionary<GameObject, ThingyPhysics>();
    
    [SerializeField] private List<CollisionDetector> collisionDetectors = new List<CollisionDetector>();

    private List<Rigidbody> connectedRigidbodies = new List<Rigidbody>();
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

    private Rigidbody rb;
    
    
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
    private void AddGameObjectToDict(GameObject _gameObject) => gameObjectToThingyMap[_gameObject] = this;
    private void RemoveAllCollisionDetectorsFromDict()
    {
        foreach(CollisionDetector collisionDetector in collisionDetectors)
        {
            gameObjectToThingyMap.Remove(collisionDetector.gameObject);
        }
    }

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

    private void OnDestroy()
    {
        RemoveAllCollisionDetectorsFromDict();
    }
}