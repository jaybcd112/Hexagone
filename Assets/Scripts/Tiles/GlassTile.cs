using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassTile : MonoBehaviour
{
    public int glassHealth = 6; //for some reason glass health is subtracted twice each jump, not sure why

    public void JumpImpact()
    {
        glassHealth--;
        if (glassHealth == 0)
            Destroy(gameObject);
    }
}
