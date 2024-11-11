using UnityEngine;

[CreateAssetMenu(fileName = "CollectibleItemSO", menuName = "ScriptableObjects/CollectibleItemSO", order = 1)]
public class CollectibleItemSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }
    
    [Header("General")]
    public string displayName;
    public string description;

    [Header("Item Properties")]
    public Sprite icon;

    private void OnValidate()
    {
        #if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
