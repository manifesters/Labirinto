using System.Collections;
using DataPersistence;
using Helper;
using Unity.VisualScripting;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public void SaveAndExit()
    {
        StartCoroutine(SaveAndLoadHomeScene());
    }

    private IEnumerator SaveAndLoadHomeScene()
    {
        // Save game data
        DataPersistenceManager.Instance.SaveGame();

        // Optional: wait for one frame to ensure save completes if async
        yield return null;

        // Load the home scene after saving
        GameManager.Instance.LoadScene("Main", GameState.HOME);
    }
}
