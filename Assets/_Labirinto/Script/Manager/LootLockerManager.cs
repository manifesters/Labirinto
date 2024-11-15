using System;
using System.Collections;
using UnityEngine;
using LootLocker.Requests;
using Helper;

namespace Manager
{
    public class LootLockerManager : SingletonMonobehaviour<LootLockerManager>
    {
        private bool isRequestSent = false;
        public bool IsSessionActive { get; private set; } = false;

        public override void Awake()
        {
            base.Awake();
            LootLockerSDKManager.Init("prod_df1ed3f648df470a908a77b7880a487c", "1.0", "seb87cie");
        }

        // Check for existing session
        public bool HasExistingSession()
        {
            return PlayerPrefs.HasKey("Player UID");
            
        }

        // Start guest session
        public void StartGuestSession(Action<bool> callback)
        {
            StartCoroutine(MonitorInternetConnection(() =>
            {
                LootLockerSDKManager.StartGuestSession(SystemInfo.deviceUniqueIdentifier, (response) =>
                {
                    if (response.success)
                    {
                        Debug.Log("Guest session started successfully!");
                        IsSessionActive = true;
                        PlayerPrefs.SetString("Player UID", response.public_uid);
                        PlayerPrefs.Save();
                        callback?.Invoke(true);
                    }
                    else
                    {
                        Debug.LogWarning("Failed to start LootLocker session: " + response.errorData);
                        callback?.Invoke(false);
                    }
                });
            }));
        }

        private IEnumerator MonitorInternetConnection(Action onInternetAvailable)
        {
            while (!isRequestSent)
            {
                if (Application.internetReachability != NetworkReachability.NotReachable)
                {
                    isRequestSent = true;
                    onInternetAvailable.Invoke();
                }
                yield return new WaitForSeconds(1f);
            }
        }

        public void SetPlayerName(string username, Action<bool, string> callback)
        {
            if (!IsSessionActive)
            {
                Debug.LogWarning("Cannot set player name because session is not active.");
                callback?.Invoke(false, "Session not active");
                return;
            }

            LootLockerSDKManager.SetPlayerName(username, (response) =>
            {
                if (response.success)
                {
                    Debug.Log("Player name set successfully.");
                    callback?.Invoke(true, response.name);
                }
                else
                {
                    Debug.LogWarning("Failed to set player name: " + response.errorData);
                    callback?.Invoke(false, response.errorData.ToString());
                }
            });
        }
    }
}
