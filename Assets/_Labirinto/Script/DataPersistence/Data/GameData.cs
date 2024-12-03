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
        public int finishedQuest;
        public int itemCollected;
        public Vector2 playerPosition;
		public float audioVolume;

		// Quest
		public List<QuestDataEntry> questDataEntries;
        public List<CollectibleItemDataEntry> collectibleItemDataEntries;
        
        public GameData()
        {
            playerPosition = new Vector2(-3, -95);
            playerScore = 0;
            finishedQuest = 0;
            itemCollected = 0;
            questDataEntries = new List<QuestDataEntry>();
            collectibleItemDataEntries = new List<CollectibleItemDataEntry>();
        }

        public string GetPlayerSavedName()
        {
            return playerSavedName;
        }
    }
}
