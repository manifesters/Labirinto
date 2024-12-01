using System.Collections.Generic;
using UnityEngine;
using Helper;
using DataPersistence;
using System.Linq;
using TMPro;

public class QuestManager : SingletonMonobehaviour<QuestManager>, IDataPersistence
{
    [Header("Config")]
    [SerializeField] private bool loadQuestState = true;

    [Header("Guide Label")]
    [SerializeField] private TMP_Text inProgressQuest;

    public Dictionary<string, Quest> questMap { get; set;}
    private int finishedQuest;

    public override void Awake()
    {
        base.Awake(); 

        if (Instance == this)
        {
            Debug.Log("Created the default");
            questMap = CreateDefaultQuestMap();
        }
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.questEvents.onStartQuest += StartQuest;
        GameEventsManager.Instance.questEvents.onAdvanceQuest += AdvanceQuest;
        GameEventsManager.Instance.questEvents.onFinishQuest += FinishQuest;

        GameEventsManager.Instance.questEvents.onQuestStepStateChange += QuestStepStateChange;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.questEvents.onStartQuest -= StartQuest;
        GameEventsManager.Instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventsManager.Instance.questEvents.onFinishQuest -= FinishQuest;

        GameEventsManager.Instance.questEvents.onQuestStepStateChange -= QuestStepStateChange;
    }

    private void Start()
    {
        foreach (Quest quest in questMap.Values)
        {
            if (quest.state == QuestState.IN_PROGRESS)
            {
                quest.InstantiateCurrentQuestStep(this.transform);
            }

            if (quest.info.questType == "InteractWithNPC" && quest.state == QuestState.IN_PROGRESS)
            {
                // Reset the quest state to CAN_START
                quest.state = QuestState.CAN_START;
                Debug.Log("Quest State Reset");
            }

            GameEventsManager.Instance.questEvents.QuestStateChange(quest);
        }
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        GameEventsManager.Instance.questEvents.QuestStateChange(quest);
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        bool meetsRequirements = true;

        foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
        {
            if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
            {
                meetsRequirements = false;
            }
        }

        return meetsRequirements;
    }

    private void Update()
    {
        foreach (Quest quest in questMap.Values)
        {
            SetQuestLogInfo(quest);
            if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }

     private void SetQuestLogInfo(Quest quest)
    {
        if(quest.state == QuestState.CAN_START)
        {
            // quest name
            inProgressQuest.text = quest.info.displayName;
        }
    }

    private void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
    }

    private void AdvanceQuest(string id)
    {
        Quest quest = GetQuestById(id);
        quest.MoveToNextStep();

        if (quest.CurrentStepExists())
        {
            quest.InstantiateCurrentQuestStep(this.transform);
        }
        else
        {
            ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
        }
    }

    private void FinishQuest(string id)
    {
        Quest quest = GetQuestById(id);

        // Update quest state to finished
        ChangeQuestState(quest.info.id, QuestState.FINISHED);

        // Increment the number of finished quests
        finishedQuest++;

        HandleFinishedQuestActions();
    }

   private void HandleFinishedQuestActions()
    {
        switch (finishedQuest)
        {
            case 1:
                AchievementsManager.Instance.CompleteAchievement("1Tagapagsimula");
                Debug.Log("First quest NPC - Achievement unlocked: Tagapagsimula");
                break;

            case 5:
                AchievementsManager.Instance.CompleteAchievement("2Mananaliksik");
                Debug.Log("5 NPC quest - Achievement unlocked: Mananaliksik");
                break;

            case 10:
                AchievementsManager.Instance.CompleteAchievement("3Manlalakbay");
                Debug.Log("10 NPC quest - Achievement unlocked: Manlalakbay");
                break;

            case 15:
                AchievementsManager.Instance.CompleteAchievement("4Magsisiyasat");
                Debug.Log("15 NPC quest - Achievement unlocked: Magsisiyasat");
                break;

            case 20:
                AchievementsManager.Instance.CompleteAchievement("5Tagapag-tipon");
                Debug.Log("20 NPC quest - Achievement unlocked: Tagapag-tipon");
                break;

            case 25:
                AchievementsManager.Instance.CompleteAchievement("6Tagapag-usisa");
                Debug.Log("25 NPC quest - Achievement unlocked: Tagapag-usisa");
                break;

            case 30:
                AchievementsManager.Instance.CompleteAchievement("7Matipuno");
                Debug.Log("30 NPC quest - Achievement unlocked: Matipuno");
                break;

            case 35:
                AchievementsManager.Instance.CompleteAchievement("8Matyaga");
                Debug.Log("35 NPC quest - Achievement unlocked: Matyaga");
                break;

            case 40:
                AchievementsManager.Instance.CompleteAchievement("9Masigasig");
                Debug.Log("40 NPC quest - Achievement unlocked: Masigasig");
                break;

            case 45:
                AchievementsManager.Instance.CompleteAchievement("10Tagapagtuklas");
                Debug.Log("45 NPC quest - Achievement unlocked: Tagapagtuklas");
                break;

            case 50:
                AchievementsManager.Instance.CompleteAchievement("11Bayani");
                Debug.Log("50 NPC quest - Achievement unlocked: Bayani");
                break;

            case 55:
                AchievementsManager.Instance.CompleteAchievement("12Talino Tagapagsimula");
                Debug.Log("55 NPC quest - Achievement unlocked: Talino Tagapagsimula");
                break;

            default:
                Debug.Log($"Total finished quests: {finishedQuest}. Keep going!");
                break;
        }
    }



