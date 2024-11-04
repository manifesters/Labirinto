using System;

public class MedalEvents
{
    public event Action<int> onMedalGained;
    public void MedalGained(int medal) 
    {
        if (onMedalGained != null) 
        {
            onMedalGained(medal);
        }
    }

    public event Action<int> onMedalChange;
    public void GoldChange(int gold) 
    {
        if (onMedalChange != null) 
        {
            onMedalChange(gold);
        }
    }
}