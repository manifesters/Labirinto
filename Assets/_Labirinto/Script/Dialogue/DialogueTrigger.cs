using Challenge;
using UnityEngine;

namespace Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [Header("Ink JSON")]
        [SerializeField] private TextAsset dialogueInkJSON;

        [Header("Quiz JSON")]
        [SerializeField] public TextAsset challengeDataJSON;

        [Header("Game Object")]
        [SerializeField] private GameObject targetGameObject;  // Reference to the GameObject

        public void EnterDialogue()
        {
            // Log the name of the GameObject that triggers the dialogue
            Debug.Log($"Entering Dialogue Mode for GameObject: {targetGameObject.name}");

            // Optionally, do something with the GameObject (e.g., activate, deactivate, change properties)
            targetGameObject.SetActive(true); // Example: Activating the GameObject when dialogue starts

            // Start the dialogue and quiz
            DialogueManager.Instance.EnterDialogueMode(dialogueInkJSON);
            ChallengeManager.Instance.SetJson(challengeDataJSON);
        }

        // You can add additional logic here that works with the GameObject
        public void SetGameObjectActive(bool isActive)
        {
            if (targetGameObject != null)
            {
                targetGameObject.SetActive(isActive); // Control the GameObject’s active state
            }
        }
    }
}
