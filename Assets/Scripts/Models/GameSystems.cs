using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class MapSystem: GameSystem
{
  
    public event Action<List<CellState>> MapChanged;
    public event Action<IngredientType, int> GenerateIngrid;
    public event Action<IReport> StateChanged;

    public void VarifiyMovment(IOcuppier player, GameState state, Vector2Int dir)
    {
        var map = state.Map;
        if (!map.ContainsKey(player.Pos)) return;

        var targetPos = new Vector2Int(player.Pos.x + dir.x, player.Pos.y + dir.y);

        CellState targetCell = map[targetPos];
        CellState actorCell = map[player.Pos];

        if (!map.ContainsKey(targetPos)) return;
        if(map[targetPos].IsWalkable)
        {
            MoveOccupier(player, state, targetPos);
            StateChanged?.Invoke(new MovmentReport((player as CharacterState).Id, actorCell.Pos, targetCell.Pos));


        } else
        {
            var ocuppier = targetCell.Ocuppier;

            if( ocuppier is IInteractable interactable)
            {
                if(interactable is not IIdentifialbe hasId) return;

                interactable.Interact();

                bool isOn = false;
                if (interactable is Stove stove)
                { 
                    isOn = stove.IsOn();
                    StateChanged?.Invoke(new StoveInteract(hasId.Id, isOn));
                }
                
                if(interactable is CuttingBoard board)
                {
                    var cutterId = IIdentifialbe.GetId(player);

                    if(board.OnCarrier is not Ingredient i) return;

                    string grade = i.cuttingGrade switch
                    {
                        CuttingGrade.WHOLE  => "whole",
                        CuttingGrade.BIG    => "big",
                        CuttingGrade.MEDUIM => "meduim",
                        CuttingGrade.SMALL  => "small",
                        _                   => "whole"
                    };
                    int ingredientId = IIdentifialbe.GetId(board.OnCarrier);

                    StateChanged?.Invoke(new CuttingBoardInteract(hasId.Id, cutterId, grade, ingredientId));
                }
            }

            if(ocuppier is OrderTable orderTable) 
            {
                if(player is not ICarrier p) return;
                if(p.OnCarrier is not Dish dish) return;

                var order = orderTable.ReciveOrder(dish.DishContent,state.ordersState);
                dish.EmptyTheContainer();

                if(order == null)
                {
                    // no pending order match the recieved order;
                    Debug.Log("fail to recieve order");
                }

                var dishCd = UnityEngine.Random.Range(10,20);
                state.DishsCD.Add(dishCd);


                int tableId = IIdentifialbe.GetId(ocuppier);
                int playerid = IIdentifialbe.GetId(player);


                p.Carry(null);
                StateChanged?.Invoke(new CarryReport(tableId, -1, playerid, -1));
                StateChanged?.Invoke(new CompelteOrderReport(order.Id));

            }

        }
        
        MapChanged?.Invoke(new(){targetCell, actorCell});
    }
    public void VarrifyCarrying(IOcuppier actor, GameState state, Vector2Int dir)
    {


        var targetPos = new Vector2Int(actor.Pos.x + dir.x, actor.Pos.y + dir.y);

        if (state.Map[targetPos].Ocuppier is not ICarrier  holder) return;
        if (actor is not ICarrier actr) return;

        if(holder is not IIdentifialbe holderWId) return;
        if(actor  is not IIdentifialbe actorWId ) return;

        int takerOnHand = -1;
        int giverOnhand = -1;


        if (actr.OnCarrier == null)
        {
            Take(giver: holder, taker: actr);
            
        }
        else
        {
            Take(giver: actr, taker: holder);
        }

        if(holder.OnCarrier is  IIdentifialbe holderOnhandWId) giverOnhand = holderOnhandWId.Id;
        if(actr.OnCarrier is  IIdentifialbe actorOnhandWithId) takerOnHand = actorOnhandWithId.Id;

        if(holder is Generator g && g.OnCarrier == null && g.InStok > 0)
        {
            int id = (g as IIdentifialbe).Id;
            GenerateIngrid?.Invoke(g.Type, id);
        }

        int taker = actorWId.Id;
        int giver = holderWId.Id;


        StateChanged?.Invoke(new CarryReport(taker,takerOnHand, giver, giverOnhand));

    }

    private void  MoveOccupier(IOcuppier player, GameState state, Vector2Int targetPos)
    {
        var map = state.Map;

        map[targetPos] = new(player, targetPos);
        map[player.Pos] = new(null, player.Pos);

        player.ChangePos(targetPos);

    }

   
    private void Take(ICarrier giver, ICarrier taker)
    {
        if(taker.OnCarrier is IContainer takerContainer) 
        {
            if(giver.OnCarrier is IContainer containergiver)
            {
                var content = containergiver.Content.ToList();

                takerContainer.AddToContainer(content);
                containergiver.EmptyTheContainer();

                ReportContentChange(takerContainer, takerContainer.Content.ToList());
                ReportContentChange(containergiver, new());
                return;
            }

            takerContainer.AddToContainer(giver.OnCarrier);

            ReportContentChange(takerContainer, takerContainer.Content.ToList());

            giver.Carry(null);
            return;
        }

        if(taker.OnCarrier != null) return;
        if(giver.OnCarrier == null) {Debug.Log("there is nothing to take"); return; }



        taker.Carry(giver.OnCarrier);

        if(taker is TrashCan can) // if the taker is a trash can just empty what the giver has
        {
            if (giver.OnCarrier is Ingredient) giver.Carry(null);
            if (giver.OnCarrier is IContainer c) c.EmptyTheContainer();
        }

        if (taker.OnCarrier == null) return; // if the taker didn't take anything then the giver still have it  so don't empty it 
        giver.Carry(null);
    }

    private void ReportContentChange(IContainer container, List<ICarriable> takercontent)
    {
        var icons = takercontent.Select(it => it switch
        {
            Ingredient i => i.Type switch
            {
                IngredientType.TOMATO => Icon.TOMATO,
                IngredientType.ONION => Icon.ONION,
                IngredientType.POTATO => Icon.POTATO,
                _ => Icon.Error,
            },

            Pot => Icon.Pot,
            _ => Icon.Error,

        }).ToList();

        ContentChange report = new(IIdentifialbe.GetId(container), icons);

        StateChanged?.Invoke(report);
    }

}
public class TicSystem: GameSystem
{

