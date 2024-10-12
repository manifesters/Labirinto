using Helper;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : SingletonMonobehaviour<CanvasManager>
{

    public Canvas splashCanvas;
    public Canvas authenticationCanvas;
    
    private void Start() {
        ShowSplash();
    }

    public void ShowSplash()
    {
        splashCanvas.gameObject.SetActive(true);
        authenticationCanvas.gameObject.SetActive(false);
    }

    public void ShowAuthentication()
    {
        splashCanvas.gameObject.SetActive(false);
        authenticationCanvas.gameObject.SetActive(true);
    }

    public void ShowHome()
    {
        splashCanvas.gameObject.SetActive(false);
        authenticationCanvas.gameObject.SetActive(false);
    }

}