    private void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
    {
        Quest quest = GetQuestById(id);
        quest.StoreQuestStepState(questStepState, stepIndex);
        ChangeQuestState(id, quest.state);
    }

    private Dictionary<string, Quest> CreateQuestMap(GameData data)
    {
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();

        foreach (QuestInfoSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.id);
            }
            idToQuestMap.Add(questInfo.id, LoadQuest(questInfo, data));
        }
        Debug.Log("Questmap from quest entries in game data is loaded");
        return idToQuestMap;
    }

    // create default quest map if the questDataEntries is null
    private Dictionary<string, Quest> CreateDefaultQuestMap()
    {
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();

        foreach (QuestInfoSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.id);
            }
            idToQuestMap.Add(questInfo.id, new Quest(questInfo));
        }
        Debug.Log("Default questmap is loaded");
        return idToQuestMap;
    } 

    private Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogError("ID not found in the Quest Map: " + id);
        }
        return quest;
    }

    private Quest LoadQuest(QuestInfoSO questInfo, GameData data)
    {
        Quest quest = null;
        try 
        {
            if (data.questDataEntries.Any(entry => entry.questName == questInfo.id) && loadQuestState)
            {
                QuestDataEntry questDataEntry = data.questDataEntries.First(entry => entry.questName == questInfo.id);
                string serializedData = questDataEntry.questStatus;

                QuestData questData = JsonUtility.FromJson<QuestData>(serializedData);
                quest = new Quest(questInfo, questData.state, questData.questStepIndex, questData.questStepStates);
                quest.rewardClaimed = questData.rewardClaimed;
            }
            else 
            {
                quest = new Quest(questInfo);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load quest with id " + quest.info.id + ": " + e);
        }
        return quest;
    }

    public void LoadData(GameData data)
    {
        if (data.questDataEntries != null && data.questDataEntries.Count != 0)
        {
            questMap = CreateQuestMap(data);
            Debug.Log("Quest Entries Loaded");

            finishedQuest = questMap.Values.Count(quest => quest.state == QuestState.FINISHED);
        }
        else
        {
            Debug.Log("No entries on questDataEntries");
        }
    }

    public void SaveData(GameData data)
    {
        data.finishedQuest = finishedQuest;
        foreach (Quest quest in questMap.Values)
        {
            try 
            {
                QuestData questData = quest.GetQuestData();
                questData.rewardClaimed = quest.rewardClaimed;
                string serializedData = JsonUtility.ToJson(questData);
                
                QuestDataEntry entry = new QuestDataEntry
                {
                    questName = quest.info.id,
                    questStatus = serializedData
                };

                bool questExists = false;
                for (int i = 0; i < data.questDataEntries.Count; i++)
                {
                    if (data.questDataEntries[i].questName == entry.questName)
                    {
                        data.questDataEntries[i].questStatus = entry.questStatus;
                        questExists = true;
                        break;
                    }
                }

                if (!questExists)
                {
                    data.questDataEntries.Add(entry);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to save quest with id " + quest.info.id + ": " + e);
            }
        }
    }
}
