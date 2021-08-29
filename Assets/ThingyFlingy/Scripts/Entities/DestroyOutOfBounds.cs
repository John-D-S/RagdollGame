using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private void FixedUpdate()
    {
        if(Bounds.theBounds && !Bounds.theBounds.IsInsideBounds(transform.position))
        {
            Destroy(gameObject);
        }
    }
}
