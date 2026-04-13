using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SeasonEvents;
using static ProgressBar;
using System.Threading.Tasks;

public class Match
{
    private EMatchState _state = EMatchState.Idle;
    private Board _board;
    private SeasonData _seasonData;
    private ESeason _currentSeason = ESeason.Winter;
    private List<IMatchBehaviour> _behaviour = new();
    private AudioBank _audioBank;
    private ProgressBar _progressBar;
    private CoreData _coreData;

    public Match(Board board, SeasonData seasonData, AudioBank audioBank, ProgressBar progressBar, CoreData coreData)
    {
        _board = board;
        _seasonData = seasonData;
        _audioBank = audioBank;
        _progressBar = progressBar;
        _coreData = coreData;
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
        if (_state != EMatchState.Playing)
            return;

        foreach (IMatchBehaviour behaviour in _behaviour)
        {
            if (behaviour.ActiveStates.Contains(_state))
            {
                behaviour.Update(deltaTime);
            }
        }
    }

    public virtual async Task StartMatch()
    {
        if (_state == EMatchState.Playing)
        {
            return;
        }

        Debug.Log("Match: StartMatch");
        _state = EMatchState.Playing;
        _progressBar.onProgressCompleted += async () => await OnProgressCompleted();
        _progressBar.Activate(_board);
        _board.ReplaceAllSlots(BoardGeneration.GenerateSlots(_coreData, _seasonData));
        await Task.Delay(100);
        _seasonData.Events.onSeasonChange += OnSeasonChange;
        _seasonData.Events.SeasonChange(_currentSeason);
    }

    private async Task OnProgressCompleted()
    {
        await Task.Delay(1000);
        EndMatch();
    }

    private void EndMatch()
    {
        _progressBar.onProgressCompleted -= async () => await OnProgressCompleted();
        _state = EMatchState.GameOver;
        _seasonData.Events.onSeasonChange -= OnSeasonChange;
        _progressBar.Deactivate();
        StartMatch();
    }

    private void OnSeasonChange(ESeason season)
    {
        _audioBank.StopAudio(_currentSeason.ToString());
        _currentSeason = season;
        _audioBank.PlayAudio(_currentSeason.ToString(), true);
    }
}
