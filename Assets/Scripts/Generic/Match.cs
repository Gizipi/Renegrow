using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SeasonEvents;

public class Match
{
    private EMatchState _state = EMatchState.Idle;
    private Board _board;
    private SeasonEvents _events;
    public SeasonEvents events
    {
        get
        {
            return _events;
        }
    }
    private ESeason _currentSeason = ESeason.Winter;
    private List<IMatchBehaviour> _behaviour = new();
    private AudioBank _audioBank;

    public Match(Board board, SeasonEvents events, AudioBank audioBank)
    {
        _board = board;
        _events = events;
        _audioBank = audioBank;
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
        _events.onSeasonChange += OnSeasonChange;
        _events.SeasonChange(_currentSeason);
    }

    private void OnSeasonChange(ESeason season)
    {
        _audioBank.StopAudio(_currentSeason.ToString());
        _currentSeason++;
        if (_currentSeason > ESeason.Winter)
        {
            _currentSeason = ESeason.Spring;
        }
        _state = EMatchState.Playing;
        _audioBank.PlayAudio(_currentSeason.ToString(), true);
    }
}
