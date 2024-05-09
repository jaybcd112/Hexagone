using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float radius = 15.0f;
    public float basePower = 5000.0f;

    public PlayerController PlayerController;

    void Start()
    {
        Vector3 explosionPos = transform.position;
    }
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.tag == "Player" && collision.gameObject != gameObject);
        if(collision.gameObject.tag == "Player" && collision.gameObject != gameObject)
        {
            float power = basePower + collision.gameObject.GetComponent<PlayerController>().percentage * 100;
            collision.rigidbody.AddExplosionForce(power, transform.position, radius, 0f);
            Debug.Log(this.GetComponent<Rigidbody>().velocity.magnitude);
            collision.gameObject.GetComponent<PlayerController>().updatePercentage(10);
        }
    }
}
