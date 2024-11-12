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
            Debug.Log($"{item.info.displayName} has been collected!");
        }
        else
        {
            Debug.LogWarning("Item not found: " + itemId);
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
            Debug.Log("Collectible entries loaded from game data");
        }
        else
        {
            Debug.Log("No collectible entries in game data; using default map");
        }
    }

    public void SaveData(GameData data)
    {
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
