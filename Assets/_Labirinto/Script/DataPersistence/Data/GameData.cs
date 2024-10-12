using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace DataPersistence
{
    [System.Serializable]
    public class GameData
    {
        public long lastUpdated;
        public string playerUID;
        public string playerName;
        public string playerSavedName;
        public Vector2 playerPosition;

        public GameData()
        {
            playerPosition = Vector2.zero;
        }
    }
}
