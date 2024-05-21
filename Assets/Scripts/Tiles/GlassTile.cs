using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassTile : MonoBehaviour
{
    public int glassStartingHealth = 6; //for some reason glass health is subtracted twice each jump, not sure why
    private int glassHealth;
    private float smoothnessIncrement;
    private float smoothnessValue;
    public Material shader;


    public void Start()
    {
        glassHealth = glassStartingHealth;

        smoothnessIncrement = 0.5f / (float)glassStartingHealth;
    }
    public void JumpImpact()
    {
        glassHealth--;
        smoothnessValue = smoothnessValue + smoothnessIncrement;
        shader.SetFloat("_Smoothness", smoothnessValue);

        if (glassHealth == 0)
            Destroy(gameObject);
            shader.SetFloat("_Smoothness", 0f);
    }
}
