using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(Rigidbody), typeof(ThingyForceField))]
public class HoverController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float rotationTorque = 5;
    // we use the camera pivot's current rotation to rotate the orb
    [SerializeField] private float movementForce = 1;
    [SerializeField] private float maxVerticalCameraAngle = 85;
    [SerializeField] private GameObject cameraGameObject;
    [SerializeField] private Vector3 cameraOffset = Vector3.back * 10;
    [SerializeField] private Vector3 cameraLookPosition = Vector3.up;
    
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

    private Vector3 forceDirection;
    private Vector3 TargetForce => forceDirection.normalized * movementForce;
    private float currentCameraXRotation = 0;
    private float currentCameraYRotation = 0;

    private void UpdateForceDirection()
    {
        Quaternion forceRot = Quaternion.Euler(0, cameraGameObject.transform.rotation.eulerAngles.y,0);
        forceDirection = forceRot * (new Vector3(Input.GetAxisRaw("Horizontal"), 0 , Input.GetAxisRaw("Vertical")).normalized * movementForce);
    }
    
    private void Update()
    {
        if(!PauseMenuHandler.IsPaused)
        {
            UpdateForceDirection();
            if(cameraGameObject)
            {
                currentCameraXRotation += Input.GetAxisRaw("Mouse Y");
                currentCameraYRotation += Input.GetAxisRaw("Mouse X");
                if(currentCameraXRotation > maxVerticalCameraAngle)
                {
                    currentCameraXRotation = maxVerticalCameraAngle;
                }
                else if(currentCameraXRotation < -maxVerticalCameraAngle)
                {
                    currentCameraXRotation = -maxVerticalCameraAngle;
                }
                cameraGameObject.transform.position = gameObject.transform.position + gameObject.transform.rotation * Quaternion.Euler(-currentCameraXRotation, currentCameraYRotation, 0) * cameraOffset;
                cameraGameObject.transform.LookAt(transform.position + cameraLookPosition);
            }
        }
    }

    private void FixedUpdate()
    {
        rb.AddTorque(StabilizingTorque);
        rb.AddRelativeForce(TargetForce);
    }
}
