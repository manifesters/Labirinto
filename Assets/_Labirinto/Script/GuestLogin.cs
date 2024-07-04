using System.Collections;
using LootLocker.Requests;
using UnityEngine;

public class GuestLogin : MonoBehaviour
{

    public void Login()
    {
        StartCoroutine(GuestLoginRoutine());
    }

    private IEnumerator GuestLoginRoutine()
    {
        bool gotResponse = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                gotResponse = true;
                UIManager.Instance.ShowUsername();
            }
            else
            {
                gotResponse = true;
                 Debug.LogError("Guest login failed: " + response.errorData);
            }
        });
        yield return new WaitWhile(() => gotResponse == false);
    }


    public bool isLoggedIn;
    public void SimpleGuestLogin()
    {
        LootLockerSDKManager.StartGuestSession((response) => {if(response.success) isLoggedIn = true; });
        UIManager.Instance.ShowUsername();
    }
}