    public event Action<IReport> ReportChange;
    public event Action<bool, int> SpawnDish;
    public void ProcessTic(GameState state, int clock)
    {
        foreach(IInteractable station in state.interactables)
        {
            if(station is not ICooker cooker) 
            {
                continue;
            }
           
            cooker.Cook();
            cooker.UnConfirm();

            Pot pot = (station as ICarrier).OnCarrier as Pot;
            if(pot != null)
            {
                var content = pot.Content.Select(it => it as Ingredient).ToList();

                var cookingProgress = content.Sum(it => it.CookingProgress);
                var cookedMark = content.Sum(it => it.MeduimPoint);
                var overcookedMarck = content.Sum(it => it.OverCookedMark);

                if (pot.Content.Count() <= 0)  {cookingProgress = 0; cookedMark = 0;}


                CookStateChange change = new(IIdentifialbe.GetId(station),cookingProgress, cookedMark, overcookedMarck);

                ReportChange?.Invoke(change);
            }

        }

        for(int x = 0; x < state.DishsCD.Count; x++)
        {
            
            state.DishsCD[x]--;
            Debug.Log("Entered Dish Cdd Reduction dish cdd is: " + state.DishsCD[x]);
            
            if(state.DishsCD[x] > 0) continue;

            state.DishsCD.RemoveAt(x);
            int rng = UnityEngine.Random.Range(0, state.OrderMakers.Count -1);

            SpawnDish?.Invoke(false, state.OrderMakers[rng].Id);

        }
        
        var pendingOrders = state.ordersState.PendingOrders;
        for(var i = 0; i < pendingOrders.Count; i++)
        {
            Order order = pendingOrders[i];
            order.LosePatiance();

            bool isFail = order.GetState == Order.State.Fial;
            if(isFail) state.ordersState.FailOrder(order);
            

            ReportChange?.Invoke(new OrderTimerReport(order.Id, order.Pataince, order.MaxPatiance, isFail));
        }
    }

  
}
public class OrderSystem: GameSystem
{


