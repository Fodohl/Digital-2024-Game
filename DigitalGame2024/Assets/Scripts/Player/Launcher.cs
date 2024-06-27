using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public float launchForce = 1000f;
    public Vector3 launchDirection = new Vector3(1, 1, 0); // Default launch direction
    public bool disableExternalForces = true; // Option to disable external forces temporarily

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // Apply impulse force
                Vector3 normalizedLaunchDirection = launchDirection.normalized; // Normalize to ensure consistent force
                playerRb.velocity = Vector3.zero; // Reset any existing velocity
                playerRb.angularVelocity = Vector3.zero; // Reset angular velocity
                playerRb.AddForce(normalizedLaunchDirection * launchForce, ForceMode.Impulse);

                // Optionally disable external forces temporarily
                if (disableExternalForces)
                {
                    playerRb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null && disableExternalForces)
            {
                // Re-enable external forces if they were disabled
                playerRb.constraints = RigidbodyConstraints.None;
            }
        }
    }
}

