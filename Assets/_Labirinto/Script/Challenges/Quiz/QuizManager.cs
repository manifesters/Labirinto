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
        [Header("Quiz")]
        [SerializeField] private GameObject quizPanel;
        [SerializeField] private TextMeshProUGUI questionText;
        [SerializeField] private List<Toggle> answerToggles;
        [SerializeField] private ToggleGroup answerToggleGroup;
        [SerializeField] private TextMeshProUGUI challengeName;

        private List<Question> questions;
        private int currentQuestionIndex = 0;
        private readonly int scorePerQuestion = 10;

        void Start()
        {
            LoadQuizFromChallengeManager();
        }

        public void LoadQuizFromChallengeManager()
        {
            // Get the quizJson from the ChallengeManager
            TextAsset quizJson = ChallengeManager.Instance.dataJson;

            if (quizJson != null)
            {
                LoadQuiz(quizJson);
            }
            else
            {
                Debug.LogError("No quiz JSON found in ChallengeManager!");
            }
        }

        public void LoadQuiz(TextAsset quizJson)
        {
            if (quizJson != null)
            {
                string jsonText = quizJson.text;
                ChallengeData challengeData = JsonConvert.DeserializeObject<ChallengeData>(jsonText);

                challengeName.text = challengeData.challengeName;
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

                answerToggleGroup.SetAllTogglesOff(); // Make sure no toggle is selected initially

                // Display answer options in toggles
                for (int i = 0; i < answerToggles.Count; i++)
                {
                    answerToggles[i].GetComponentInChildren<Text>().text = currentQuestion.options[i];
                }
            }
        }

        public void SubmitAnswer()
        {
            // Check which toggle is selected
            int selectedAnswerIndex = -1;

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
                // Check if the selected answer is correct
                Question currentQuestion = questions[currentQuestionIndex];
                if (selectedAnswerIndex == currentQuestion.correctAnswerIndex)
                {
                    // Handle correct answer (e.g., show feedback, increase score)
                    PointManager.Instance.AddScore(scorePerQuestion); 
                    Debug.Log("Correct!");
                }
                else
                {
                    // Handle wrong answer
                    Debug.Log("Wrong!");
                }

                currentQuestionIndex++;
                if (currentQuestionIndex < questions.Count)
                {
                    ShowQuestion();
                }
                else
                {
                    EndQuiz();
                }
            }
            else
            {
                // Handle case where no answer was selected
                Debug.Log("Please select an answer.");
            }
        }

        private void EndQuiz()
        {
            quizPanel.SetActive(false);
            Destroy(quizPanel);
            Debug.Log("Quiz Finished");
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
        public QuizData quiz;
    }

    [System.Serializable]
    public class QuizData
    {
        public List<Question> questions;
    }
}
