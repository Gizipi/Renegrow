using UnityEngine;

public class SeasonEvents
{
    public delegate void OnSeasonChange(ESeason season);
    public event OnSeasonChange onSeasonChange;
    public void SeasonChange(ESeason season)
    {
        if (onSeasonChange != null)
            onSeasonChange(season);
    }
}
