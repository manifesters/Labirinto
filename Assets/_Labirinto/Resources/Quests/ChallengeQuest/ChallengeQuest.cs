using UnityEngine;

public class ChallengeQuestStep : QuestStep
{
    private void Start()
    {
        // Set the initial status
        string status = "Complete the required challenge.";
        ChangeState("", status);
    }

    private void OnEnable()
    {
        // Subscribe to challenge completion event
        GameEventsManager.Instance.challengeEvents.onChallengeComplete += OnChallengeCompleted;
    }

    private void OnChallengeCompleted()
    {
        // Trigger quest step completion upon any challenge completion
        string status = "Challenge completed successfully.";
        ChangeState("", status);
        Debug.Log(status);
        FinishQuestStep();
    }

    protected override void SetQuestStepState(string state)
    {
        // Optionally update state if needed
        // This is for when the state of this quest step is set externally
    }

    private void OnDisable()
    {
        // Unsubscribe from event when the object is disabled to avoid memory leaks
        GameEventsManager.Instance.challengeEvents.onChallengeComplete -= OnChallengeCompleted;
    }
}
