using Helper;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : SingletonMonobehaviour<ScenesManager>
{
    public override void Awake()
    {
        base.Awake();
    }

    public void LoadAuthenticationScene()
    {
         if (Application.internetReachability != NetworkReachability.NotReachable)
        {

            SceneManager.LoadScene("Authentication");
        }
        else 
        {
            Debug.Log("No internet connection");
            LoadHomeScene();
        }
    }    

    public void LoadHomeScene()
    {
        SceneManager.LoadScene("Main");
    }
	public void LoadQuater1()
	{
		SceneManager.LoadScene("Quarter 1");
	}

}
