using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Hover : MonoBehaviour
{
    [SerializeField, Tooltip("The height above the object below it that this object will try to hover.")] private float targetHeight = 3;
    public float TargetHeight => targetHeight;
    [SerializeField, Tooltip("How much vertical drag will this gameObject have. This helps to prevent excessive bouncy oscillation.")] private float verticalDragModifier = 1;
    private Rigidbody rb;
    
    /// <summary>
    /// The amount of upward force to apply to this gameobject
    /// </summary>
    private float UpwardThrust
    {
        get
        {
            if(rb)
            {
                RaycastHit hit = new RaycastHit();
                // if the gameobject is above a collider, apply a force to hover.
                if(Physics.Raycast(transform.position, Vector3.down, out hit,targetHeight * 2f, ~LayerMask.GetMask("IgnoredByHover", "Hover")))
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
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.up* UpwardThrust);
    }
}
