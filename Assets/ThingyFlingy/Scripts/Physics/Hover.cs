using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEditor.Experimental.GraphView;

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Hover : MonoBehaviour
{
    [SerializeField] private float targetHeight = 3;
    /*[SerializeField] private List<int> ignoredLayers = new List<int>();
    private int[] AllIgnoredLayers
    {
        get
        {
            List<int> allIgnoredLayers = ignoredLayers.ToList();
            allIgnoredLayers.Add(gameObject.layer);
            return allIgnoredLayers.ToArray();
        }
    }*/
    public float TargetHeight => targetHeight;
    [SerializeField] private float verticalDragModifier = 1;
    private Rigidbody rb;
    
    private float UpwardThrust
    {
        get
        {
            if(rb)
            {
                RaycastHit hit = new RaycastHit();
                if(Physics.Raycast(transform.position, Vector3.down, out hit,targetHeight * 2f, ~LayerMask.GetMask("IgnoredByHover")))
                {
                    float height = hit.distance;
                    return Mathf.Abs(Physics.gravity.y) * targetHeight / height - rb.velocity.y * verticalDragModifier;
                }
            }
            return 0;
        }
    }

    private void Start()
    {
        //gameObject.layer = LayerMask.GetMask("Hover");
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.up* UpwardThrust);
    }
}
