using System;
using UnityEngine;
using Helper;

public class GameEventsManager : SingletonMonobehaviour<GameEventsManager>
{
    public InputEvents inputEvents;
    public PlayerEvents playerEvents;
    public MiscEvents miscEvents;
    public QuestEvents questEvents;
    public StoryEvents storyEvents;

    public override void Awake()
    {
        base.Awake();
        
        if (Instance == this)
        {
            // Initialize all events
            inputEvents = new InputEvents();
            playerEvents = new PlayerEvents();
            miscEvents = new MiscEvents();
            questEvents = new QuestEvents();
            storyEvents = new StoryEvents();
        }
    }
}
