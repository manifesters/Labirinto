using System.Collections;
using LootLocker.Requests;
using TMPro;
using UnityEngine;

public class UsernameValidator : MonoBehaviour
{
    [SerializeField] private GameObject setUsernamePanel;
    [SerializeField] private TMP_InputField usernameInputField;

    public void OnSetUsernameButtonClicked()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            SetUsername();
        }
        else
        {
            SetTemporaryUsername();
        }
    }

    public void SetThisPanelActive()
    {
        setUsernamePanel.SetActive(true);
    }

    public void SetUsername()
    {
        if (PlayerPrefs.HasKey("TemporaryUsername"))
        {
            string username = PlayerPrefs.GetString("TemporaryUsername");
            if (!string.IsNullOrEmpty(username))
            {
                LootLockerSDKManager.SetPlayerName(username, (response) =>
                {
                    if (response.success)
                    {
                        Debug.Log("Setting username is successful!");
                        ScenesManager.Instance.LoadHomeScene();
                    }
                    else
                    {
                        Debug.LogWarning("Setting username is failed!");
                    }
                });
            }
        }
        else
        {
            if (usernameInputField != null)
            {
                string username = usernameInputField.text;

                Debug.Log($"Attempting to set playerName with Name: {username}");

                // Call LootLocker to set player name
                LootLockerSDKManager.SetPlayerName(username, (response) =>
                {
                    if (response.success)
                    {
                        Debug.Log("playerName successful!");
                        ScenesManager.Instance.LoadHomeScene();
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
    }
    
    // store temporary username when offline
    public void SetTemporaryUsername()
    {
        if (usernameInputField != null)
        {
            string temporaryUsername = usernameInputField.text;
            PlayerPrefs.SetString("TemporaryUsername", temporaryUsername);
            Debug.Log("Temporary username stored: " + temporaryUsername);
            ScenesManager.Instance.LoadHomeScene();
        }
        else
        {
            Debug.LogWarning("Temporary to set temporary username!");
        }
    }
}
