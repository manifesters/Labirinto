using UnityEngine;
using Manager;
using Helper;

public class AuthenticationSceneController : MonoBehaviour
{
	public void OnStartGameClicked()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.button_click);
        if (LootLockerManager.Instance.HasExistingSession())
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                GameManager.Instance.LoadScene("Main", GameState.HOME);
            }
            Debug.Log("Existing session found, loading Main Scene.");
            StartGuestSession();
        }
        else
        {
            Debug.Log("No existing session found, loading Authentication Scene.");
            GameManager.Instance.LoadAuthenticationScene();
        }
    }

    public void StartGuestSession()
    {
        LootLockerManager.Instance.StartGuestSession((success) =>
        {
            if (success)
            {
                Debug.Log("Guest session started, loading Main Scene.");
                GameManager.Instance.LoadScene("Main", GameState.HOME);
            }
            else
            {
                Debug.LogWarning("Failed to start guest session.");
            }
        });
    }
}
