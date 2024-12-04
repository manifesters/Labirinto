using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class StoryManager : MonoBehaviour
{
    [Header("Timeline")]
    [SerializeField] private PlayableDirector playableDirector;

    private TimelineAsset timelineAsset;

    void Start()
    {
        if (playableDirector == null)
        {
            Debug.LogError("PlayableDirector is not assigned.");
            return;
        }

        // Get the TimelineAsset
        timelineAsset = playableDirector.playableAsset as TimelineAsset;
        if (timelineAsset == null)
        {
            Debug.LogError("PlayableDirector does not have a valid TimelineAsset assigned.");
        }
    }

  public void SkipCurrentTrack()
{
    if (timelineAsset == null)
    {
        Debug.LogWarning("No timeline is loaded in the PlayableDirector.");
        return;
    }

    double currentTime = playableDirector.time;
    Debug.Log($"Current PlayableDirector time: {currentTime}");

    // Variable to track the next clip to skip to
    double nextClipStartTime = double.MaxValue;
    TimelineClip nextClip = null;

    // Iterate over all tracks in the Timeline
    foreach (var track in timelineAsset.GetOutputTracks())
    {
        Debug.Log($"Processing Track: {track.name}");

        // Check for clips in each track
        foreach (var clip in track.GetClips())
        {
            // Find the next clip that starts after the current time
            if (clip.start > currentTime && clip.start < nextClipStartTime)
            {
                nextClipStartTime = clip.start;
                nextClip = clip;
            }
        }
    }

    // If a next clip is found, skip to that clip
    if (nextClip != null)
    {
        Debug.Log($"Skipping to next clip: {nextClip.displayName} at time: {nextClipStartTime}");
        playableDirector.time = nextClipStartTime;
        playableDirector.Play();
    }
    else
    {
        Debug.LogWarning("No next clip found to skip to.");
    }
}


}