    private Menu _menu;

    public event Action<IReport> StateChanged;
    
    public OrderSystem(Menu menu)
    {
        _menu = menu;
    }

    public void MakeOrderAfterCoolDown(GameState state)
    {
        foreach(var costumer in state.OrderMakers)
        {
            if (costumer.MakeOrderReady())
            {
                var orderstate = state.ordersState;
                var rng = UnityEngine.Random.Range(0,_menu.CurrentList.Count);
                var o = _menu.CurrentList[rng];

                var code = o.Item1;
                var icon = o.Item2;

                var order = orderstate.MakeOrder(code, icon, costumer.Id);

                orderstate.AddOrder(order);
                StateChanged?.Invoke(new AddOrderReport(code, icon, order.Id));
            }

        }
    }
}

public class SpawnSystem
{
    private Identfier _identifier;
    private EntitiesFactory _factory; 
    public event Action<IReport> carriableSpawned;

    public SpawnSystem(Identfier identfier, EntitiesFactory factory)
    {
        _identifier = identfier;
        _factory    = factory;
    }

    public void SpawnIngredient(IngredientType type, int carrierid)
    {
        var carrier = _identifier.GetEntity(carrierid) as ICarrier;
        Ingredient ingredient;
        var pool = RecycledIngrid.GetIngredient(type);

        if (pool != null) {ingredient = pool; }
        else ingredient = _factory.CreateIngredient(type);

        if(carrier.OnCarrier is IContainer container)
        {
            container.AddToContainer(ingredient);
            return;
        }
        if(carrier.OnCarrier != null) return;

        carrier.Carry(ingredient);

        carriableSpawned?.Invoke(new SpawnReport(ingredient, carrierid));
    }

    public void SpawnDish(bool isclean, int carrierId)
    {
        var carrier = _identifier.GetEntity(carrierId) as ICarrier;
        Dish dish;
        var pool = RecycledIngrid.GetDish();

        if (pool != null) {dish = pool; }
        else dish = _factory.CreateDish();

        if(carrier.OnCarrier != null) return;

        carrier.Carry(dish);
        carriableSpawned?.Invoke(new SpawnReport(dish, carrierId));
    }

}

public class RecycledIngrid
{
    private static Queue<Ingredient> PotatoPool = new();
    private static Queue<Ingredient> OnionPool = new();
    private static Queue<Ingredient> TomatoPool = new();
    private static Queue<Dish> DishesPool = new();

    public static Ingredient GetIngredient(IngredientType type)
    {
        var output = type switch
        {
            IngredientType.TOMATO => TomatoPool.Count > 0? TomatoPool.Dequeue(): null,
            IngredientType.ONION  => PotatoPool.Count > 0? PotatoPool.Dequeue(): null,
            IngredientType.POTATO => OnionPool.Count > 0?  OnionPool.Dequeue(): null,
            _ => null

        };

        if(output == null) return null;

        output.Reset();
        return output;
    }

    public static Dish GetDish()
    {
        if(DishesPool.Count <= 0) return null;

        return DishesPool.Dequeue();
    }

    public static void AddToPool(Ingredient ingredient)
    {
        Queue<Ingredient> pool = ingredient.Type switch
        {
            IngredientType.TOMATO => TomatoPool,
            IngredientType.ONION  => OnionPool,
            IngredientType.POTATO => PotatoPool,
            _ => null
        };
        if (pool == null) return;

        pool.Enqueue(ingredient);
    }
}

public interface GameSystem{}