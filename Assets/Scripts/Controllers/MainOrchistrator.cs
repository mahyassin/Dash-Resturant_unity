using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainOrchistrator
{

    private GameState    _gameState;
    private MapView      _mapview;
    private MapViewModel _viewModel;
    private MapSystem    _mapSystem;
    private TicSystem    _ticSystem;
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
        _mapSystem = new();
        _ticSystem = new();

        _timer.OnTimerTick += ProcessTic;



        _mapview.DisplayMap(_viewModel.DecodeState(_gameState));

        inputs.Moved             += OnPlayerMoved;
        inputs.Interacted        += OnPlayerCarrying;
        _mapSystem.MapChanged    += OnMapChanged;
        _ticSystem.OnTicProecess += OnTicProecess;

    }

    private void OnPlayerMoved(Vector2 dirction)
    {

        Vector2Int dir = new((int)dirction.x, (int)dirction.y);
        
        _mapSystem.VarifiyMovment(_gameState.PlayerState, _gameState, dir);
        _currentDiretion = dir;
    }

    private void OnPlayerCarrying()
    {
            _mapSystem.VarrifyCarrying(_gameState.PlayerState, _gameState, _currentDiretion);
    }

    private void OnMapChanged(GameState state)
    {
        _mapview.DisplayMap(_viewModel.DecodeState(state));
        _mapview.UpdateChoppingBoard(_viewModel.DecodeCuttingBoards(_gameState));
    }

    private void ProcessTic(int clock)
    {
        _ticSystem.VarifiyCooking(_gameState, clock);
    }

    private void OnTicProecess(GameState Changes, int clock)
    {
        _mapview.UpdateClock(clock);
        _mapview.UpdateStove(_viewModel.DecodeStoves(_gameState));
    }


}
