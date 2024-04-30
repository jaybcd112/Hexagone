using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float radius = 5.0f;
    public float power = 10.0f;

    void Start()
    {
        Vector3 explosionPos = transform.position;
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("contact");
        Debug.Log(collision.rigidbody);
        collision.rigidbody.AddExplosionForce(power, transform.position, radius, 3.0f);
    }
}
