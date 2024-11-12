using System.Collections;
using Helper;
using LootLocker.Requests;
using UnityEngine;

namespace Manager
{
    public class AuthenticationManager : SingletonMonobehaviour<AuthenticationManager>
    {
        private bool isRequestSent = false;

        public override void Awake()
        {
            base.Awake();
        }

        public void Start()
        {
            CheckForExistingSession();
        }

        // Guest Login
        public void OnGuestLoginClicked()
        {
            StartCoroutine(MonitorInternetConnection());
        }

        private IEnumerator MonitorInternetConnection()
        {
            while (true)
            {
                if (Application.internetReachability != NetworkReachability.NotReachable && !isRequestSent)
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
                ScenesManager.Instance.LoadHomeScene();
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
                    ScenesManager.Instance.LoadHomeScene();
                }
                else
                {
                    Debug.Log("Failed to start guest session, will retry." + response.errorData);
                }
            });
        }
    }
}
