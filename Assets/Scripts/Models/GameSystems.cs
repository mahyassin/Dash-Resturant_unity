using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class MapSystem: GameSystem
{
  
    public event Action<List<CellState>> MapChanged;
    public event Action<MovmentReport> Moved;

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
            Moved(new((player as CharacterState).Id, actorCell.Pos, targetCell.Pos));


        } else
        {
            var ocuppier = targetCell.Ocuppier;

            if( ocuppier is IInteractable interactable) interactable.Interact();

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

        if (actr.OnCarrier == null)
        {
            Take(giver: holder, taker: actr);
            
        }
        else
        {
            Take(giver: actr, taker: holder);
        }

        MapChanged?.Invoke(new(){state.Map[targetPos], state.Map[actor.Pos]});
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

public class SpawningSystem
{
    private Identfier _identifier;
    private EntitiesFactory _factory; 

    public SpawningSystem(Identfier identfier, EntitiesFactory factory)
    {
        _identifier = identfier;
        _factory    = factory;
    }


}

public interface GameSystem{}