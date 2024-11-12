using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Score;

public class GameController : MonoBehaviour
{
    [SerializeField] private Timer timer; // Reference to Timer
    [SerializeField] private GameObject resultPanel; // Result panel for showing final score
    [SerializeField] private TextMeshProUGUI resultText; // Text for displaying result message
    [SerializeField] private int totalCorrectAnswers = 5; // Total correct answers required
    private int correctAnswersPlaced = 0;
    private PointManager scoreManager; // Reference to ScoreManager

    private void Start()
    {
        resultPanel.SetActive(false); // Hide result panel at start
        scoreManager = FindObjectOfType<PointManager>();
    }

    public void CorrectAnswerPlaced()
    {
        correctAnswersPlaced++;
        scoreManager.AddScore(10); // Add score when a correct answer is placed

        // Check if the player placed all the correct answers
        if (correctAnswersPlaced >= totalCorrectAnswers)
        {
            EndGame(true); // End game with a win condition
        }
    }
    public void IncorrectAnswerPlaced()
    {
        timer.ReduceTime(5); // Reduce time if an incorrect answer
    }

    public void TimeUp()
    {
        EndGame(false); // End the game when the time is up
    }

    public void EndGame(bool isWin)
    {
        timer.Stop();

        if (isWin)
        {
            // Add bonus points based on remaining time if player wins
            int bonusPoints = Mathf.CeilToInt(timer.GetRemainingTime() * 2); //2 points per second remaining
            scoreManager.AddScore(bonusPoints); // Add bonus points to the final score
            resultText.text = "Congratulations! You placed all the correct words!\nFinal Score: " + scoreManager.GetCurrentScore();
        }
        else
        {
            // When time is up, just show the current score without bonus
            resultText.text = "Betterluck next time.\nScore: " + scoreManager.GetCurrentScore();
        }
        resultPanel.SetActive(true); // Show the result panel with the final score
    }
}