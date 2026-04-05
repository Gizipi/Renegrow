using UnityEngine;

public class Match<TBoard> where TBoard : Board
{
    private LevelData _levelData;
    private TBoard _board;
    private MatchEvents _events;
    public MatchEvents events
    {
        get
        {
            return _events;
        }
    }

    public Match(LevelData levelData, TBoard board)
    {
        _levelData = levelData;
        _board = board;
    }

    public void start()
    {

    }
}
