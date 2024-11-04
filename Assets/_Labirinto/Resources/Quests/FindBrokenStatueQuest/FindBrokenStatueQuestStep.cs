using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class FindBrokenStatueQuestStep : QuestStep
{

    [Header("Config")]
    [SerializeField] private string statueName;


    private void Start()
    {
        string status = "Find the " + statueName + " statue.";
        ChangeState("", status);
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            string status = "The " + statueName + " has found.";
            ChangeState("", status);
            Debug.Log(status);
            FinishQuestStep();
        }
    }

    protected override void SetQuestStepState(string state)
    {
        // none
    }
}
