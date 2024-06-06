using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public float basePower = 5f;
    public float radius = 15.0f;
    public float lockoutDuration = 1f;
    public float baseStunDuration;

    public AudioSource stunAudioSource;

    public AudioSource hurtAudioSource;

    public AudioClip hurt1;
    public AudioClip hurt2;
    public AudioClip hurt3;
    public AudioClip hurt4;


    private bool isLockedOut = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isLockedOut && other.gameObject.layer == LayerMask.NameToLayer("Hitbox"))
        {

            ApplyDamage(other);
            PlayHurtSound();
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

        float power = basePower + playerControler.GetPercentage() * 10;

        rb.AddForce(direction * power, ForceMode.Impulse);

        playerControler.UpdatePercentage(10f);

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

    public void PlayHurtSound()
    {
        int randomIndex = Random.Range(0, 4);
        AudioClip selectedClip = null;

        switch (randomIndex)
        {
            case 0:
                selectedClip = hurt1;
                break;
            case 1:
                selectedClip = hurt2;
                break;
            case 2:
                selectedClip = hurt3;
                break;
            case 3:
                selectedClip = hurt4;
                break;
        }

        hurtAudioSource.clip = selectedClip;
        hurtAudioSource.Play();
    }

    public IEnumerator Stunned()
    {
        PlayerController playerControler = GetComponentInParent<PlayerController>();
        float stunDuration = baseStunDuration + (playerControler.GetPercentage() * .02f);

        ParticleSystem.MainModule mainModule = playerControler.stunParticle.main;
        mainModule.startLifetime = baseStunDuration + stunDuration;
        playerControler.stunParticle.Play();

        playerControler.animator.SetBool("Stunned", true);
        playerControler.SetStunned(true);

        if (stunAudioSource != null)
        {
            stunAudioSource.Play();
        }

        yield return new WaitForSeconds(stunDuration);

        if (stunAudioSource != null)
        {
            stunAudioSource.Stop();
        }

        isLockedOut = false;
        playerControler.SetStunned(false);
        playerControler.animator.SetBool("Stunned", false);
    }



}
