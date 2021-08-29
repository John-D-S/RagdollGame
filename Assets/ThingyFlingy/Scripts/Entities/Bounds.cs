using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds : MonoBehaviour
{
    [SerializeField] private Vector3 minBoundCorner = new Vector3(-100, -100, -100);
    [SerializeField] private Vector3 maxBoundCorner = new Vector3(100, 100, 100);
    public static Bounds theBounds;

    private void Start()
    {
        theBounds = this;
    }
    
    public bool IsInsideBounds(Vector3 _position)
    {
        bool isLessThanMax = _position.x < maxBoundCorner.x && _position.y < maxBoundCorner.y && _position.z < maxBoundCorner.z;
        bool isGreaterThanMin = _position.x > minBoundCorner.x && _position.y > minBoundCorner.y && _position.z > minBoundCorner.z;
        return isLessThanMax && isGreaterThanMin;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(minBoundCorner + (maxBoundCorner - minBoundCorner) * 0.5f, maxBoundCorner - minBoundCorner);
    }
}
