using DataPersistence;
using Helper;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndStory : MonoBehaviour
{
    void OnEnable()
    {
        DataPersistenceManager.Instance.SaveGame();
        GameManager.Instance.LoadScene("Labirinto", GameState.LABIRINTO);
        SceneManager.LoadSceneAsync("Labirinto");   
    }
}
