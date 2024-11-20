using System.Collections.Generic;
using UnityEngine;

namespace DataPersistence
{
    [System.Serializable]
    public class PlayerData
    {
        [System.Serializable]
        public class AchievementsDataEntry
        {
            public string achievementName;
            public bool isCompleted;
            public bool isRewardClaimed;
        }

        public string playerUID;
        public string playerName;
        public int playerMedal;
        public List<AchievementsDataEntry> achievementsDataEntries;

        public PlayerData()
        {
            playerMedal = 0;
            achievementsDataEntries = new List<AchievementsDataEntry>();
        }
    }
}