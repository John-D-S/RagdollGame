using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds : MonoBehaviour
{
    [SerializeField, Tooltip("The greatest corner of the bounding box.")] private Vector3 minBoundCorner = new Vector3(-100, -100, -100);
    [SerializeField, Tooltip("The other corner of the bounding box.")] private Vector3 maxBoundCorner = new Vector3(100, 100, 100);
    //The currently active bounds.
    public static Bounds theBounds;
    
    private void Start()
    {
        //set the theBounds singleton to this
        theBounds = this;
    }
    
    /// <summary>
    /// returns whether or not the position is within the bounds.
    /// </summary>
    public bool IsInsideBounds(Vector3 _position)
    {
        bool isLessThanMax = _position.x < maxBoundCorner.x && _position.y < maxBoundCorner.y && _position.z < maxBoundCorner.z;
        bool isGreaterThanMin = _position.x > minBoundCorner.x && _position.y > minBoundCorner.y && _position.z > minBoundCorner.z;
        return isLessThanMax && isGreaterThanMin;
    }
    
    private void OnDrawGizmos()
    {
        //draw a wireframe of the bounds.
        Gizmos.DrawWireCube(minBoundCorner + (maxBoundCorner - minBoundCorner) * 0.5f, maxBoundCorner - minBoundCorner);
    }
}
