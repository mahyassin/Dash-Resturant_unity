using Codice.Client.BaseCommands;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;

public class MainOrchistrator
{

    private GameState _gameState;
    private MapView _mapview;
    private MapViewModel _viewModel;
    private MapSystem _system;
    public InputReader _inputs;
    private Timer _timer;

    public MainOrchistrator(GameState gameState, MapView mapView, InputReader inputs, Timer timer)
    {
        _gameState = gameState;
        _mapview = mapView;
        _inputs = inputs;
        _timer = timer;
        _viewModel = new();
        _system = new();

        _timer.OnTimerTick += OnClockTic;



        _mapview.DisplayMap(_viewModel.DecodeState(_gameState));

        inputs.Moved += OnPlayerMoved;
        _system.MapChanged += OnMapChanged;
    }

    private void OnPlayerMoved(Vector2 dirction)
    {
        Vector2Int dir = new((int)dirction.x, (int)dirction.y) ;

        _system.MoveOccupier(_gameState.PlayerState, _gameState, dir);
    }

    private void OnMapChanged(GameState state)
    {
        _mapview.DisplayMap(_viewModel.DecodeState(state));
    }

    private void OnClockTic(int clock)
    {
        _mapview.DisplayClock(clock);
    }

}
