using UnityEngine;
using TMPro; // Make sure to include the TextMeshPro namespace
using System.Collections.Generic;

public class PlayerScore : MonoBehaviour
{
    public float currentScore = 0; // Player's current score
    public TextMeshProUGUI scoreText; // Reference to the TextMeshPro UI component
    public List<CapturePoint> capturedPoints = new List<CapturePoint>(); // List of captured points
    public float scoreMultiplier = 1.0f; // Multiplier to control score speed

    void Start()
    {
        // Initialize the score display
        UpdateScoreDisplay();
    }

    void Update()
    {
        // Increase the score based on the number of captured points
        if (capturedPoints.Count > 0)
        {
            foreach (var point in capturedPoints)
            {
                float pointsToAdd = point.GetScorePerSecond() * Time.deltaTime * scoreMultiplier;
                currentScore += pointsToAdd;
            }
            UpdateScoreDisplay();
        }
    }

    public void CapturePoint(CapturePoint point)
    {
        if (!capturedPoints.Contains(point))
        {
            capturedPoints.Add(point);
        }
    }

    void UpdateScoreDisplay()
    {
        scoreText.text = "Score: " + Mathf.RoundToInt(currentScore).ToString();
    }
}
