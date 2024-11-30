using System;

public class ChallengeEvents
{
    public event Action onChallengeComplete;

    public void TriggerOnChallengeComplete()
    {
        onChallengeComplete?.Invoke();
    }

    public void CompleteChallenge()
    {
        TriggerOnChallengeComplete();
    }
}
