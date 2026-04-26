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
    private OrderSystem  _orderSystem;
    private InputReader  _inputs;
    private Timer        _timer;
    private Vector2Int   _currentDiretion;

    public MainOrchistrator(GameState gameState, MapView mapView, InputReader inputs, Timer timer)
    {
        _gameState = gameState;
        _mapview   = mapView;
        _inputs    = inputs;
        _timer     = timer;

        Menu menu = new(new List<string>()
        {
            Recipes.PotatoSuop,
            Recipes.TomatoSuop, 
            Recipes.TomatoWithOnion,
        });

        _viewModel   = new();
        _mapSystem   = new();
        _ticSystem   = new();
        _orderSystem = new(menu);

        _timer.OnTimerTick += ProcessTic;


        _mapview.DisplayMap(_viewModel.DecodeState(_gameState), _viewModel.ContainersUiState);

        inputs.Moved             += OnPlayerMoved;
        inputs.Interacted        += OnPlayerCarrying;
        _mapSystem.MapChanged    += OnMapChanged;
        _ticSystem.OnTicProecess += OnTicProecess;
        _orderSystem.OnOrdersChange += OnOrdersChange;

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
        _mapview.DisplayMap(_viewModel.DecodeState(state), _viewModel.ContainersUiState);
        _mapview.UpdateChoppingBoard(_viewModel.DecodeCuttingBoards(_gameState));
    }

    private void ProcessTic(int clock)
    {
        _ticSystem.VarifiyCooking(_gameState, clock);
        _orderSystem.MakeOrderAfterCoolDown(_gameState);
    }

    private void OnTicProecess(GameState Changes, int clock)
    {
        _mapview.UpdateClock(clock);
        _mapview.UpdateStove(_viewModel.DecodeStoves(_gameState));
    }

    public void OnOrdersChange(OrdersState state)
    {
        _viewModel.UpdateOrderState(state);
        _mapview.DisplayOrders(_viewModel.OrdersUiState);
    }


}
