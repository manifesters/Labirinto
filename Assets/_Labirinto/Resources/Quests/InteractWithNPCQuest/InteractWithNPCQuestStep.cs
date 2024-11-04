using UnityEngine;

public class InteractWithNPCQuestStep : QuestStep
{
    [Header("Config")]
    [SerializeField] private string npcName;

    private void Start()
    {
        string status = "Talk to " + npcName + " NPC.";
        ChangeState("", status);
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.storyEvents.onDialogue += Interacted;
    }

    private void Interacted()
    {

        string status = "Talked to " + npcName + " NPC.";
        ChangeState("", status);
        Debug.Log(status);
        FinishQuestStep(); 
    }

    protected override void SetQuestStepState(string state)
    {
        // this quest can start
    }
}
