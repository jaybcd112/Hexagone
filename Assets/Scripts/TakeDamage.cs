using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public float basePower = 5f;
    public float radius = 15.0f;
    public float lockoutDuration = 1f;

    private bool isLockedOut = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isLockedOut && other.gameObject.layer == LayerMask.NameToLayer("Hitbox"))
        {
            
            StartCoroutine(ApplyDamage(other));
        }
    }

    IEnumerator ApplyDamage(Collider other)
    {

        isLockedOut = true;

        Vector3 direction = other.transform.root.forward;
        direction.y = 0f;
        direction.Normalize();

        Debug.DrawRay(transform.position, direction * 5f, Color.green, 1f);

        float power = basePower + GetComponentInParent<PlayerController>().GetPercentage() * 10;

        Rigidbody rb = GetComponentInParent<Rigidbody>();

        // Apply force in the opposite direction of the hitting player's facing direction
        rb.AddForce(direction * power, ForceMode.Impulse);

        GetComponentInParent<PlayerController>().UpdatePercentage(10f);

        yield return new WaitForSeconds(lockoutDuration);

        isLockedOut = false;
    }

}
