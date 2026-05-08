using System.Collections;
using System.Collections.Generic;
using Codice.Client.Common.Authentication;
using UnityEngine;

public class MainOrchistrator
{

    private GameState    _gameState;
    private ViewManager  _viewManager;
    private MapSystem    _mapSystem;
    private TicSystem    _ticSystem;
    private OrderSystem  _orderSystem;
    private SpawnSystem  _spawnSystem;
    private InputReader  _inputs;
    private Timer        _timer;
    private Vector2Int   _currentDiretion;
    private FactoryContext _factoryCtx;

    public MainOrchistrator(GameState gameState, ViewManager mapView, InputReader inputs, Timer timer, FactoryContext factoryContext)
    {
        _gameState   = gameState;
        _viewManager = mapView;
        _inputs      = inputs;
        _timer       = timer;
        _factoryCtx  = factoryContext;


        Menu menu = new(new List<string>()
        {
            Recipes.PotatoSuop,
            Recipes.TomatoSuop, 
            Recipes.TomatoWithOnion,
        });

        _mapSystem   = new();
        _ticSystem   = new();
        _orderSystem = new(menu);
        _spawnSystem = new(_factoryCtx.Identfier,_factoryCtx.EntitiesFactory);


        _timer.OnTimerTick += ProcessTic;


        inputs.Moved                  += OnPlayerMoved;
        inputs.Interacted             += OnPlayerCarrying;
        inputs.Tested                 += OnTest;
        _mapSystem.MapChanged         += OnMapChanged;
        _mapSystem.StateChanged       += OnStateChanged;
        _ticSystem.OnTicProecess      += OnTicProecess;
        _orderSystem.OnOrdersChange   += OnOrdersChange;
        _mapSystem.GenerateIngrid     += _spawnSystem.SpawnIngredient;
        _spawnSystem.carriableSpawned += OnStateChanged;

        _viewManager.FocusCamera(_factoryCtx.ViewsRigistry.GetOnTile(gameState.PlayerState.Id) as CharacterView);

    }

    
    private void OnTest()
    {
        // Debug.Log("tested");
        // _viewManager.ViewCutting(_registry.GetOnTile(_gameState.PlayerState.Id) as CharacterView);
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
        var registry = _factoryCtx.ViewsRigistry;

        if(recievedReport is MovmentReport rMovment)
        {
            _viewManager.ViewMovment(registry.GetOnTile(rMovment.ActorId) as CharacterView ,rMovment.From, rMovment.To);  
        }

        if(recievedReport is CarryReport rCarry)
        {
            var taker = registry.GetOnTile(rCarry.Taker);
            var giver = registry.GetOnTile(rCarry.TakenFrom);
            var onTaker = registry.GetOnTile(rCarry.TakerOnHand) as CarriabaleView;
            var onGiver = registry.GetOnTile(rCarry.TakenOnHand) as CarriabaleView;

            _viewManager.ViewCarry(taker, giver, onTaker, onGiver);
            var pool = _viewManager.Pool;
            if (pool.Count > 0)
            {
                foreach(var view in pool)
                {
                    registry.AddToPool(view.Type, view);
                }
            }
        }

        if(recievedReport is StoveInteract rStove)
        {
            StoveContext ctx = new(rStove.IsOn);
            _viewManager.ViewStation(registry.GetOnTile(rStove.InteractableId) as StationView, ctx);
        }

        if(recievedReport is CuttingBoardInteract rBoard)
        {
            var actor = registry.GetOnTile(rBoard.CutterId) as CharacterView;
            var board = registry.GetOnTile(rBoard.InteractableId) as StationView;
            var onBoard = registry.GetOnTile(rBoard.ingredientId) as CarriabaleView;


            _viewManager.ViewCutting(actor, onBoard, rBoard.CuttingGrade);
            _viewManager.ViewStation( board, null);
        }

        if(recievedReport is SpawnReport spawn)
        {
            var fromPool = registry.GetFromPool(spawn.Spwan as IIdentifialbe);
            if (fromPool == null)
            {
                var factory = _factoryCtx.ViewFactory;
                int id = (spawn.Spwan as IIdentifialbe).Id;

                var view = factory.CreateCarriable(spawn.Spwan, (registry.GetOnTile(spawn.SpanwnCarrierId) as StationView).Anchor.transform);
                registry.AddView(id, view);
                return;
            }

            _viewManager.SpawnFromPool(fromPool as CarriabaleView, registry.GetOnTile(spawn.SpanwnCarrierId));
           
        }
    }

    private void ProcessTic(int clock)
    {
        _ticSystem.VarifiyCooking(_gameState, clock);
        _orderSystem.MakeOrderAfterCoolDown(_gameState);
    }

    private void OnTicProecess(GameState Changes, int clock)
    {
        
    }

    public void OnOrdersChange(OrdersState state)
    {
    }


}
