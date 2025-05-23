using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;
using Challenge;
using Score;

namespace QuizChallenge
{
    public class QuizManager : MonoBehaviour
    {
        [Header("Quiz Elements")]
        [SerializeField] private GameObject quizPanel;
        [SerializeField] private TextMeshProUGUI questionText;
        [SerializeField] private List<Toggle> answerToggles;
        [SerializeField] private ToggleGroup answerToggleGroup;
        [SerializeField] private TextMeshProUGUI challengeName;
        [SerializeField] private TextMeshProUGUI instructionDesc;
        [SerializeField] private Timer gameTimer; 

        private List<Question> questions;
        private int currentQuestionIndex = 0;
        private int score = 0;
        private readonly int scorePerQuestion = 10;

        private void Start()
        {
            // Load quiz data from ChallengeManager
            LoadQuizFromChallengeManager();

            // Attach timer event if applicable
            if (gameTimer != null)
            {
                gameTimer.OnTimeEnd += EndQuiz; // Ends quiz when time runs out
            }
            else
            {
                Debug.LogError("Game Timer is not assigned!");
            }
        }

        private void LoadQuizFromChallengeManager()
        {
            TextAsset quizJson = ChallengeManager.Instance.CurrentChallengeJson;

            if (quizJson != null)
            {
                LoadQuiz(quizJson);
            }
            else
            {
                Debug.LogError("No quiz JSON found in ChallengeManager!");
            }
        }

        private void LoadQuiz(TextAsset quizJson)
        {
            if (quizJson != null)
            {
                string jsonText = quizJson.text;
                ChallengeData challengeData = JsonConvert.DeserializeObject<ChallengeData>(jsonText);

                challengeName.text = challengeData.challengeName;
                instructionDesc.text = challengeData.instructionDesc;
                questions = challengeData.quiz.questions;

                ShowQuestion();
                quizPanel.SetActive(true);
            }
            else
            {
                Debug.LogError("Quiz JSON file is missing!");
            }
        }

        private void ShowQuestion()
        {
            if (currentQuestionIndex < questions.Count)
            {
                Question currentQuestion = questions[currentQuestionIndex];
                questionText.text = currentQuestion.question;

                answerToggleGroup.SetAllTogglesOff(); // Ensure no toggle is selected initially

                // Assign options to toggles
                for (int i = 0; i < answerToggles.Count; i++)
                {
                    if (i < currentQuestion.options.Count)
                    {
                        answerToggles[i].gameObject.SetActive(true);
                        answerToggles[i].GetComponentInChildren<Text>().text = currentQuestion.options[i];
                        ResetTextColor(answerToggles[i]);
                    }
                    else
                    {
                        answerToggles[i].gameObject.SetActive(false); // Hide unused toggles
                    }
                }
            }
            else
            {
                Debug.LogWarning("No questions available to display!");
            }
        }

        public void SubmitAnswer()
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.button_click);
            int selectedAnswerIndex = -1;

            // Identify the selected answer
            for (int i = 0; i < answerToggles.Count; i++)
            {
                if (answerToggles[i].isOn)
                {
                    selectedAnswerIndex = i;
                    break;
                }
            }

            if (selectedAnswerIndex != -1)
            {
                EvaluateAnswer(selectedAnswerIndex);
            }
            else
            {
                Debug.Log("Please select an answer.");
            }
        }

        private void EvaluateAnswer(int selectedAnswerIndex)
        {
            Question currentQuestion = questions[currentQuestionIndex];

            if (selectedAnswerIndex == currentQuestion.correctAnswerIndex)
            {
                Debug.Log("Correct answer!");
                score += scorePerQuestion;
                HighlightText(answerToggles[selectedAnswerIndex], Color.green);
            }
            else
            {
                Debug.Log("Wrong answer!");
                HighlightText(answerToggles[selectedAnswerIndex], Color.red);
                HighlightText(answerToggles[currentQuestion.correctAnswerIndex], Color.green);
            }

            currentQuestionIndex++;

            Invoke(nameof(NextQuestion), 2f);
        }

        private void NextQuestion()
        {
            if (currentQuestionIndex < questions.Count)
            {
                ShowQuestion();
            }
            else
            {
                EndQuiz();
            }
        }

        private void HighlightText(Toggle toggle, Color color)
        {
            var textComponent = toggle.GetComponentInChildren<Text>();
            if (textComponent != null)
            {
                textComponent.color = color;
            }
        }

        private void ResetTextColor(Toggle toggle)
        {
            var textComponent = toggle.GetComponentInChildren<Text>();
            if (textComponent != null)
            {
                textComponent.color = Color.black;
            }
        }

        private void EndQuiz()
        {
            Debug.Log($"Quiz Finished. Final Score: {score}");
            quizPanel.SetActive(false);

            ChallengeManager.Instance.CompleteChallenge(score);
            Destroy(this.gameObject);
        }
    }

    [System.Serializable]
    public class Question
    {
        public string question;
        public List<string> options;
        public int correctAnswerIndex;
    }

    [System.Serializable]
    public class ChallengeData
    {
        public string challengeName;
        public string instructionDesc;
        public QuizData quiz;
    }

    [System.Serializable]
    public class QuizData
    {
        public List<Question> questions;
    }
}
