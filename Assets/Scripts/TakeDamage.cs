using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public float basePower = 5f;
    public float radius = 15.0f;
    public float lockoutDuration = 1f;
    public float baseStunDuration;

    private bool isLockedOut = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isLockedOut && other.gameObject.layer == LayerMask.NameToLayer("Hitbox"))
        {
            
            ApplyDamage(other);
        }
    }

    private void ApplyDamage(Collider other)
    {

        isLockedOut = true;

        //direction of the hitting player
        Vector3 direction = other.transform.root.forward;
        direction.y = 0f;
        direction.Normalize();

        PlayerController playerControler = GetComponentInParent<PlayerController>(gameObject);
        Rigidbody rb = GetComponentInParent<Rigidbody>();



        float power = basePower + playerControler.percentage * 10;
        rb.AddForce(direction * power, ForceMode.Impulse);

        playerControler.UpdatePercentage(10);

        Vector3 targetDirection = other.transform.position - transform.parent.position;
        targetDirection.y = 0f;
        targetDirection.Normalize();

        StartCoroutine(Stunned());
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        float rotationSpeed = 20f;
        while (Quaternion.Angle(transform.parent.rotation, targetRotation) > 0.1f)
        {
            transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    IEnumerator Stunned()
    {
        PlayerController playerControler = GetComponentInParent<PlayerController>(gameObject);
        float stunDuration = baseStunDuration + (playerControler.percentage * .02f);

        ParticleSystem.MainModule mainModule = playerControler.stunParticle.main;
        mainModule.startLifetime = baseStunDuration + (stunDuration);
        playerControler.stunParticle.Play();

        playerControler.animator.SetBool("Stunned", true);
        playerControler.stunned = true;

        yield return new WaitForSeconds(stunDuration);

        isLockedOut = false;
        playerControler.stunned = false;
        playerControler.animator.SetBool("Stunned", false);
    }



}
