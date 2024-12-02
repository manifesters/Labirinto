using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using DataPersistence;

namespace Helper
{
    public enum GameState
    {
        SPLASH,
        AUTHENTICATION,
        HOME,
        LABIRINTO,
    }

    public class GameManager : SingletonMonobehaviour<GameManager>
    {
        public GameState CurrentState { get; private set; } // Current game state
        public string CurrentQuest { get; private set; } // Name of the current quest (if any)
        public string CurrentChallenge {get; private set;}

        public string CurrentScene => SceneManager.GetActiveScene().name; // Current scene name

        public override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }

        private void Start()
        {
            DataPersistenceManager.Instance.LoadPlayer();

            if (!DataPersistenceManager.Instance.HasPlayerData())
            {
                Debug.Log("No player data found. Creating new player data.");
                DataPersistenceManager.Instance.NewPlayerData();
                DataPersistenceManager.Instance.SavePlayer();
            }

            SetGameState(GameState.SPLASH);
            AudioManager.Instance.PLAY_SPLASH_BGM();
        }

        public void SetGameState(GameState newState)
        {
            if (CurrentState == newState) return;

            CurrentState = newState;
            Debug.Log($"Game state changed to: {newState}");

            // Trigger events or actions based on the new state
            HandleGameStateChange(newState);
        }

        private void HandleGameStateChange(GameState state)
        {
            switch (state)
            {
                case GameState.SPLASH:
                   AudioManager.Instance.PLAY_SPLASH_BGM();
                    break;
                case GameState.AUTHENTICATION:
                    // Handle authentication screen logic
                    break;
                case GameState.HOME:
                    AudioManager.Instance.PLAY_HOME_BGM();
                    break;
                case GameState.LABIRINTO:
                    AudioManager.Instance.PLAY_LABIRINTO_BGM();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public void SetCurrentQuest(string questName)
        {
            CurrentQuest = questName;
            Debug.Log($"Current quest set to: {questName}");
        }

        public void ClearCurrentQuest()
        {
            CurrentQuest = null;
            Debug.Log("Current quest cleared.");
        }

        public void SetCurrentChallenge(string challengeName)
        {
            CurrentChallenge = challengeName;
            Debug.Log($"Current quest set to: {challengeName}");
        }

        public void ClearCurrentChallenge()
        {
            CurrentChallenge = null;
            Debug.Log("Current quest cleared.");
        }

        public void LoadScene(string sceneName, GameState newState)
        {
            StartCoroutine(LoadSceneAsync(sceneName, newState));
        }

        public void LoadAuthenticationScene()
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                LoadScene("Authentication", GameState.AUTHENTICATION);
            }
            else 
            {
                Debug.Log("No internet connection");
                LoadScene("Main", GameState.HOME);
            }
        }    

        private System.Collections.IEnumerator LoadSceneAsync(string sceneName, GameState newState)
        {
            var asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            SetGameState(newState);
        }
    }
}
