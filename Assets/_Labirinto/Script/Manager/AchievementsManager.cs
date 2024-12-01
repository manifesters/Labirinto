using System.Collections.Generic;
using UnityEngine;
using Helper;
using DataPersistence;
using System.Linq;
using static DataPersistence.PlayerData;

public class AchievementsManager : SingletonMonobehaviour<AchievementsManager>, IPlayerDataPersistence
{
    public Dictionary<string, Achievement> achievementsMap { get; private set; }
    public override void Awake()
    {
        base.Awake();
        
        if (Instance == this)
        {
            achievementsMap = CreateDefaultAchievementsMap();
            Debug.Log("Default achievements map created.");
        }
    }

    private Dictionary<string, Achievement> CreateDefaultAchievementsMap()
    {
        AchievementSO[] allAchievements = Resources.LoadAll<AchievementSO>("Achievements");
        Dictionary<string, Achievement> idToAchievementMap = new Dictionary<string, Achievement>();

        foreach (var achievementInfo in allAchievements)
        {
            if (idToAchievementMap.ContainsKey(achievementInfo.id))
            {
                Debug.LogWarning("Duplicate ID found when creating achievement map: " + achievementInfo.id);
            }
            idToAchievementMap.Add(achievementInfo.id, new Achievement(achievementInfo));
        }

        Debug.Log("Default achievements map loaded.");
        return idToAchievementMap;
    }

    private Dictionary<string, Achievement> CreateAchievementsMap(PlayerData playerData)
    {
        AchievementSO[] allAchievements = Resources.LoadAll<AchievementSO>("Achievements");
        Dictionary<string, Achievement> idToAchievementMap = new Dictionary<string, Achievement>();

        foreach (var achievementInfo in allAchievements)
        {
            if (idToAchievementMap.ContainsKey(achievementInfo.id))
            {
                Debug.LogWarning("Duplicate ID found when creating achievement map: " + achievementInfo.id);
            }
            else
            {
                var existingEntry = playerData.achievementsDataEntries.FirstOrDefault(entry => entry.achievementName == achievementInfo.id);
                bool completed = existingEntry.isCompleted;
                bool rewardClaimed = existingEntry.isRewardClaimed;
                idToAchievementMap.Add(achievementInfo.id, new Achievement(achievementInfo, rewardClaimed));
            }
        }

        Debug.Log("Achievements map loaded from player data.");
        return idToAchievementMap;
    }

    // call when achievements is completed
    public void CompleteAchievement (string achievementId)
    {
       this.achievementsMap[achievementId].isCompleted = true;
       Debug.Log("Achievement Completed");
       DataPersistenceManager.Instance.SavePlayer();
    }

    public void LoadPlayerData(PlayerData playerData)
    {
        // Ensure the achievements map exists
        if (achievementsMap == null)
        {
            achievementsMap = CreateDefaultAchievementsMap();
        }

        // Iterate through all entries in PlayerData
        foreach (var dataEntry in playerData.achievementsDataEntries)
        {
            // Check if the achievement exists in the current map
            if (achievementsMap.TryGetValue(dataEntry.achievementName, out var achievement))
            {
                // Update the existing achievement with data from PlayerData
                achievement.isCompleted = dataEntry.isCompleted;
                achievement.rewardClaimed = dataEntry.isRewardClaimed;
            }
            else
            {
                // Log a warning if an achievement in PlayerData doesn't exist in the map
                Debug.LogWarning($"Achievement ID {dataEntry.achievementName} not found in the default map.");
            }
        }

        Debug.Log("Achievements map loaded with player data.");
    }

    public void SavePlayerData(PlayerData playerData)
    {
        // Iterate through all achievements in the current achievements map
        foreach (var achievement in achievementsMap.Values)
        {
            // Find an existing entry in the player data with the same achievement ID
            var existingEntry = playerData.achievementsDataEntries
                .FirstOrDefault(entry => entry.achievementName == achievement.info.id);

            if (existingEntry != null)
            {
                // Update the existing entry
                existingEntry.isCompleted = achievement.isCompleted;
                existingEntry.isRewardClaimed = achievement.rewardClaimed;
            }
            else
            {
                // Add a new entry if it doesn't exist
                AchievementsDataEntry newEntry = new AchievementsDataEntry
                {
                    achievementName = achievement.info.id,
                    isCompleted = achievement.isCompleted,
                    isRewardClaimed = achievement.rewardClaimed
                };
                
                playerData.achievementsDataEntries.Add(newEntry);
            }
        }
        
        Debug.Log("Achievements map saved to player data.");
    }
}
