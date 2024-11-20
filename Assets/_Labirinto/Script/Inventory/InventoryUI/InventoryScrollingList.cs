using System.Collections.Generic;
using UnityEngine;

public class InventoryScrollingList : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject contentParent;

    [Header("Inventory Item Prefab")]
    [SerializeField] private GameObject itemPrefab;

    [SerializeField] private InventoryDescription itemDescription;

    private Dictionary<string, InventoryItem> idToInventoryMap = new Dictionary<string, InventoryItem>();

    public Sprite image;

    private void Start()
    {
         if (CollectibleManager.Instance == null)
        {
            Debug.LogError("CollectibleManager.Instance is null. Ensure CollectibleManager is initialized.");
            return;
        }
        
        Dictionary<string, CollectibleItem> items = CollectibleManager.Instance.collectibleItemMap;
        foreach (CollectibleItem item in items.Values)
        {
            if (item.isCollected == true) {
                Debug.Log(item.info.id);
                CreateItemIfNotExists(item);
            }
        }
    }

    public InventoryItem CreateItemIfNotExists(CollectibleItem item)
    {
        InventoryItem inventoryItem = null;

        if (!idToInventoryMap.ContainsKey(item.info.id))
        {
            inventoryItem = InstantiateItem(item);
        }
        else
        {
            inventoryItem = idToInventoryMap[item.info.id];
        }

        return inventoryItem;
    }

    private InventoryItem InstantiateItem(CollectibleItem item)
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab, contentParent.transform).GetComponent<InventoryItem>();

        if (inventoryItem == null)
        {
            Debug.LogError("InventoryItem component is missing from the itemPrefab.");
            return null;
        }

        inventoryItem.gameObject.name = item.info.id + "_item";

        // Initialize with sprite and onSelect action
        inventoryItem.Initialize(item.info.icon, () => OnItemSelected(item));

        idToInventoryMap[item.info.id] = inventoryItem;
        return inventoryItem;
    }

    private void OnItemSelected(CollectibleItem item)
    {
        if (idToInventoryMap.TryGetValue(item.info.id, out InventoryItem inventoryItem))
        {
            Debug.Log("Selected Item: " + item.info.displayName);
			itemDescription.SetDescription(item.info.icon, item.info.displayName, item.info.description);

		}
    }
}
