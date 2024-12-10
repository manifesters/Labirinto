using UnityEngine;
using TMPro;
using Challenge;
using Helper;
using Score;

public class Result : MonoBehaviour
{
    [Header("Result Objects")]
    [SerializeField] private GameObject successObject;
    [SerializeField] private GameObject failedObject;

    [Header("Panel Elements")]
    [SerializeField] private GameObject starPanel;
    [SerializeField] private GameObject[] stars;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject okButton;
    [SerializeField] private GameObject retryButton;

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
        scoreText.text = score.ToString();

        // Show the result panel
        starPanel.SetActive(true);
        okButton.SetActive(isSuccess);
        retryButton.SetActive(!isSuccess);

        if (isSuccess)
        {
            PointManager.Instance.AddScore(score);
        }
    }

    private void UpdateStars(int starCount)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].SetActive(i < starCount); 
        }
    }

    public void RetryButtonClick()
    {
        ChallengeManager.Instance.RetryChallenge();
        Destroy(this.gameObject);
    }

    public void OKButtonClick()
    {
        GameEventsManager.Instance.challengeEvents.CompleteChallenge();
        ChallengeManager.Instance.ClearChallenge();
        Destroy(this.gameObject);
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
        else if (score == 0)
        {
            return 0; // 0 stars for a score of exactly 0
        }
        else
        {
            return 1; // 1 star for score 1-39
        }
    }
}
