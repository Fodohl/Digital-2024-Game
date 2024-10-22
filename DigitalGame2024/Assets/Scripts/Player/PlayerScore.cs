using UnityEngine;
using TMPro; // Namespace for using TextMeshPro for UI text
using System.Collections.Generic;

public class PlayerScore : MonoBehaviour
{
    // Variables for score management and UI
    public float currentScore = 0; // Player's current score
    public TextMeshProUGUI scoreText; // Reference to the TextMeshPro UI component for displaying the score
    public List<CapturePoint> capturedPoints = new List<CapturePoint>(); // List to track all points the player has captured
    public float scoreMultiplier = 1.0f; // Multiplier to control the rate at which score is added (e.g., for bonuses)

    void Start()
    {
        // Initialize the score display at the start of the game
        UpdateScoreDisplay();
    }

    void Update()
    {
        // If the player has captured points, increase the score over time
        if (capturedPoints.Count > 0)
        {
            // Loop through each captured point
            foreach (var point in capturedPoints)
            {
                // Calculate points to add based on the capture point's score per second, deltaTime, and score multiplier
                float pointsToAdd = point.GetScorePerSecond() * Time.deltaTime * scoreMultiplier;
                // Add the calculated points to the player's current score
                currentScore += pointsToAdd;
            }
            // Update the score display after adding the points
            UpdateScoreDisplay();
        }
    }

    // Function to call when the player captures a new point
    public void CapturePoint(CapturePoint point)
    {
        // Add the point to the capturedPoints list if it's not already there
        if (!capturedPoints.Contains(point))
        {
            capturedPoints.Add(point);
        }
    }

    // Function to update the score display UI
    void UpdateScoreDisplay()
    {
        // Display the current score rounded to an integer value in the UI
        scoreText.text = "Score: " + Mathf.RoundToInt(currentScore).ToString();
    }
}
