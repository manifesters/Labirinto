using System;

public class StoryEvents
{
    public event Action onDialogue;

    public void TriggerOnDialogue()
    {
        onDialogue?.Invoke();
    }

    public void StoryPlayed()
    {
        TriggerOnDialogue();
    }
}
