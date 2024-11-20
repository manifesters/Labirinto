using UnityEngine;

[CreateAssetMenu(fileName = "AchievementSO", menuName = "ScriptableObjects/AchievementSO", order = 1)]
public class AchievementSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }

    [Header("Achievement Info")]
    public string achievementName;
    public string description;

    [Header("Reward Settings")]
    public int medalReward;

    private void OnValidate()
    {
        #if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
