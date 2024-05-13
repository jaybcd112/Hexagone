using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassTile : MonoBehaviour
{
    public int glassHealth = 5;

    public void JumpImpact()
    {
        glassHealth--;
        if (glassHealth == 0)
            Destroy(gameObject);
    }
}
