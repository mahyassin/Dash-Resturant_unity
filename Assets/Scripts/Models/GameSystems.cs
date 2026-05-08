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
                    if(actorCell.Ocuppier is not IIdentifialbe cutter) return;
                    string grade = (board.OnCarrier as Ingredient).cuttingGrade switch
                    {
                        CuttingGrade.WHOLE  => "whole",
                        CuttingGrade.BIG    => "big",
                        CuttingGrade.MEDUIM => "meduim",
                        CuttingGrade.SMALL  => "small",
                        _                   => "whole"
                    };
                    int ingredientId = IIdentifialbe.GetId(board.OnCarrier);

                    StateChanged?.Invoke(new CuttingBoardInteract(hasId.Id, cutter.Id, grade, ingredientId));
                }
            }

            if(ocuppier is OrderTable orderTable) 
            {
                if(player is not CharacterState p) return;
                if(p.OnCarrier is not Dish dish) return;

                orderTable.ReciveOrder(dish.DishContent,state.ordersState);
                dish.EmptyTheContainer();

                var dishCd = UnityEngine.Random.Range(10,20);
                state.DishsCD.Add(dishCd);

                p.Carry(null);
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

        // Debug.Log($"ids {taker}, {giver}, {takerOnHand}, {giverOnhand}");
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
        if(taker.OnCarrier is IContainer container) 
        {
            if(giver.OnCarrier is IContainer containergiver)
            {
                container.AddToContainer(containergiver.Content.ToList());
                containergiver.EmptyTheContainer();
                return;
            }
            
            container.AddToContainer(giver.OnCarrier);
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

    
}
public class TicSystem: GameSystem
{

    public event Action<GameState, int> OnTicProecess;
    public void VarifiyCooking(GameState state, int clock)
    {
        foreach(IInteractable station in state.interactables)
        {
            if(station is not ICooker cooker) 
            {
                continue;
            }
           
            cooker.Cook();
            cooker.UnConfirm();
        }
        for(int x = 0; x < state.DishsCD.Count; x++)
        {
            
            state.DishsCD[x]--;
            Debug.Log("Entered Dish Cdd Reduction dish cdd is: " + state.DishsCD[x]);
            
            if(state.DishsCD[x] > 0) continue;

            state.DishsCD.RemoveAt(x);
            int rng = UnityEngine.Random.Range(0, state.OrderMakers.Count -1);

            state.OrderMakers[rng].RetrunUncleanDish();
        }


        
        OnTicProecess?.Invoke(state, clock);
    }

  
}
public class OrderSystem: GameSystem
{


    private Menu _menu;
    public event Action<OrdersState> OnOrdersChange;
    
    public OrderSystem(Menu menu)
    {
        _menu = menu;
    }

    public void MakeOrderAfterCoolDown(GameState state)
    {
        foreach(var costumer in state.OrderMakers)
        {
            var rng = UnityEngine.Random.Range(0,_menu.CurrentList.Count);
            if (costumer.MakeOrderReady())
            {
                state.ordersState.AddOrder(new(_menu.CurrentList[rng], costumer.Id));
            }
        }
        OnOrdersChange?.Invoke(state.ordersState);
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

        if (pool != null) ingredient = pool; 
        else ingredient = _factory.CreateIngredient(type);

        if(carrier.OnCarrier is IContainer container)
        {
            container.AddToContainer(ingredient);
            return;
        }
        if(carrier.OnCarrier != null) return;

        carrier.Carry(ingredient);

        var carrierId = (carrier as IIdentifialbe).Id;


        carriableSpawned?.Invoke(new SpawnReport(ingredient, carrierid));
    }

}

public class RecycledIngrid
{
    private static Queue<Ingredient> PotatoPool = new();
    private static Queue<Ingredient> OnionPool = new();
    private static Queue<Ingredient> TomatoPool = new();

    public static Ingredient GetIngredient(IngredientType type)
    {
        return type switch
        {
            IngredientType.TOMATO => TomatoPool.Count > 0? TomatoPool.Dequeue(): null,
            IngredientType.ONION  => PotatoPool.Count > 0? TomatoPool.Dequeue(): null,
            IngredientType.POTATO => OnionPool.Count > 0? TomatoPool.Dequeue(): null,
            _ => null

        };
    }

    public static void AddToPool(IngredientType type, Ingredient ingredient)
    {
        Queue<Ingredient> pool = type switch
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