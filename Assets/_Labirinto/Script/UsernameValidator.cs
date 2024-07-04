using System.Collections;
using LootLocker.Requests;
using TMPro;
using UnityEngine;

public class UsernameValidator : MonoBehaviour
{
    public TMP_InputField usernameInputField;
    
    public void AddUsername(){

        string username = usernameInputField.text;

        if (string.IsNullOrEmpty(username))
        {
            Debug.LogError("Please enter a username.");
            return;
        }

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            StartCoroutine( ValidateUsername(usernameInputField.text));
            Debug.Log("Initiated username validation: " + username);

            string storedUsername = PlayerPrefs.GetString("Username", "DefaultUsername");
            Debug.Log("Retrieved Username: " + storedUsername);
        }
        else {
            SetTemporaryUsername(username);
        }
    }

    // store temporary username when offline
    public void SetTemporaryUsername(string username)
    {
        PlayerPrefs.SetString("TemporaryUsername", username);
        Debug.Log("Temporary username stored: " + username);
    }

    // this method will validate username online
    private  IEnumerator ValidateUsername(string username)
    {
        bool gotResponse = false;
        bool isAvailable = false;

        if (PlayerPrefs.HasKey("TemporaryUsername"))
        {
            username = PlayerPrefs.GetString("TemporaryUsername");
        }

        LootLockerSDKManager.SetPlayerName(username, (response) => 
        {
            gotResponse = true;
            if (response.success)
            {
                isAvailable = true;
                Debug.Log("Username is available");
            }
            else {
                isAvailable = false;
                Debug.Log("Username is taken");
            }
        });
        
        yield return new WaitWhile(() => !gotResponse);

        if (isAvailable)
        {
            PlayerPrefs.SetString("Username", username);
            Debug.Log("Username stored: " + username);
        }
    }
}
