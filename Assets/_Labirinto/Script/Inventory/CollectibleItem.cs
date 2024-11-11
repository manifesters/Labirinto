public class CollectibleItem
{
    public CollectibleItemSO info;
    public bool isCollected;

    // Constructor with default collected state as false
    public CollectibleItem(CollectibleItemSO itemInfo, bool collected = false)
    {
        info = itemInfo;
        isCollected = collected;
    }

    public void Collect()
    {
        isCollected = true;
    }
}
