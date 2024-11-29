using UnityEngine;
using TMPro;
using Manager;
using DataPersistence;

public class UsernameValidator : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInputField;

	AudioManager audioManager;

	public void Awake()
	{
		audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
	}

	public void Start()
    {
        if (!PlayerPrefs.HasKey("PlayerName") && PlayerPrefs.HasKey("TemporaryUsername"))
        {
            string username = PlayerPrefs.GetString("TemporaryUsername");
            LootLockerManager.Instance.SetPlayerName(username, (success, playerName) =>
            {
                if (success)
                {
                    Debug.Log("Username set successfully.");
                    PlayerPrefs.SetString("PlayerName", username);
                    this.gameObject.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("Failed to set username.");
                }
            });
            this.gameObject.SetActive(false);
        }
        else if (PlayerPrefs.HasKey("PlayerName") || PlayerPrefs.HasKey("TemporaryUsername"))
        {
            this.gameObject.SetActive(false);
        }
    }

    public void OnSetUsernameButtonClicked()
    {
		audioManager.PlaySFX(audioManager.uiButton);
		if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            SetUsername();
        }
        else
        {
            SetTemporaryUsername();
        }
    }

    private void SetUsername()
    {
        string username = usernameInputField.text;
        LootLockerManager.Instance.SetPlayerName(username, (success, playerName) =>
        {
            if (success)
            {
                Debug.Log("Username set successfully.");
                PlayerPrefs.SetString("PlayerName", username);
                this.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Failed to set username.");
            }
        });
    }

    private void SetTemporaryUsername()
    {
        string temporaryUsername = usernameInputField.text;
        PlayerPrefs.SetString("TemporaryUsername", temporaryUsername);
        Debug.Log("Temporary username stored: " + temporaryUsername);
        this.gameObject.SetActive(false);
    }
}
