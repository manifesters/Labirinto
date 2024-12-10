using System.Collections;
using System.Collections.Generic;
using Challenge;
using UnityEngine;

public class ChallengeTrigger : MonoBehaviour
{
    [Header("Challenge JSON")]
    [SerializeField] public TextAsset challengeDataJSON;

    [Header("Challenge Details")]
    [SerializeField] private string challengePanel;
    [SerializeField] private Sprite challengeImage;

    public void EnterChallenge()
    {
        // Start the challenge
        ChallengeManager.Instance.StartChallenge(challengeDataJSON, challengePanel, challengeImage);
        Debug.Log("Starting challenge with data.");
    }
}
