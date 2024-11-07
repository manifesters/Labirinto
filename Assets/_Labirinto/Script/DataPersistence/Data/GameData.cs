using System.Collections.Generic;
using UnityEngine;

namespace DataPersistence
{

    [System.Serializable]
    public class QuestDataEntry
    {
        public string questName;
        public string questStatus;
    }

    [System.Serializable]
    public class GameData
    {
        public long lastUpdated;
        public string playerUID;
        public string playerName;
        public string playerSavedName;
        public int playerScore;
        public Vector2 playerPosition;

        // Quest
        public List<QuestDataEntry> questDataEntries;
        
        public GameData()
        {
            playerPosition = Vector2.zero;
            playerScore = 0;
            questDataEntries = new List<QuestDataEntry>();
        }

        public string GetPlayerSavedName()
        {
            return playerSavedName;
        }
    }
}
