using System.Collections.Generic;
using LootLocker.Requests;
using UnityEngine;

public class LeaderboardScrollingList : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject contentParent;
    [SerializeField] private GameObject leaderboardEntryPrefab;
    private Dictionary<string, LeaderboardBar> idToEntryMap = new Dictionary<string, LeaderboardBar>();

    private const string LEADERBOARD_KEY = "25344";
    private void Start()
    {
        GetTopPlayers();
    }

    public void GetTopPlayers()
    {
        int count = 10;
        LootLockerSDKManager.GetScoreList(LEADERBOARD_KEY, count, 0, (response) =>
        {
            if (response.success)
            {
                LootLockerLeaderboardMember[] members = response.items;

                // Clear existing entries
                ClearExistingEntries();

                foreach (var member in members)
                {
                    CreateEntry(member.rank.ToString(), member.player.name, member.score.ToString());
                }

                Debug.Log("Successfully got score list!");
            }
            else
            {
                Debug.LogError("Failed to fetch leaderboard data: " + response.errorData);
            }
        });
    }

    private void CreateEntry(string rank, string playerName, string score)
    {
        LeaderboardBar leaderboardEntry = Instantiate(leaderboardEntryPrefab, contentParent.transform)
            .GetComponent<LeaderboardBar>();

        if (leaderboardEntry == null)
        {
            Debug.LogError("LeaderboardEntry component is missing from the leaderboardEntryPrefab.");
            return;
        }

        leaderboardEntry.Initialize(playerName, score, rank);
    }

    private void ClearExistingEntries()
    {
        foreach (Transform child in contentParent.transform)
        {
            Destroy(child.gameObject);
        }

        idToEntryMap.Clear();
    }
}
