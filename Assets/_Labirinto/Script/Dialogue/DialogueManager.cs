using System.Collections;
using System.Collections.Generic;
using DataPersistence;
using Helper;
using Ink.Runtime;
using Panel;
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
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Button continueButton;

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

        // Begin the dialogue when player click the interact button
        public void EnterDialogueMode(TextAsset inkJSON) 
        {
            currentStory = new Story(inkJSON.text);
            dialogueIsPlaying = true;
            dialoguePanel.SetActive(true);

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

        private void ContinueStory() 
        {
            if (currentStory.canContinue) 
                {
                    // Get the next line of the story
                    string nextLine = currentStory.Continue();

                    // Display the next line
                    if (displayLineCoroutine != null) 
                    {
                        StopCoroutine(displayLineCoroutine);
                    }
                    displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));

                    // Check for tags in the current line
                    List<string> tags = currentStory.currentTags;
                    foreach (string tag in tags) 
                    {
                        HandleTag(tag);
                    }
                }
                else 
                {
                    Debug.Log("Exit Dialogue");
                    StartCoroutine(ExitDialogueMode());
                }
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
                    ShowChallengePanel("Panel_QuizWindow", PanelShowBehaviour.KEEP_PREVIOUS); 
                    break;
                case "LOAD_QUARTER1":
                    Debug.Log("Load quarter 1 secene");
                    ScenesManager.Instance.LoadQuater1();
                    DataPersistenceManager.Instance.SaveGame();
                    break;
                case "START_CHALLENGE_MATCHPAIR":
                    Debug.Log("Enter a challenge");
                    ShowChallengePanel("Panel_MatchPairWindow", PanelShowBehaviour.KEEP_PREVIOUS); 
                    break;
                case "START_CHALLENGE_PICTURE":
                    Debug.Log("Enter a challenge");
                    ShowChallengePanel("Panel_GuessPictureWindow", PanelShowBehaviour.KEEP_PREVIOUS);
                    break;
                // Add more tags if necessary
                default:
                    Debug.Log("Unhandled tag: " + tag);
                    break;
            }
        }

        private void ShowChallengePanel(string panelID, PanelShowBehaviour behaviour = PanelShowBehaviour.KEEP_PREVIOUS) 
        {
            Debug.Log($"Starting the {panelID} challenge...");

            // Call the PanelManager directly to show the panel
            PanelManager.Instance.ShowPanel(panelID, behaviour);
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
