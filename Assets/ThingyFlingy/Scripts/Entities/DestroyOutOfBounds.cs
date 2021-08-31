using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private void FixedUpdate()
    {
        //if there are bounds and this gameobject is outside them, destroy this gameobject.
        if(Bounds.theBounds && !Bounds.theBounds.IsInsideBounds(transform.position))
        {
            Destroy(gameObject);
        }
    }
}
