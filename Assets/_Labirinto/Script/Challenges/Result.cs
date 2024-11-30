using UnityEngine;
using TMPro;
using Challenge;
using LootLocker.Extension.DataTypes;
using Helper;

public class Result : MonoBehaviour
{
    [Header("Result Objects")]
    [SerializeField] private GameObject successObject; // Object to show on success
    [SerializeField] private GameObject failedObject; // Object to show on failure

    [Header("Panel Elements")]
    [SerializeField] private GameObject starPanel; // Panel containing star objects
    [SerializeField] private GameObject[] stars; // Array of star GameObjects
    [SerializeField] private TextMeshProUGUI scoreText; // Text to display the score
    [SerializeField] private GameObject okButton; // OK button to close the panel

    public void SetResult(bool isSuccess, int score)
    {
        // Show success or failed object based on result
        successObject.SetActive(isSuccess);
        failedObject.SetActive(!isSuccess);

        // Calculate star count based on the score
        int starCount = CalculateStars(score);

        // Update the star display
        UpdateStars(starCount);

        // Update score text
        scoreText.text = "Score: " + score;

        // Show the result panel
        starPanel.SetActive(true);
        okButton.SetActive(true);
    }

    private void UpdateStars(int starCount)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].SetActive(i < starCount); // Activate stars up to the star count
        }
    }

    public void CloseResultPanel()
    {
        ChallengeManager.Instance.ClearChallenge();
    }

    private int CalculateStars(int score)
    {
        if (score >= 80)
        {
            return 3; // 3 stars for score 80-100
        }
        else if (score >= 40)
        {
            return 2; // 2 stars for score 40-60
        }
        else
        {
            return 1; // 1 star for score 0-40
        }
    }
}
