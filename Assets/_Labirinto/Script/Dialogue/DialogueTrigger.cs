using Challenge;
using TMPro;
using UnityEngine;

namespace Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [Header("Ink JSON")]
        [SerializeField] private TextAsset dialogueInkJSON;
        
        [Header("NPC Details")]
        [SerializeField] private string npcName;
        [SerializeField] private Sprite npcPortrait;

        public void EnterDialogue()
        {
            // Start the dialogue and quiz
            DialogueManager.Instance.EnterDialogueMode(dialogueInkJSON, npcName, npcPortrait);
        }
    }
}
