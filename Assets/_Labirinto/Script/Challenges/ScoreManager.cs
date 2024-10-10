using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText; // During game score display
    private int score = 0;

    // add points to the score
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    // deduct points from the score
    public void DeductScore(int points)
    {
        score -= points;
        if (score < 0) score = 0; // Ensure score doesn't go below 0
        UpdateScoreUI();
    }

    // update the score UI during gameplay
    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    // get the current score
    public int GetCurrentScore()
    {
        return score;
    }
}