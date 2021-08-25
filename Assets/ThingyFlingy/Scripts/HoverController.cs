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
    // we use the camera pivot's current rotation to rotate the orb
    [SerializeField] private float movementForce = 1;
    [SerializeField] private float maxVerticalCameraAngle = 85;
    [SerializeField] private GameObject cameraGameObject;
    [SerializeField] private Vector3 cameraOffset = Vector3.back * 10;
    [SerializeField] private Vector3 cameraLookPosition = Vector3.up;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private Vector3 StabilizingTorque
    {
        get
        {
            Quaternion rot = Quaternion.FromToRotation(transform.up, Vector3.up);
            return new Vector3(rot.x, rot.y, rot.z) * rotationTorque;
        }
    }

    private Vector3 AimingTorque => new Vector3(0, mouseDelta.x * rotationTorque, 0);
    
    private Vector2 mouseDelta;
    private Vector3 unnormalizedDirection;
    private Vector3 TargetForce => unnormalizedDirection.normalized * movementForce;
    private float currentCameraXrotation = 0;
    private void Update()
    {
        mouseDelta.x = Input.GetAxisRaw("Mouse X");
        mouseDelta.y = Input.GetAxisRaw("Mouse Y");
        unnormalizedDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if(cameraGameObject)
        {
            currentCameraXrotation += Input.GetAxisRaw("Mouse Y");
            if(currentCameraXrotation > maxVerticalCameraAngle)
            {
                currentCameraXrotation = maxVerticalCameraAngle;
            }
            else if(currentCameraXrotation < -maxVerticalCameraAngle)
            {
                currentCameraXrotation = -maxVerticalCameraAngle;
            }
            cameraGameObject.transform.position = gameObject.transform.position + gameObject.transform.rotation * Quaternion.Euler(-currentCameraXrotation,0, 0) * cameraOffset;
            cameraGameObject.transform.LookAt(transform.position + cameraLookPosition);
        }
    }

    private void FixedUpdate()
    {
        rb.AddTorque(StabilizingTorque);
        rb.AddRelativeTorque(AimingTorque);
        rb.AddRelativeForce(TargetForce);
    }
}
