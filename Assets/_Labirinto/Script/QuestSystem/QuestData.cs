using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestData
{
    public QuestState state;
    public int questStepIndex;
    public QuestStepState[] questStepStates;
    public bool rewardClaimed;

    public QuestData(QuestState state, int questStepIndex, QuestStepState[] questStepStates, bool rewardClaimed)
    {
        this.state = state;
        this.questStepIndex = questStepIndex;
        this.questStepStates = questStepStates;
        this.rewardClaimed = rewardClaimed;
    }
}