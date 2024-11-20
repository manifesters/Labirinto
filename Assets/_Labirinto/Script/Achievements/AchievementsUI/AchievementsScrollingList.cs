using System;
using System.Collections;
using System.Collections.Generic;
using Medal;
using UnityEngine;

public class AchievementsScrollingList : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject contentParent;

    [Header("Achievement Bar")]
    [SerializeField] private GameObject achievementBarPrefab;

    private Dictionary<string, AchievementsBar> idToBarMap = new Dictionary<string, AchievementsBar>();

    private void Start()
    {
        Dictionary<string, Achievement> achievements = AchievementsManager.Instance.achievementsMap;

        foreach (Achievement achievement in achievements.Values)
        {
            CreateBarIfNotExists(achievement);
        }
    }

    public AchievementsBar CreateBarIfNotExists(Achievement achievement) 
    {
        AchievementsBar achievementsBarPanel = null;
        
        // Check if the achievement panel already exists
        if (!idToBarMap.ContainsKey(achievement.info.id))
        {
            // If not, instantiate a new one
            achievementsBarPanel = InstantiateAchievementPanel(achievement);
        }
        else 
        {
            // If it exists, retrieve it from the map
            achievementsBarPanel = idToBarMap[achievement.info.id];
        }

        // If the achievement is complete, check if the reward is claimed
        if (achievement.isCompleted)
        {
            if (achievement.rewardClaimed)
            {
                // If the reward is claimed, disable the button
                achievementsBarPanel.ClaimedButton();
            }
        }
        else
        {
            // If the achievement is not completed, disable the button
            achievementsBarPanel.TryingButton();
        }

        return achievementsBarPanel;
    }


    private AchievementsBar InstantiateAchievementPanel(Achievement achievement)
    {
        AchievementsBar achievementsBar = Instantiate(achievementBarPrefab, contentParent.transform).GetComponent<AchievementsBar>();
        
        if (achievementsBar == null)
        {
            Debug.LogError("AchievementBar component is missing from the achievementBarPrefab.");
            return null;
        }

        achievementsBar.gameObject.name = achievement.info.id + "_button";

        // Initialize with display name, reward, and onSelect action
        achievementsBar.Initialize(achievement.info.achievementName, achievement.info.description, achievement.info.medalReward.ToString(), () => OnQuestSelected(achievement));

        idToBarMap[achievement.info.id] = achievementsBar;
        return achievementsBar;
    }

    private void OnQuestSelected(Achievement achievement)
    {
        if (idToBarMap.TryGetValue(achievement.info.id, out AchievementsBar achievementsBar) && !achievement.rewardClaimed)
        {
            achievementsBar.ClaimedButton();
            Debug.Log("Rewards Claimed: " + achievement.info.id);
            MedalManager.Instance.AddMedal(achievement.info.medalReward);
            AchievementsManager.Instance.achievementsMap[achievement.info.id].rewardClaimed = true;
        }
    }
}
