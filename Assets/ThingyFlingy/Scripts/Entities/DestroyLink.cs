using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyLink : MonoBehaviour
{
    [SerializeField] private GameObject linkedGameObject;

    private void FixedUpdate()
    {
        if(!linkedGameObject)
        {
            Destroy(gameObject);
        }
    }
}
