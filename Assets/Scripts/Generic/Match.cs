using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Match
{
    private EMatchState _state = EMatchState.Idle;
    private Board _board;
    private MatchEvents _events;
    public MatchEvents events
    {
        get
        {
            return _events;
        }
    }
    private ESeason _currentSeason = ESeason.Winter;
    private List<IMatchBehaviour> _behaviour = new();

    public Match(Board board, MatchEvents events)
    {
        _board = board;
        _events = events;
    }

    public virtual void Enable()
    {
        StartMatch();
    }

    public virtual void Disable()
    {
    }

    public virtual void AddBehaviour(IMatchBehaviour behaviour)
    {
        if (_behaviour.Contains(behaviour))
            return;
        _behaviour.Add(behaviour);
    }

    public virtual void Update(float deltaTime)
    {
        foreach (IMatchBehaviour behaviour in _behaviour)
        {
            if (behaviour.ActiveStates.Contains(_state))
            {
                behaviour.Update(deltaTime);
            }
        }
    }

    public virtual void StartMatch()
    {
        changeToNextSeason();
    }

    private void changeToNextSeason()
    {
        _currentSeason++;
        if (_currentSeason > ESeason.Winter)
        {
            _currentSeason = ESeason.Spring;
        }
        _events.SeasonChange(_currentSeason);
        _state = EMatchState.Playing;
    }
}
