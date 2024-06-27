using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public float launchForce = 1000f;
    public Vector3 launchDirection = new Vector3(1, 1, 0); // Default launch direction

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 normalizedLaunchDirection = launchDirection.normalized; // Normalize to ensure consistent force
                playerRb.AddForce(normalizedLaunchDirection * launchForce, ForceMode.Impulse);
            }
        }
    }
}
