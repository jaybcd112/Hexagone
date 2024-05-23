using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundAndDestroy : MonoBehaviour
{
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Awake()
    {
        Destroy(gameObject, audioSource.clip.length);
    }
}
