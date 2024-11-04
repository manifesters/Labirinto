using Challenge;
using UnityEngine;

namespace Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [Header("Ink JSON")]
        [SerializeField] private TextAsset inkJSON;

        [Header("Quiz JSON")]
        [SerializeField] public TextAsset quizJson;

        public void EnterDialogue()
        {
            Debug.Log("Entering Dialogue Mode");
            DialogueManager.Instance.EnterDialogueMode(inkJSON);
            // Here you might also want to set the quiz JSON if needed
            ChallengeManager.Instance.SetJson(quizJson);
        }
    }
}
