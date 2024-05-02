using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float radius = 15.0f;
    public float power = 5000.0f;

    void Start()
    {
        Vector3 explosionPos = transform.position;
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag == "Player" && collision.gameObject != gameObject.);
        if(collision.gameObject.tag == "Player" && collision.gameObject != gameObject)
            collision.rigidbody.AddExplosionForce(power, transform.position, radius, 0f);
    }
}
