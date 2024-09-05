using UnityEngine;
using UnityEngine.UI; // Use this for the Image component

public class CapturePoint : MonoBehaviour
{
    public float captureTime = 10f; // Time required to capture the point
    public Image captureProgressBar; // Image component for the progress bar
    public float baseScorePerSecond = 10; // Base points awarded per second after capture

    private float captureProgress = 0f;
    private bool isBeingCaptured = false;
    private bool isCaptured = false; // To track if the point has been captured
    private GameObject capturingPlayer = null;
    private PlayerScore playerScore; // Reference to the player's score component

    void Start()
    {
        // Hide the progress bar at the start
        captureProgressBar.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isBeingCaptured && capturingPlayer != null && !isCaptured)
        {
            captureProgress += Time.deltaTime;
            captureProgressBar.fillAmount = captureProgress / captureTime;

            if (captureProgress >= captureTime)
            {
                CapturePointFully();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            captureProgressBar.gameObject.SetActive(true); // Show the progress bar

            if (!isCaptured)
            {
                isBeingCaptured = true;
                capturingPlayer = other.gameObject;
                playerScore = capturingPlayer.GetComponent<PlayerScore>(); // Get the PlayerScore component
            }
            else
            {
                captureProgressBar.fillAmount = 1f; // Show the bar as fully filled if already captured
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            captureProgressBar.gameObject.SetActive(false); // Hide the progress bar

            if (!isCaptured)
            {
                isBeingCaptured = false;
                capturingPlayer = null;
                playerScore = null; // Clear the reference to the PlayerScore component
                captureProgress = 0f;
                captureProgressBar.fillAmount = 0f;
            }
        }
    }

    void CapturePointFully()
    {
        Debug.Log("Point Captured!");
        isCaptured = true;
        captureProgressBar.fillAmount = 1f; // Fill the bar completely
        playerScore.CapturePoint(this); // Notify the player score system
    }

    public float GetScorePerSecond()
    {
        // Return the score per second for this point
        return baseScorePerSecond;
    }
}
