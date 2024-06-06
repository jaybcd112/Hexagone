using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyTile : MonoBehaviour
{
    public float bounceForce = 300f; // Adjust the bounce force as needed
    public AudioSource boing;

    void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
        boing.pitch = Random.Range(0.80f, 1.20f);
        boing.Play();

        if (rb != null)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                Vector3 bounce = Vector3.up * bounceForce;
                rb.AddForce(bounce, ForceMode.Impulse);
                break;
            }
        }
    }
}
