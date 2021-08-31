using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyLink : MonoBehaviour
{
    [SerializeField, Tooltip("This gameobject will be destroyed when the LinkedGameObject is destroyed.")] private GameObject linkedGameObject;

    private void FixedUpdate()
    {
        //if the linked gameobject is destroyed, destroy this one.
        if(!linkedGameObject)
        {
            Destroy(gameObject);
        }
    }
}
