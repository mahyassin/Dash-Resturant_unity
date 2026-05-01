using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainOrchistrator
{

    private GameState    _gameState;
    private ViewManager  _viewManager;
    private MapSystem    _mapSystem;
    private TicSystem    _ticSystem;
    private OrderSystem  _orderSystem;
    private InputReader  _inputs;
    private Timer        _timer;
    private Vector2Int   _currentDiretion;
    private ViewsRigistry _registry;
    private Identfier     _identifier;

    public MainOrchistrator(GameState gameState, ViewManager mapView, InputReader inputs, Timer timer, ViewsRigistry characterRigistry)
    {
        _gameState   = gameState;
        _viewManager = mapView;
        _inputs      = inputs;
        _timer       = timer;
        _registry    = characterRigistry;


        Menu menu = new(new List<string>()
        {
            Recipes.PotatoSuop,
            Recipes.TomatoSuop, 
            Recipes.TomatoWithOnion,
        });

        _mapSystem   = new();
        _ticSystem   = new();
        _orderSystem = new(menu);

        _timer.OnTimerTick += ProcessTic;

        inputs.Moved                += OnPlayerMoved;
        inputs.Interacted           += OnPlayerCarrying;
        _mapSystem.MapChanged       += OnMapChanged;
        _mapSystem.StateChanged     += OnStateChanged;
        _ticSystem.OnTicProecess    += OnTicProecess;
        _orderSystem.OnOrdersChange += OnOrdersChange;

        _viewManager.FocusCamera(_registry.GetOnTile(gameState.PlayerState.Id) as CharacterView);

    }

    private void OnPlayerMoved(Vector2 dirction)
    {

        Vector2Int dir = new((int)dirction.x, (int)dirction.y);
        
        _mapSystem.VarifiyMovment(_gameState.PlayerState, _gameState, dir);
        _currentDiretion = dir;
    }

    private void OnPlayerCarrying()
    {
        // Debug.Log("on player carrying");
        _mapSystem.VarrifyCarrying(_gameState.PlayerState, _gameState, _currentDiretion);
    }

    private void OnMapChanged(List<CellState> cells)
    {
        
    }

    private void OnStateChanged(IReport recievedReport)
    {
        if(recievedReport is MovmentReport rMovment)
        {
            _viewManager.ViewMovment(_registry.GetOnTile(rMovment.ActorId) as CharacterView ,rMovment.From, rMovment.To);  
        }

        if(recievedReport is CarryReport rCarry)
        {
                // Debug.Log(" it is a Carry Event");
            var taker = _registry.GetOnTile(rCarry.Taker);
            var giver = _registry.GetOnTile(rCarry.TakenFrom);
            var onTaker = _registry.GetOnTile(rCarry.TakerOnHand) as CarriabaleView;
            var onGiver = _registry.GetOnTile(rCarry.TakenOnHand) as CarriabaleView;

            // Debug.Log($"ontaker is null{onTaker == null}, ongiver is null {onGiver == null}");

            _viewManager.ViewCarry(taker, giver, onTaker, onGiver);
        }
        if(recievedReport is InteractReport rInteraction)
        {

            _viewManager.ViewStationInteraction(_registry.GetOnTile(rInteraction.InteractableId) as StationView, rInteraction.IsOn);
        }
    }

    private void ProcessTic(int clock)
    {
        _ticSystem.VarifiyCooking(_gameState, clock);
        _orderSystem.MakeOrderAfterCoolDown(_gameState);
    }

    private void OnTicProecess(GameState Changes, int clock)
    {
        _viewManager.UpdateClock(clock);
    }

    public void OnOrdersChange(OrdersState state)
    {
    }


}
