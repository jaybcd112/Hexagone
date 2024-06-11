using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinDisplay : MonoBehaviour
{
    // Public Vector3 to set rotation speed in the Inspector
    public Vector3 rotationSpeed;

    void Update()
    {
        // Rotate the object based on rotationSpeed and Time.deltaTime
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
