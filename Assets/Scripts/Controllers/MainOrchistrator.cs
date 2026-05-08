using System.Collections.Generic;
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

        _mapSystem.GenerateIngrid     += _spawnSystem.SpawnIngredient;

        _spawnSystem.carriableSpawned += OnStateChanged;
        _mapSystem.StateChanged       += OnStateChanged;
        _ticSystem.ReportChange       += OnStateChanged;
        _orderSystem.StateChanged     += OnStateChanged;

        _viewManager.FocusCamera(_factoryCtx.ViewsRigistry.GetOnTile(gameState.PlayerState.Id) as CharacterView);

    }

    
    private void OnTest()
    {
        // Debug.Log("tested");
        // _viewManager.ViewCutting(_registry.GetOnTile(_gameState.PlayerState.Id) as CharacterView);
    }

    private void OnPlayerMoved(Vector2 dirction)
    {
        int x = dirction.x > 0.85f? 1: dirction.x < -0.85? -1: 0;
        int y = dirction.y > 0.85f? 1: dirction.y < -0.85? -1: 0;
        

        Vector2Int dir = new(x, y);
        
        _mapSystem.VarifiyMovment(_gameState.PlayerState, _gameState, dir);
        _currentDiretion = dir;
    }

    private void OnPlayerCarrying()
    {
        _mapSystem.VarrifyCarrying(_gameState.PlayerState, _gameState, _currentDiretion);
    }

    private void OnStateChanged(IReport report)
    {
        var registry = _factoryCtx.ViewsRigistry;

        switch (report)
        {
            case MovmentReport r:

                _viewManager.ViewMovment(registry.GetCharacter(r.ActorId), r.From, r.To);  

            break;

            case CarryReport r:

                ViewCarry(registry, r);
                
            break;

            case StoveInteract r:

                StoveContext ctx = new(r.IsOn);
                _viewManager.ViewStation(registry.GetStation(r.InteractableId), ctx);

            break;

            case CuttingBoardInteract r:

                ViewCutting(registry, r);

            break;

            case SpawnReport r: 

                ViewSpawn(registry, r);
                
            break;


            case ContentChange r:

                _viewManager.ViewContainerContent(registry.GetOnTile(r.ContainerId), r.Icons);

            break;

            case CookStateChange r:

                _viewManager.ViewCookingProgress(registry.GetStation(r.CookerId), r.Progress, r.CookedMark, r.OverCookedMark);

            break;

            case PendingOrdersReport r:

                _viewManager.ViewPendingOrders(r.Orders);
            break;
        }
    }

    private void ViewSpawn(ViewsRigistry registry, SpawnReport r)
    {
        var fromPool = registry.GetFromPool(r.Spwan as IIdentifialbe);
        if (fromPool == null)
        {
            var factory = _factoryCtx.ViewFactory;
            int id = IIdentifialbe.GetId(r.Spwan);

            var view = factory.CreateCarriable(r.Spwan, registry.GetStation(r.SpanwnCarrierId).Anchor.transform);
            registry.AddView(id, view);
            return;
        }

        _viewManager.SpawnFromPool(fromPool as CarriabaleView, registry.GetOnTile(r.SpanwnCarrierId));

    }

    private void ViewCutting(ViewsRigistry registry, CuttingBoardInteract r)
    {
        var actor   = registry.GetCharacter(r.CutterId);
        var board   = registry.GetStation(r.InteractableId);
        var onBoard = registry.GetCarriable(r.ingredientId);


        _viewManager.ViewCutting(actor, onBoard, r.CuttingGrade);
        _viewManager.ViewStation(board, null);
            
    }
    private void ViewCarry(ViewsRigistry registry, CarryReport r)
    {
        var taker   = registry.GetOnTile(r.Taker);
        var giver   = registry.GetOnTile(r.TakenFrom);
        var onTaker = registry.GetOnTile(r.TakerOnHand);
        var onGiver = registry.GetOnTile(r.TakenOnHand);

        _viewManager.ViewCarry(taker, giver, onTaker, onGiver);

        var pool = _viewManager.Pool;
        if (pool.Count > 0)
        {
            foreach (var view in pool)
            {
                registry.AddToPool(view.Type, view);
            }
        }
    }

    private void ProcessTic(int clock)
    {
        _ticSystem.VarifiyCooking(_gameState, clock);
        _orderSystem.MakeOrderAfterCoolDown(_gameState);
    }

    

}
