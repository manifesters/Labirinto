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
    public class CollectibleItemDataEntry
    {
        public string itemId;
        public bool isCollected;
    }

    [System.Serializable]
    public class GameData
    {
        public long lastUpdated;
        public string playerSavedName;
        public int playerScore;
        public string lastScene;
        public Vector2 playerPosition;

        // Quest
        public List<QuestDataEntry> questDataEntries;
        public List<CollectibleItemDataEntry> collectibleItemDataEntries;
        
        public GameData()
        {
            playerPosition = Vector2.zero;
            playerScore = 0;
            lastScene = "Labirinto";
            questDataEntries = new List<QuestDataEntry>();
            collectibleItemDataEntries = new List<CollectibleItemDataEntry>();
        }

        public string GetPlayerSavedName()
        {
            return playerSavedName;
        }
    }
}
