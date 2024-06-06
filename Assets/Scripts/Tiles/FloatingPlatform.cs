using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    public float bounceForce = 1000f;
    public Vector3 bounceDirection = new Vector3(1, -1, 0); // adjust for each platform

    void Start()
    {
        // Normalize the bounce direction to ensure consistent force application
    }

    void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 bounce = bounceDirection * bounceForce;
            rb.AddForce(bounce, ForceMode.Impulse);
        }
    }
}
