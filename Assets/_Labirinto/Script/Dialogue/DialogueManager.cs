using System.Collections;
using System.Collections.Generic;
using Challenge;
using DataPersistence;
using Helper;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    public class DialogueManager : SingletonMonobehaviour<DialogueManager>
    {   
        [Header("Params")]
        [SerializeField] private float typingSpeed = 0.04f;

        [Header("Load Globals JSON")]
        [SerializeField] private TextAsset loadGlobalsJSON;

        [Header("Dialogue UI")]
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private TextMeshProUGUI npcNamePanel;
        [SerializeField] private Image npcPortraitPanel;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button skipButton;

        [Header("Choices UI")]
        [SerializeField] private GameObject[] choices;
        private TextMeshProUGUI[] choicesText;
        
        private Story currentStory;
        public bool dialogueIsPlaying {get; private set;}

        private bool canContinueToNextLine = false;
        private Coroutine displayLineCoroutine;
        private DialogueVariables dialogueVariables;

        // Quest
        private QuestPoint questPoint;

        public override void Awake()
        {
            base.Awake();
            dialogueVariables = new DialogueVariables(loadGlobalsJSON);
        }

        private void Start() 
        {
            dialogueIsPlaying = false;
            dialoguePanel.SetActive(false);
            continueButton.gameObject.SetActive(false);
            skipButton.onClick.AddListener(SkipDialogue);

            // to get all the choices text
            choicesText = new TextMeshProUGUI[choices.Length];
            int index = 0;
            foreach (GameObject choice in choices) 
            {
                choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();

                // choices button on click event
                Button choiceButton = choice.GetComponent<Button>();
                int capturedIndex = index;
                choiceButton.onClick.AddListener(() => MakeChoice(capturedIndex));
                
                index++;
            }
        }

        private void Update() 
        {
            // return if dialogue is not playing
            if (!dialogueIsPlaying) 
            {
                return;
            }
        
            // listen to the click on continue button to continue the story
            if (canContinueToNextLine)
            {
                continueButton.onClick.RemoveAllListeners();
                continueButton.onClick.AddListener(() => ContinueStory());
            }
        }

        public void EnterDialogueMode(TextAsset inkJSON, string npcName, Sprite npcPortraitSprite)
        {
            currentStory = new Story(inkJSON.text);
            dialogueIsPlaying = true;
            dialoguePanel.SetActive(true);

            // Set NPC name and portrait
            npcNamePanel.text = npcName;
            npcPortraitPanel.sprite = npcPortraitSprite;

            dialogueVariables.StartListening(currentStory);
            ContinueStory();
        }

        // Exit the dialogue
        private IEnumerator ExitDialogueMode() 
        {
            yield return new WaitForSeconds(0.2f);

            dialogueVariables.StopListening(currentStory);

            dialogueIsPlaying = false;
            dialoguePanel.SetActive(false);
            dialogueText.text = "";
            GameEventsManager.Instance.storyEvents.TriggerOnDialogue();
        }

        private void SkipDialogue()
        {
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
                displayLineCoroutine = null;
            }

            // Instantly display the full line of text
            dialogueText.maxVisibleCharacters = dialogueText.text.Length;

            // Allow continuation to next line or choices immediately
            continueButton.gameObject.SetActive(true);
            DisplayChoices();
            canContinueToNextLine = true;
        }


        private void ContinueStory()
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.button_click);
            while (currentStory.canContinue)
            {
                // Get the next line of the story
                string nextLine = currentStory.Continue().Trim();

                // Check for tags in the current line
                List<string> tags = currentStory.currentTags;
                foreach (string tag in tags)
                {
                    HandleTag(tag);
                }

                // Skip processing if the line is empty (e.g., just tags)
                if (!string.IsNullOrEmpty(nextLine))
                {
                    // Display the next line
                    if (displayLineCoroutine != null)
                    {
                        StopCoroutine(displayLineCoroutine);
                    }
                    displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));
                    return;
                }
            }

            // If no more lines, exit dialogue mode
            Debug.Log("Exit Dialogue");
            StartCoroutine(ExitDialogueMode());
        }


        private IEnumerator DisplayLine(string line) 
        {
            dialogueText.text = line;
            dialogueText.maxVisibleCharacters = 0;

            // hide while typing
            continueButton.gameObject.SetActive(false);
            HideChoices();

            canContinueToNextLine = false;

            bool isAddingRichTextTag = false;

            // display each letter one at a time
            foreach (char letter in line.ToCharArray())
            {
                // check for rich text tag, if found, add it without waiting
                if (letter == '<' || isAddingRichTextTag) 
                {
                    isAddingRichTextTag = true;
                    if (letter == '>')
                    {
                        isAddingRichTextTag = false;
                    }
                }
                // if not rich text, add the next letter and wait a small time
                else 
                {
                    dialogueText.maxVisibleCharacters++;
                    yield return new WaitForSeconds(typingSpeed);
                }
            }
            continueButton.gameObject.SetActive(true);
            DisplayChoices();
            canContinueToNextLine = true;
        }

        private void HideChoices() 
        {
            foreach (GameObject choiceButton in choices) 
            {
                choiceButton.SetActive(false);
            }
        }

        private void DisplayChoices() 
        {
            List<Choice> currentChoices = currentStory.currentChoices;

            // cheks the current choices lenght
            if (currentChoices.Count > choices.Length)
            {
                Debug.LogError("More choices were given than the UI can support. Number of choices given: " 
                    + currentChoices.Count);
            }

            int index = 0;
            // enable and initilize the choices
            foreach(Choice choice in currentChoices) 
            {
                choices[index].gameObject.SetActive(true);
                choicesText[index].text = choice.text;
                index++;
                continueButton.gameObject.SetActive(false);
            }
            // hide unused choices
            for (int i = index; i < choices.Length; i++) 
            {
                choices[i].gameObject.SetActive(false);
            }
        }

        public void MakeChoice(int choiceIndex)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.button_click);
            if (canContinueToNextLine) 
            {
                currentStory.ChooseChoiceIndex(choiceIndex);
                ContinueStory();
            }
        }

        public void SetQuestPoint(QuestPoint questPoint)
        {
            this.questPoint = questPoint;
        }

        private void HandleTag(string tag) 
        {
            switch (tag) 
            {
                case "START_CHALLENGE_QUIZ":
                    Debug.Log("Enter a challenge");
                    ChallengeManager.Instance.StartChallenge("Panel_QuizWindow");
                    break;
                case "START_CHALLENGE_MATCHIMAGE":
                    Debug.Log("Enter a challenge");
                    ChallengeManager.Instance.StartChallenge("Panel_MatchImageWindow");
                    break;
                case "START_CHALLENGE_MATCHWORD":
                    Debug.Log("Enter a challenge");
                    ChallengeManager.Instance.StartChallenge("Panel_MatchWordWindow");
                    break;
                case "START_CHALLENGE_PICTURE":
                    Debug.Log("Enter a challenge");
                    ChallengeManager.Instance.StartChallenge("Panel_GuessPictureWindow");
                    break;
                // Add more tags if necessary
                default:
                    Debug.Log("Unhandled tag: " + tag);
                    break;
            }
        }

        public Ink.Runtime.Object GetVariableState(string variableName) 
        {
            Ink.Runtime.Object variableValue = null;
            dialogueVariables.variables.TryGetValue(variableName, out variableValue);
            if (variableValue == null) 
            {
                Debug.LogWarning("Ink Variable was found to be null: " + variableName);
            }
            return variableValue;
        }

        public void OnApplicationQuit() 
        {
            dialogueVariables.SaveVariables();
        }
    }
}
