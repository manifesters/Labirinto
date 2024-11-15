using UnityEngine;
using Manager;

public class AuthenticationSceneController : MonoBehaviour
{
    public void OnStartGameClicked()
    {
        if (LootLockerManager.Instance.HasExistingSession())
        {
            Debug.Log("Existing session found, loading Main Scene.");
            StartGuestSession();
        }
        else
        {
            Debug.Log("No existing session found, loading Authentication Scene.");
            ScenesManager.Instance.LoadAuthenticationScene();
        }
    }

    public void StartGuestSession()
    {
        LootLockerManager.Instance.StartGuestSession((success) =>
        {
            if (success)
            {
                Debug.Log("Guest session started, loading Main Scene.");
                ScenesManager.Instance.LoadHomeScene();
            }
            else
            {
                Debug.LogWarning("Failed to start guest session.");
            }
        });
    }
}
