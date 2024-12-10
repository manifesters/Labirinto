using UnityEngine;

public class ChallengeQuestStep : QuestStep
{

    [Header("Config")]
    [SerializeField] private string challengeName;


    private void Start()
    {
        string status = "Complete the " + challengeName + " challenge.";
        ChangeState("", status);
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.challengeEvents.onChallengeComplete += QuestFinished;
    }


    private void QuestFinished()
    {
        string status = "Completed the " + challengeName + " challenge.";
        ChangeState("", status);
        Debug.Log(status);
        FinishQuestStep(); 
    }

    protected override void SetQuestStepState(string state)
    {
        // none
    }
}
