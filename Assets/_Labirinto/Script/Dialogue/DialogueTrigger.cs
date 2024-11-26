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

        public void EnterDialogue()
        {
            // Start the dialogue and quiz
            DialogueManager.Instance.EnterDialogueMode(dialogueInkJSON);
            ChallengeManager.Instance.SetJson(challengeDataJSON);
        }
    }
}
