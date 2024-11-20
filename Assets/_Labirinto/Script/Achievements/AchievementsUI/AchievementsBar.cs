using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AchievementsBar : MonoBehaviour
{
    [SerializeField] private GameObject achievementsBar;
    [SerializeField] private TextMeshProUGUI achievementName;
    [SerializeField] private TextMeshProUGUI achievementDesc;
    [SerializeField] private TextMeshProUGUI rewards;
    [SerializeField] private Button claimButton;
    [SerializeField] private TextMeshProUGUI claimButtonText;

    private UnityAction onSelectAction;

    public void Initialize(string displayName, string description, string reward, UnityAction selectAction)
    {
        this.achievementName.text = displayName;
        this.achievementDesc.text = description;
        this.rewards.text = reward;
        this.onSelectAction = selectAction;

        if (claimButton != null)
        {
            claimButton.onClick.AddListener(() => selectAction());
        }
        else
        {
            Debug.LogWarning("Claim button not assigned in the Achievements.");
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        onSelectAction?.Invoke();
    }

    public void TryingButton()
    {
        if (this.claimButton != null)
        {
            this.claimButtonText.text = "Trying";
            this.claimButton.interactable = false;
        }
    }

    public void ClaimedButton()
    {
        if (this.claimButton != null)
        {
            this.claimButtonText.text = "Claimed";
            this.claimButton.interactable = false;
        }
    }
}
