using Codice.Client.BaseCommands;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;

public class MapController
{

    private GameState _gameState;
    private MapView _mapview;
    private MapViewModel _viewModel;
    private MapSystem _system;
    public InputReader _inputs;

    public MapController(GameState gameState, MapView mapView, InputReader inputs)
    {
        _gameState = gameState;
        _mapview = mapView;
        _inputs = inputs;
        _viewModel = new();
        _system = new();



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

}
