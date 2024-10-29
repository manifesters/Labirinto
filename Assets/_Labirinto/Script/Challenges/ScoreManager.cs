using DataPersistence;
using Helper;
using TMPro;
using UnityEngine;

namespace Score
{
    public class ScoreManager : MonoBehaviour, IDataPersistence
    {
        public static ScoreManager Instance { get; private set; }
        [SerializeField] private TextMeshProUGUI scoreText;
        private int score;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Start()
        {
            UpdateScoreUI();
        }

        public void AddScore(int points)
        {
            score += points;
            UpdateScoreUI();
        }

        private void UpdateScoreUI()
        {
            if (scoreText == null)
            {
                scoreText = GameObject.Find("Score_Text")?.GetComponent<TextMeshProUGUI>();
            }

            if (scoreText != null)
            {
                scoreText.text = score.ToString();
            }
            else
            {
                Debug.LogWarning("ScoreText object not found in the current scene!");
            }
        }

        public int GetCurrentScore()
        {
            return score;
        }

        public void LoadData(GameData data)
        {
            this.score = data.playerScore;
            UpdateScoreUI();
        }

        public void SaveData(GameData data)
        {
            data.playerScore = score;
        }
    }   
}