using DataPersistence;
using Helper;
using Manager;
using TMPro;
using UnityEngine;

namespace Medal
{
    public class MedalManager : SingletonMonobehaviour<MedalManager>, IPlayerDataPersistence
    {
        [SerializeField] private TextMeshProUGUI medalText;
        private int playerMedal;

        public override void Awake()
        {
            base.Awake();
        }

        public void Start()
        {
            UpdateMedalUI();
        }

        public void AddMedal(int playerMedal)
        {
            this.playerMedal += playerMedal;
            UpdateMedalUI();
            Debug.Log("Added a score");
        }

        private void UpdateMedalUI()
        {
            if (medalText == null)
            {
                medalText = GameObject.Find("Score_Text")?.GetComponent<TextMeshProUGUI>();
            }

            if (medalText != null)
            {
                medalText.text = playerMedal.ToString();
            }
            else
            {
                Debug.LogWarning("ScoreText object not found in the current scene!");
            }
        }

        public int GetCurrentMedal()
        {
            return playerMedal;
        }

        public void LoadPlayerData(PlayerData playerData)
        {
            this.playerMedal = playerData.playerMedal;
            UpdateMedalUI();
            LootLockerManager.Instance.SubmitScore(playerMedal);
        }

        public void SavePlayerData(PlayerData playerData)
        {
            playerData.playerMedal = this.playerMedal;
        }
    }   
}