using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Function for making projectiles spin
public class Spinner : MonoBehaviour
{
    // The speed of rotation, in degrees per second
    [SerializeField] float rotationSpeed = 1f;
        
    
    void Update()
    {
        transform.Rotate(transform.rotation.x, transform.rotation.y, rotationSpeed * Time.deltaTime);
    }
}
