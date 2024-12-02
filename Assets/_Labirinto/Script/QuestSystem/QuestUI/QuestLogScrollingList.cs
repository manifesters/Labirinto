using System.Collections;
using System.Collections.Generic;
using Score;
using UnityEngine;

public class QuestLogScrollingList : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject contentParent;

    [Header("Quest Log Panel")]
    [SerializeField] private GameObject questLogPanelPrefab;

    private Dictionary<string, QuestLogPanel> idToPanelMap = new Dictionary<string, QuestLogPanel>();

    private void Start()
    {
        Dictionary<string, Quest> quests = QuestManager.Instance.questMap;
        foreach (Quest quest in quests.Values)
        {
            if (quest.state == QuestState.FINISHED) {
                Debug.Log(quest.info.id);
                CreatePanelIfNotExists(quest);
            }
        }
    }

    public QuestLogPanel CreatePanelIfNotExists(Quest quest) 
    {
        QuestLogPanel questLogPanel = null;
        
        if (!idToPanelMap.ContainsKey(quest.info.id))
        {
            questLogPanel = InstantiateQuestLogPanel(quest);
        }
        else 
        {
            questLogPanel = idToPanelMap[quest.info.id];
        }

        if (quest.rewardClaimed)
        {
            questLogPanel.DisableButton();
        }

        return questLogPanel;
    }

    private QuestLogPanel InstantiateQuestLogPanel(Quest quest)
    {
        // Instantiate the quest log panel and get the QuestLogPanel component
        QuestLogPanel questLogPanel = Instantiate(questLogPanelPrefab, contentParent.transform).GetComponent<QuestLogPanel>();
        
        if (questLogPanel == null)
        {
            Debug.LogError("QuestLogPanel component is missing from the questLogPanelPrefab.");
            return null;
        }

        questLogPanel.gameObject.name = quest.info.id + "_button";

        // Initialize with display name, reward, and onSelect action
        questLogPanel.Initialize(quest.info.displayName, quest.info.pointReward.ToString(), () => OnQuestSelected(quest));

        idToPanelMap[quest.info.id] = questLogPanel;
        return questLogPanel;
    }

    private void OnQuestSelected(Quest quest)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.button_click);
        if (idToPanelMap.TryGetValue(quest.info.id, out QuestLogPanel questLogPanel) && !quest.rewardClaimed)
        {
            questLogPanel.DisableButton();
            Debug.Log("Rewards Claimed: " + quest.info.id);
            PointManager.Instance.AddScore(quest.info.pointReward);
            QuestManager.Instance.questMap[quest.info.id].rewardClaimed = true;
        }
    }
}
