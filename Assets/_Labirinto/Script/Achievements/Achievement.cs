using UnityEngine;

public class Achievement
{
    public AchievementSO info;
    public bool isCompleted;
    public bool rewardClaimed;

    // Constructor with default completed and reward claimed states as false
    public Achievement(AchievementSO achievementInfo, bool completed = false, bool claimed = false)
    {
        info = achievementInfo;
        isCompleted = completed;
        rewardClaimed = claimed;
    }

    // Method to mark the achievement as completed
    public void Complete()
    {
        isCompleted = true;
    }

    // Method to claim the reward
    public void ClaimReward()
    {
        if (isCompleted && !rewardClaimed)
        {
            rewardClaimed = true;
            Debug.Log($"Reward claimed for: {info.achievementName}");
        }
        else
        {
            Debug.Log("Achievement not completed or reward already claimed.");
        }
    }
}
