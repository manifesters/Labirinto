using System.Collections.Generic;
using UnityEngine;
using Helper;
using DataPersistence;
using System.Linq;

public class CollectibleManager : SingletonMonobehaviour<CollectibleManager>, IDataPersistence
{
    [Header("Config")]
    [SerializeField] private bool loadCollectibleState = true;

    public Dictionary<string, CollectibleItem> collectibleItemMap { get; private set; }
    private int itemCollected;

    public override void Awake()
    {
        base.Awake();
        
        if (Instance == this)
        {
            Debug.Log("Created the default collectible manager");
            collectibleItemMap = CreateDefaultCollectibleMap();
        }
    }

    private Dictionary<string, CollectibleItem> CreateDefaultCollectibleMap()
    {
        CollectibleItemSO[] allCollectibles = Resources.LoadAll<CollectibleItemSO>("Collectibles");
        Dictionary<string, CollectibleItem> idToCollectibleMap = new Dictionary<string, CollectibleItem>();

        foreach (var collectibleInfo in allCollectibles)
        {
            if (idToCollectibleMap.ContainsKey(collectibleInfo.id))
            {
                Debug.LogWarning("Duplicate ID found when creating collectible map: " + collectibleInfo.id);
            }
            idToCollectibleMap.Add(collectibleInfo.id, new CollectibleItem(collectibleInfo));
        }
        Debug.Log("Default collectible map loaded");
        return idToCollectibleMap;
    }

    private Dictionary<string, CollectibleItem> CreateCollectibleMap(GameData data)
    {
        CollectibleItemSO[] allCollectibles = Resources.LoadAll<CollectibleItemSO>("Collectibles");
        Dictionary<string, CollectibleItem> idToCollectibleMap = new Dictionary<string, CollectibleItem>();

        foreach (var collectibleInfo in allCollectibles)
        {
            if (idToCollectibleMap.ContainsKey(collectibleInfo.id))
            {
                Debug.LogWarning("Duplicate ID found when creating collectible map: " + collectibleInfo.id);
            }
            else
            {
                // Load collectible item state if it exists in the data
                var existingEntry = data.collectibleItemDataEntries.FirstOrDefault(entry => entry.itemId == collectibleInfo.id);
                bool isCollected = existingEntry != null ? existingEntry.isCollected : false;
                idToCollectibleMap.Add(collectibleInfo.id, new CollectibleItem(collectibleInfo, isCollected));
            }
        }
        Debug.Log("Collectible map loaded from game data entries");
        return idToCollectibleMap;
    }

    public void CollectItem(string itemId)
    {
        if (collectibleItemMap.ContainsKey(itemId))
        {
            CollectibleItem item = collectibleItemMap[itemId];
            item.Collect();
            itemCollected++;
            Debug.Log($"{item.info.displayName} has been collected!");
            HandleCollectibleAchievements();
        }
        else
        {
            Debug.LogWarning("Item not found: " + itemId);
        }
    }

    private void HandleCollectibleAchievements()
    {
        switch (itemCollected)
        {
            case 1:
                AchievementsManager.Instance.CompleteAchievement("1Unang Bagay");
                Debug.Log("First collectible item acquired! Achievement unlocked: Unang Bagay");
                break;

            case 5:
                AchievementsManager.Instance.CompleteAchievement("2Mangangalap");
                Debug.Log("Two collectible items acquired! Achievement unlocked: Mangangalap");
                break;

            case 10:
                AchievementsManager.Instance.CompleteAchievement("3Matalas");
                Debug.Log("Three collectible items acquired! Achievement unlocked: Matalas");
                break;

            case 15:
                AchievementsManager.Instance.CompleteAchievement("4Panaliksik");
                Debug.Log("Four collectible items acquired! Achievement unlocked: Panaliksik");
                break;

            case 20:
                AchievementsManager.Instance.CompleteAchievement("5Tagakolekta");
                Debug.Log("Five collectible items acquired! Achievement unlocked: Tagakolekta");
                break;

            case 25:
                AchievementsManager.Instance.CompleteAchievement("6Tagapag-archive");
                Debug.Log("Six collectible items acquired! Achievement unlocked: Tagapag-archive");
                break;

            case 30:
                AchievementsManager.Instance.CompleteAchievement("7Manlalakbay");
                Debug.Log("Seven collectible items acquired! Achievement unlocked: Manlalakbay");
                break;

            case 35:
                AchievementsManager.Instance.CompleteAchievement("8Makabuo");
                Debug.Log("Eight collectible items acquired! Achievement unlocked: Makabuo");
                break;

            case 40:
                AchievementsManager.Instance.CompleteAchievement("9Kaalaman");
                Debug.Log("Nine collectible items acquired! Achievement unlocked: Kaalaman");
                break;

            default:
                Debug.Log($"Total items collected: {itemCollected}. Keep collecting!");
                break;
        }
    }


    public bool IsItemCollected(string itemId)
    {
        return collectibleItemMap.ContainsKey(itemId) && collectibleItemMap[itemId].isCollected;
    }

    public void LoadData(GameData data)
    {
        if (data.collectibleItemDataEntries != null && data.collectibleItemDataEntries.Count > 0 && loadCollectibleState)
        {
            collectibleItemMap = CreateCollectibleMap(data);

            // Check the loaded data to count how many items are already collected
            itemCollected = data.collectibleItemDataEntries.Count(entry => entry.isCollected);

            Debug.Log("Collectible entries loaded from game data");
        }
        else
        {
            Debug.Log("No collectible entries in game data; using default map");
        }
    }

    public void SaveData(GameData data)
    {
        data.itemCollected = itemCollected;
        foreach (var item in collectibleItemMap.Values)
        {
            var existingEntry = data.collectibleItemDataEntries.Find(entry => entry.itemId == item.info.id);
            if (existingEntry != null)
            {
                existingEntry.isCollected = item.isCollected;
            }
            else
            {
                CollectibleItemDataEntry newEntry = new CollectibleItemDataEntry
                {
                    itemId = item.info.id,
                    isCollected = item.isCollected
                };
                data.collectibleItemDataEntries.Add(newEntry);
            }
        }
    }
}
