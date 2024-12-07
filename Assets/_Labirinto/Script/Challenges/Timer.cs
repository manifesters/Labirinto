using UnityEngine;

public class Timer : MonoBehaviour
{
    public GameObject timerBar;
    [SerializeField] private float timeRemaining = 60f;  // Initial time in seconds

    public GameObject pausePanel;

    private bool isRunning = true;

    // Define a delegate and event for when the time ends
    public delegate void TimeEndAction();
    public event TimeEndAction OnTimeEnd;  // This is the event

    void Update()
    {
        if (isRunning && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;  // Decrease time
            AnimateBar();  // Animate the timer bar
        }
        else if (timeRemaining <= 0 && isRunning)
        {
            timeRemaining = 0;
            AnimateBar();  // Final update for the bar when time is up
            isRunning = false;

            // Trigger the OnTimeEnd event
            OnTimeEnd?.Invoke();
        }
    }

    // Animates the timer bar to show remaining time
    private void AnimateBar()
    {
        float normalizedTime = Mathf.Clamp(timeRemaining / 60f, 0f, 1f);
        LeanTween.scaleX(timerBar, normalizedTime, 0.1f);  // Scale the bar based on remaining time
    }

    // Returns the remaining time
    public float GetRemainingTime()
    {
        return timeRemaining;
    }

    public void ToggleTimer()
    {
        isRunning = !isRunning;
        pausePanel.SetActive(true);
    }

    public void TimeContinue()
    {
        isRunning = !isRunning;
        pausePanel.SetActive(false);
    }

}
