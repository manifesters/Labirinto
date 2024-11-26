using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using Challenge;
using Score;

namespace PictureChallenge
{
    public class GuessThePictureManager : MonoBehaviour
    {
        [Header("Guess the Picture")]
        [SerializeField] private GameObject challengePanel; // UI Panel containing the challenge
        [SerializeField] private Image pictureDisplay; // Image UI to show the picture
        [SerializeField] private List<Button> optionButtons; // Buttons for the options
        [SerializeField] private TextMeshProUGUI challengeName; // Challenge name text

        private List<PictureQuestion> pictureQuestions;
        private int currentQuestionIndex = 0;
        private readonly int scorePerQuestion = 10;

        void Start()
        {
            LoadChallengeFromManager();
        }

        public void LoadChallengeFromManager()
        {
            // Get the challenge JSON from ChallengeManager
            TextAsset challengeJson = ChallengeManager.Instance.dataJson;

            if (challengeJson != null)
            {
                LoadChallenge(challengeJson);
            }
            else
            {
                Debug.LogError("No challenge JSON found in ChallengeManager!");
            }
        }

        public void LoadChallenge(TextAsset challengeJson)
        {
            if (challengeJson != null)
            {
                string jsonText = challengeJson.text;
                ChallengeData challengeData = JsonConvert.DeserializeObject<ChallengeData>(jsonText);

                challengeName.text = challengeData.challengeName;
                pictureQuestions = challengeData.pictureQuiz.questions;
                ShowQuestion();
                challengePanel.SetActive(true);
            }
            else
            {
                Debug.LogError("Challenge JSON file is missing!");
            }
        }

        private void ShowQuestion()
        {
            if (currentQuestionIndex < pictureQuestions.Count)
            {
                PictureQuestion currentQuestion = pictureQuestions[currentQuestionIndex];

                // Load the picture into the Image UI
                Sprite questionImage = Resources.Load<Sprite>(currentQuestion.picturePath);
                if (questionImage != null)
                {
                    pictureDisplay.sprite = questionImage;
                }
                else
                {
                    Debug.LogError($"Failed to load image at path: {currentQuestion.picturePath}");
                }

                // Set button texts for options
                for (int i = 0; i < optionButtons.Count; i++)
                {
                    if (i < currentQuestion.options.Count)
                    {
                        optionButtons[i].gameObject.SetActive(true);
                        optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.options[i];
                    }
                    else
                    {
                        optionButtons[i].gameObject.SetActive(false);
                    }
                }

                // Add click listeners to buttons
                for (int i = 0; i < optionButtons.Count; i++)
                {
                    int index = i; // Capture the index for the closure
                    optionButtons[i].onClick.RemoveAllListeners();
                    optionButtons[i].onClick.AddListener(() => SubmitAnswer(index));
                }
            }
        }

        public void SubmitAnswer(int selectedOptionIndex)
        {
            PictureQuestion currentQuestion = pictureQuestions[currentQuestionIndex];

            if (selectedOptionIndex == currentQuestion.correctAnswerIndex)
            {
                // Handle correct answer
                PointManager.Instance.AddScore(scorePerQuestion);
                Debug.Log("Correct!");
            }
            else
            {
                // Handle wrong answer
                Debug.Log("Wrong!");
            }

            currentQuestionIndex++;
            if (currentQuestionIndex < pictureQuestions.Count)
            {
                ShowQuestion();
            }
            else
            {
                EndChallenge();
            }
        }

        private void EndChallenge()
        {
            challengePanel.SetActive(false);
            Destroy(challengePanel);
            Debug.Log("Picture Guess Challenge Finished");
        }
    }

    [System.Serializable]
    public class PictureQuestion
    {
        public string picturePath; // Path to the image file
        public List<string> options; // Options for the question
        public int correctAnswerIndex; // Index of the correct answer
    }

    [System.Serializable]
    public class ChallengeData
    {
        public string challengeName;
        public PictureQuizData pictureQuiz; // Container for picture questions
    }

    [System.Serializable]
    public class PictureQuizData
    {
        public List<PictureQuestion> questions;
    }
}
