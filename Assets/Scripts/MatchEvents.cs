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
}
