using UnityEngine;
using UnityEngine.UI; // Use this for the Image component

public class CapturePoint : MonoBehaviour
{
    // Variables to control capture time and scoring
    public float captureTime = 10f; // Time required to capture the point
    public Image captureProgressBar; // Image component for the progress bar UI
    public float baseScorePerSecond = 10; // Base points awarded per second after capture

    private float captureProgress = 0f; // Tracks the capture progress (0 to 100%)
    private bool isBeingCaptured = false; // Is the point currently being captured?
    private bool isCaptured = false; // Has the point been fully captured?
    private GameObject capturingPlayer = null; // Reference to the player currently capturing
    private PlayerScore playerScore; // Reference to the player's score component

    void Start()
    {
        // Hide the progress bar at the start of the game
        captureProgressBar.gameObject.SetActive(false);
    }

    void Update()
    {
        // If a player is capturing and the point isn't fully captured yet
        if (isBeingCaptured && capturingPlayer != null && !isCaptured)
        {
            // Increase the capture progress over time
            captureProgress += Time.deltaTime;
            // Update the progress bar fill amount based on capture progress
            captureProgressBar.fillAmount = captureProgress / captureTime;

            // If the capture progress reaches the required time
            if (captureProgress >= captureTime)
            {
                // Call the function to complete the capture
                CapturePointFully();
            }
        }
    }

    // Triggered when a player enters the capture zone
    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the zone is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Show the progress bar when a player starts capturing
            captureProgressBar.gameObject.SetActive(true);

            // If the point is not yet captured
            if (!isCaptured)
            {
                // Start the capture process
                isBeingCaptured = true;
                capturingPlayer = other.gameObject; // Set the capturing player
                playerScore = capturingPlayer.GetComponent<PlayerScore>(); // Get the PlayerScore component of the player
            }
            else
            {
                // If already captured, show the bar as fully filled
                captureProgressBar.fillAmount = 1f;
            }
        }
    }

    // Triggered when a player exits the capture zone
    void OnTriggerExit(Collider other)
    {
        // Check if the object leaving the zone is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Hide the progress bar when the player leaves the zone
            captureProgressBar.gameObject.SetActive(false);

            // If the point is not captured yet
            if (!isCaptured)
            {
                // Stop the capture process and reset variables
                isBeingCaptured = false;
                capturingPlayer = null;
                playerScore = null; // Clear reference to the player's score component
                captureProgress = 0f; // Reset capture progress
                captureProgressBar.fillAmount = 0f; // Reset the progress bar UI
            }
        }
    }

    // Function to call when the point is fully captured
    void CapturePointFully()
    {
        Debug.Log("Point Captured!"); // Log a message for debugging
        isCaptured = true; // Mark the point as captured
        captureProgressBar.fillAmount = 1f; // Ensure the progress bar is fully filled
        playerScore.CapturePoint(this); // Notify the player's score system that the point was captured
    }

    // Function to get the score per second awarded after capturing the point
    public float GetScorePerSecond()
    {
        // Return the base score per second for this point
        return baseScorePerSecond;
    }
}
