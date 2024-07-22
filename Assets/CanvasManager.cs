using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : SingletonMonobehaviour<CanvasManager>
{

    public Canvas splashCanvas;
    public Canvas usernameCanvas;
    public Canvas homeCanvas;

     public override void Awake() {
        base.Awake();
    }

    private void Start() {
        ShowSplash();
    }

    public void ShowSplash()
    {
        splashCanvas.gameObject.SetActive(true);
        usernameCanvas.gameObject.SetActive(false);
        homeCanvas.gameObject.SetActive(false);
    }

    public void ShowUsername()
    {
        splashCanvas.gameObject.SetActive(false);
        usernameCanvas.gameObject.SetActive(true);
        homeCanvas.gameObject.SetActive(false);
    }

    public void ShowHome()
    {
        splashCanvas.gameObject.SetActive(false);
        usernameCanvas.gameObject.SetActive(false);
        homeCanvas.gameObject.SetActive(true);
    }

}
