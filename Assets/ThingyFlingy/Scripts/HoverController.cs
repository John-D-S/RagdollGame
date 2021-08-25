using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(Rigidbody))]
public class HoverController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float rotationTorque = 5;
    [SerializeField] private float rotationTorqueDrag = 1;
    [SerializeField] private float movementForce = 1;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private Vector3 StabilizingTorque
    {
        get
        {
            Quaternion rot = Quaternion.FromToRotation(transform.up, Vector3.up);
            return new Vector3(rot.x, rot.y, rot.z) * rotationTorque;
        }
    }

    private Vector3 AimingTorque => new Vector3(mouseDelta.y, mouseDelta.x, 0);

    private Vector2 mouseDelta;
    private Vector3 TargetForce;
    private void Update()
    {
        mouseDelta.x = Input.GetAxisRaw("Mouse X");
        mouseDelta.y = Input.GetAxisRaw("Mouse Y");
        TargetForce = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * movementForce;
    }

    private void FixedUpdate()
    {
        rb.AddTorque(StabilizingTorque);
        rb.AddRelativeTorque(AimingTorque);
        rb.AddRelativeForce(TargetForce);
    }
}
