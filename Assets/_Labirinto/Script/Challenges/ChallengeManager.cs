using Helper;
using Panel;
using UnityEngine;

namespace Challenge
{
    public enum ChallengeState
    {
        Inactive,
        Ongoing,
        Completed,
        Failed,
    }

    public class ChallengeManager : SingletonMonobehaviour<ChallengeManager>
    {
        public ChallengeState CurrentState { get; private set; }
        public TextAsset CurrentChallengeJson { get; private set; }

        public void SetCurrentChallengeJson(TextAsset challengeJson)
        {
            if (challengeJson != null)
            {
                CurrentChallengeJson = challengeJson;
                Debug.Log("Challenge JSON set successfully.");
            }
            else
            {
                Debug.LogError("Challenge JSON is null!");
            }
        }

        private void SetState(ChallengeState newState)
        {
            CurrentState = newState;
            HandleStateChange(newState);
        }

        private void HandleStateChange(ChallengeState newState)
        {
            switch (newState)
            {
                case ChallengeState.Inactive:
                    Debug.Log("ChallengeManager: No active challenge.");
                    // Reset any challenge-related data here
                    ClearChallenge();
                    break;

                case ChallengeState.Ongoing:
                    Debug.Log("ChallengeManager: Challenge started. Prepare gameplay.");
                    // Initialize gameplay elements for the challenge
                    break;

                case ChallengeState.Completed:
                    Debug.Log("ChallengeManager: Challenge completed!");
                    // Reward player, save progress, notify GameManager
                    break;

                case ChallengeState.Failed:
                    Debug.Log("ChallengeManager: Challenge failed. Awaiting retry.");
                    // Show retry prompt or reset challenge data
                    break;
            }
        }

        // starting a challenge
        public void StartChallenge(string challengePanel)
        {
            if (CurrentState == ChallengeState.Inactive)
            {
                SetState(ChallengeState.Ongoing);
                Debug.Log("ChallengeManager: Challenge has started.");
                // Activate challeng UI based on what type of challenge
                ShowChallengePanel(challengePanel, PanelShowBehaviour.KEEP_PREVIOUS);
            }
            else
            {
                Debug.LogWarning("Cannot start a new challenge while one is active.");
            }
        }

        public void CompleteChallenge(int score)
        {
            if (CurrentState == ChallengeState.Ongoing)
            {
                // Show the result panel
                ShowResultPanel("Panel_ResultWindow", PanelShowBehaviour.KEEP_PREVIOUS);

                // Determine success or failure
                bool isSuccess = score >= 80;
                
                // Log result
                if (isSuccess)
                {
                    Debug.Log($"Challenge completed with score: {score}");
                    SetState(ChallengeState.Completed);
                    GameEventsManager.Instance.challengeEvents.CompleteChallenge();
                }
                else
                {
                    Debug.Log($"Challenge failed with score: {score}");
                    SetState(ChallengeState.Failed);
                }

                // Get the Result component and pass the score
                Result resultComponent = FindObjectOfType<Result>();
                if (resultComponent != null)
                {
                    resultComponent.SetResult(isSuccess, score);
                }
                else
                {
                    Debug.LogError("Result component not found!");
                }
            }
            else
            {
                Debug.LogWarning("ChallengeManager: No active challenge to complete.");
            }
        }

        public void RetryChallenge()
        {
            if (CurrentState == ChallengeState.Failed)
            {
                Debug.Log("ChallengeManager: Retrying challenge...");
                SetState(ChallengeState.Ongoing);
                // Reset challenge-related data
                ClearChallenge(); // Clear previous data if necessary
                // Reload challenge logic or UI
            }
            else
            {
                Debug.LogWarning("ChallengeManager: Retry is only allowed after a failed challenge.");
            }
        }

        public void ClearChallenge()
        {
            CurrentChallengeJson = null;
            CurrentState = ChallengeState.Inactive;
        }

        //  ------------ Showing a panel in object pool -----------------------
        private void ShowChallengePanel(string panelID, PanelShowBehaviour behaviour = PanelShowBehaviour.KEEP_PREVIOUS) 
        {
            Debug.Log($"Starting the {panelID} challenge...");
            PanelManager.Instance.ShowPanel(panelID, behaviour);
        }

        private void ShowResultPanel(string panelID, PanelShowBehaviour behaviour = PanelShowBehaviour.KEEP_PREVIOUS) 
        {
            Debug.Log("Showing result panel");
            PanelManager.Instance.ShowPanel(panelID, behaviour);
        }
    }
}
