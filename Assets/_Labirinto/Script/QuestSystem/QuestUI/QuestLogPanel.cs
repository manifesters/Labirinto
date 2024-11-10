using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class QuestLogPanel : MonoBehaviour, ISelectHandler
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button claimButton;
    [SerializeField] private TextMeshProUGUI claimButtonText;
    [SerializeField] private TextMeshProUGUI questNameText;
    [SerializeField] private TextMeshProUGUI rewardsText;
    private UnityAction onSelectAction;

    // Initialize panel with display name, reward, and onSelect action
    public void Initialize(string displayName, string reward, UnityAction selectAction)
    {
        this.questNameText.text = displayName;
        this.rewardsText.text = reward;
        this.onSelectAction = selectAction;

        if (claimButton != null)
        {
            claimButton.onClick.AddListener(() => selectAction());
        }
        else
        {
            Debug.LogWarning("Claim button not assigned in the QuestLogPanel.");
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        onSelectAction?.Invoke();
    }

    public void DisableButton()
    {
        if (this.claimButton != null)
        {
            this.claimButtonText.text = "Claimed";
            this.claimButton.interactable = false;
        }
    }
}
