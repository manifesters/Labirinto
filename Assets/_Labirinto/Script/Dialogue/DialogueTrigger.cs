using Challenge;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [Header("Visual Cue")]
        [SerializeField] private Button npcButton;

        [Header("Main Canvas")]
        [SerializeField] private Canvas mainCanvas;

        [Header("Ink JSON")]
        [SerializeField] private TextAsset inkJSON;

        [Header("Quiz JSON")]
        [SerializeField] public TextAsset quizJson;

        private RectTransform buttonRectTransform;
        private bool playerInRange;

        private void Awake() 
        {
            playerInRange = false;
            npcButton.gameObject.SetActive(false);
            buttonRectTransform = npcButton.GetComponent<RectTransform>();
        }

        private void Update() 
        {
            if (playerInRange && !DialogueManager.Instance.dialogueIsPlaying) 
            {
                npcButton.gameObject.SetActive(true);

                // Move the button to the main canvas
                buttonRectTransform.SetParent(mainCanvas.transform, false);

                buttonRectTransform.anchoredPosition = new Vector2(280, -70);

                npcButton.onClick.RemoveAllListeners();
                npcButton.onClick.AddListener(() => 
                {
                    ChallengeManager.Instance.SetJson(quizJson);
                    DialogueManager.Instance.EnterDialogueMode(inkJSON);
                });
            }
            else 
            {
                npcButton.gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider) 
        {
            if (collider.gameObject.tag == "Player")
            {
                playerInRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collider) 
        {
            if (collider.gameObject.tag == "Player")
            {
                playerInRange = false;
                npcButton.gameObject.SetActive(false);
                buttonRectTransform.SetParent(this.transform);
            }
        }
    }
}
