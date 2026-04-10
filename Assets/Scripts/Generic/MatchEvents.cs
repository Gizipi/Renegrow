using UnityEngine;

public class MatchEvents
{
    public delegate void OnMatchStart();
    public event OnMatchStart onMatchStart;
    public void MatchStart()
    {
        if (onMatchStart != null)
            onMatchStart();
    }
    
    public delegate void OnSeasonChange(ESeason season);
    public event OnSeasonChange onSeasonChange;
    public void SeasonChange(ESeason season)
    {
        if (onSeasonChange != null)
            onSeasonChange(season);
    }
}
