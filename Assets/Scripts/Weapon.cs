using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float radius = 15.0f;
    public float basePower = 5f;

    public Collider hammerCollider;
    void Start()
    {
        hammerCollider.enabled = false;
        Vector3 explosionPos = transform.position;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject != transform.root.gameObject)
        {
            float power = basePower  * 1000 + collision.gameObject.GetComponent<NewPlayerController>().percentage * 500;
            collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(power, transform.position, radius, 0f);
            collision.gameObject.GetComponent<NewPlayerController>().UpdatePercentage(10);
        }
    }
}
