using Dialogue;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CircleCollider2D))]
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoForPoint;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;

    [Header("Interact Button")]
    [SerializeField] private Button interactButton;
    [SerializeField] private GameObject panel;

    private Button interactButtonComponent; // Reference to the Button component
    private bool playerIsNear = false;
    private string questId;
    private QuestState currentQuestState;

    private QuestIcon questIcon;
    private DialogueTrigger dialogueTrigger;

    private RectTransform buttonRectTransform;

    private void Awake() 
    {
        questId = questInfoForPoint.id;
        questIcon = GetComponentInChildren<QuestIcon>();
        dialogueTrigger = GetComponentInChildren<DialogueTrigger>();

       // hide interact button
        interactButton.gameObject.SetActive(false);
        buttonRectTransform = interactButton.GetComponent<RectTransform>();

        // listner on interact button
        interactButtonComponent = interactButton.GetComponent<Button>();
        interactButtonComponent.onClick.AddListener(OnInteractButtonClicked);
    }

    private void OnEnable()
    {
        // Register for quest state changes
        GameEventsManager.Instance.questEvents.onQuestStateChange += QuestStateChange;
    }

    private void OnDisable()
    {
        // Unregister to prevent memory leaks
        GameEventsManager.Instance.questEvents.onQuestStateChange -= QuestStateChange;
    }

    private void OnInteractButtonClicked()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.button_click);
        SubmitPressed(); // Call the method to handle quest submission
    }

    private void SubmitPressed()
    {
        if (!playerIsNear)
        {
            return;
        }

        // Start or finish a quest
        if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
        {
            GameEventsManager.Instance.questEvents.StartQuest(questId);
            // Enter dialogue mode when starting the quest
            dialogueTrigger.EnterDialogue();
        }
        else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
        {
            GameEventsManager.Instance.questEvents.FinishQuest(questId);
        }

        // Optionally hide the button after interaction
        interactButton.gameObject.SetActive(false);
    }

    private void QuestStateChange(Quest quest)
    {
        // Only update the quest state if this point has the corresponding quest
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            questIcon.SetState(currentQuestState, startPoint, finishPoint);
            UpdateButtonVisibility();
        }
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            playerIsNear = true;
            UpdateButtonVisibility();
        }
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            playerIsNear = false;
            interactButton.gameObject.SetActive(false);
            buttonRectTransform.SetParent(this.transform);
        }
    }

    private void UpdateButtonVisibility()
    {
        if (playerIsNear && (currentQuestState.Equals(QuestState.CAN_START) || currentQuestState.Equals(QuestState.CAN_FINISH)))
        {
            if (interactButton.transform.parent != panel.transform)
            {
                buttonRectTransform.SetParent(panel.transform, false);

                // Optionally adjust the button's position within the panel
                buttonRectTransform.anchoredPosition = new Vector2(300, -70);
            }

            interactButton.gameObject.SetActive(true);
        }
        else
        {
            interactButton.gameObject.SetActive(false);
        }
    }
}
