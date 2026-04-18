using System.Collections.Generic;
using UnityEngine;

public class MainOrchistrator
{

    private GameState    _gameState;
    private MapView      _mapview;
    private MapViewModel _viewModel;
    private MapSystem    _system;
    private InputReader  _inputs;
    private Timer        _timer;
    private Vector2Int   _currentDiretion;

    public MainOrchistrator(GameState gameState, MapView mapView, InputReader inputs, Timer timer)
    {
        _gameState = gameState;
        _mapview   = mapView;
        _inputs    = inputs;
        _timer     = timer;
        _viewModel = new();
        _system    = new();

        _timer.OnTimerTick += OnClockTic;



        _mapview.DisplayMap(_viewModel.DecodeState(_gameState));

        inputs.Moved       += OnPlayerMoved;
        inputs.Interacted  += OnPlayerCarrying;
        _system.MapChanged += OnMapChanged;
    }

    private void OnPlayerMoved(Vector2 dirction)
    {

        Vector2Int dir = new((int)dirction.x, (int)dirction.y);

        _system.VarifiyMovment(_gameState.PlayerState, _gameState, dir);
        _currentDiretion = dir;
    }

    private void OnPlayerCarrying()
    {
        _system.VarrifyCarrying(_gameState.PlayerState, _gameState, _currentDiretion);
    }

    private void OnMapChanged(GameState state)
    {
        _mapview.DisplayMap(_viewModel.DecodeState(state));
    }

    private void OnClockTic(int clock)
    {
        _mapview.DisplayClock(clock, _viewModel.DecodeStations(_gameState));
    }
}
