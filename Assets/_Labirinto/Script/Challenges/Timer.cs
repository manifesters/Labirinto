using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private float timeRemaining = 60f; // Set the time
    [SerializeField] private TextMeshProUGUI timerText;
    private GameController gameController; // Reference to GameController
    private bool isRunning = true; // control the timer running state

    void Start()
    {
        gameController = FindObjectOfType<GameController>(); // reference to GameController
    }

    void Update()
    {
        if (isRunning && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Ceil(timeRemaining).ToString();
        }
        else if (timeRemaining <= 0)
        {
            timeRemaining = 0; // Ensure time not go negative
            timerText.text = "Time: 0";
            isRunning = false;
            gameController.TimeUp(); // Notify GameController that time is up
        }
    }

    public void ReduceTime(float amount)
    {
        timeRemaining -= amount;
    }

    public void Stop()
    {
        isRunning = false; // Stop the timer
    }

    public void StartTimer(float initialTime)
    {
        timeRemaining = initialTime; // Reset time and start
        isRunning = true;
    }
    public float GetRemainingTime()
    {
        return timeRemaining;
    }
}
