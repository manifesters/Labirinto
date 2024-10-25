using UnityEngine;

public class ChallengeManager : MonoBehaviour
{
    public static ChallengeManager Instance { get; private set; }
    public TextAsset dataJson { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Function to set the quiz JSON when NPC is clicked
    public void SetJson(TextAsset json)
    {
        dataJson = json;
    }

    // Function to clear quiz data if needed
    public void ClearJson()
    {
        dataJson = null;
    }
}
