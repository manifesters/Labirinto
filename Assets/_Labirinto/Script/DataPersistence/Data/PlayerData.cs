using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{

    [System.Serializable]
    public class AchivementsDataEntry
    {
        public string achievementsName;
        public string achievementsStatus;
    }

    public string playerUID;
    public string playerName;
    public int playerMedal;
    public List<AchivementsDataEntry> achivementsDataEntries;

    public PlayerData()
    {
        playerMedal = 0;
        achivementsDataEntries = new List<AchivementsDataEntry>();
    }
}
