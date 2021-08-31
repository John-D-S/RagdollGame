using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HoverController : MonoBehaviour
{
    [SerializeField, Tooltip("How much force is applied to move the character.")] private float movementForce = 1;
    [SerializeField, Tooltip("The maximum angle the camera can look up or down.")] private float maxVerticalCameraAngle = 85;
    [SerializeField, Tooltip("The camera that rotates around the player")] private GameObject cameraGameObject;
    [SerializeField, Tooltip("What should be the camera's position in relation to the player.")] private Vector3 cameraOffset = Vector3.back * 10;
    [SerializeField, Tooltip("what position in relation to the player should the camera be looking at.")] private Vector3 cameraLookPosition = Vector3.up;
    private Rigidbody rb;

    //The direction the force should be applied in.
    private Vector3 forceDirection;
    //The force that should be applied to move the player
    private Vector3 TargetForce => forceDirection.normalized * movementForce;
    //The current x and y rotation of the camera
    private float currentCameraXRotation = 0;
    private float currentCameraYRotation = 0;

    private void Start()
    {
        //set the rigidbody
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Update the force direction based on the current imputs and the current angle of the camera.
    /// </summary>
    private void UpdateForceDirection()
    {
        Quaternion forceRot = Quaternion.Euler(0, cameraGameObject.transform.rotation.eulerAngles.y,0);
        forceDirection = forceRot * (new Vector3(Input.GetAxisRaw("Horizontal"), 0 , Input.GetAxisRaw("Vertical")).normalized * movementForce);
    }

    private void Update()
    {
        //if the game isn't paused, rotate the camera according to the mouse input.
        if(!PauseMenuHandler.IsPaused)
        {
            UpdateForceDirection();
            if(cameraGameObject)
            {
                currentCameraXRotation += Input.GetAxisRaw("Mouse Y");
                currentCameraYRotation += Input.GetAxisRaw("Mouse X");
                //clamp the camera rotation to be less than the max and greater than the min
                currentCameraXRotation = Mathf.Clamp(currentCameraXRotation, -maxVerticalCameraAngle, maxVerticalCameraAngle);
                //set the position and rotation of the camera according to the current camera rotation variables.
                cameraGameObject.transform.position = gameObject.transform.position + gameObject.transform.rotation * Quaternion.Euler(-currentCameraXRotation, currentCameraYRotation, 0) * cameraOffset;
                cameraGameObject.transform.LookAt(transform.position + cameraLookPosition);
            }
        }
    }

    private void FixedUpdate()
    {
        rb.AddRelativeForce(TargetForce);
    }
}
