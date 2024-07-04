using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : SingletonMonobehaviour<UIManager>
{

    public Canvas splashCanvas;
    public Canvas usernameCanvas;

     public override void Awake() {
        base.Awake();
    }

    private void Start() {
        ShowSplash();
    }

    // SCENE

    
    //CANVAS

    public void ShowSplash()
    {
        splashCanvas.gameObject.SetActive(true);
        usernameCanvas.gameObject.SetActive(false);
    }

    public void ShowUsername()
    {
        splashCanvas.gameObject.SetActive(false);
        usernameCanvas.gameObject.SetActive(true);
    }

}
