using System.Collections;
using LootLocker.Requests;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace Name
{
    public class AuthenticationManager : SingletonMonobehaviour<AuthenticationManager>
    {
        private bool isGuestLoginClicked = false;
        private bool isRequestSent = false;

        public override void Awake()
        {
            dontDestroyOnLoad = true;
            base.Awake();
        }

        public void Start()
        {
            // will start monitoring internet connection at the start of the game
            StartCoroutine(MonitorInternetConnection());
            Debug.Log("Started monitoring internet connection");
        }

        // Guest Login
        public void OnGuestLoginClicked()
        {
            isGuestLoginClicked = true;
        }

        private IEnumerator MonitorInternetConnection()
        {
            while (true)
            {
                if (Application.internetReachability != NetworkReachability.NotReachable && isGuestLoginClicked && !isRequestSent)
                {
                    SendGuestLoginRequest();
                }
                yield return new WaitForSeconds(1f);
            }
        }

        public static void CheckForExistingSession()
        {
            // check for LootLockerGuestPlayerID stored in playerprefs
            string lootLockerGuestSession = PlayerPrefs.GetString("LootLockerGuestPlayerID", string.Empty);
            if (!string.IsNullOrEmpty(lootLockerGuestSession))
            {
                Debug.Log("Existing session found. Proceeding to Home.");
                ProceedToHome();
            }
        }

        public void SendGuestLoginRequest()
        {
            LootLockerSDKManager.StartGuestSession(SystemInfo.deviceUniqueIdentifier, (response) =>
            {
                if (response.success)
                {
                    Debug.Log("Guest session started successfully!");
                    isRequestSent = true;
                    PlayerPrefs.SetString("Player UID", response.public_uid);
                    PlayerPrefs.Save();
                }
                else
                {
                    Debug.Log("Failed to start guest session, will retry.");
                }
            });
        }
        
        public static void SetUsername()
        {
            PanelInstanceModel lastPanel = PanelManager.Instance.GetLastPanel();

            if (lastPanel != null)
            {
                Debug.Log($"Last panel ID: {lastPanel.PanelID}");
                GameObject panelInstance = lastPanel.PanelInstance; // Get the GameObject from pool

                TMP_InputField inputFieldUsername = panelInstance.transform.Find("InputField_Username")?.GetComponent<TMP_InputField>();
            
                if (inputFieldUsername != null)
                {
                    string playerName = inputFieldUsername.text;

                    Debug.Log($"Attempting to set playerName with Name: {playerName}");

                    // Call LootLocker to set player name
                    LootLockerSDKManager.SetPlayerName(playerName, (response) =>
                    {
                        if (response.success)
                        {
                            Debug.Log("playerName successful!");
                            ProceedToHome();
                        }
                        else
                        {
                            Debug.LogWarning("playerName failed");
                        }
                    });
                }
                else
                {
                    Debug.LogWarning("TMP_InputFields not found in the last panel instance.");
                }
            }
            else
            {
                Debug.LogWarning("No panels are currently showing.");
            }
        }

        public static void ProceedToHome()
        {
            SceneManager.LoadScene("Main");
        }
    }
}
