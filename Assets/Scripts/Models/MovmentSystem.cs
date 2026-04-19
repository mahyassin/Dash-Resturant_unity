using System;
using UnityEngine;

public class MapSystem: GameSystem
{
  
    public event Action<GameState> MapChanged;

    public void VarifiyMovment(IOcuppier player, GameState state, Vector2Int dir)
    {
        var map = state.Map;
        if (!map.ContainsKey(player.Pos)) return;

        var targetPos = new Vector2Int(player.Pos.x + dir.x, player.Pos.y + dir.y);

        if (!map.ContainsKey(targetPos)) return;
        if(map[targetPos].IsWalkable)
        {
            MoveOccupier(player, state, targetPos);

        } else
        {
            if(map[targetPos].Ocuppier is IInteractable interactable)

            interactable.Interact();
            
        }
        
        MapChanged?.Invoke(state);

    }
    public void VarrifyCarrying(IOcuppier actor, GameState state, Vector2Int dir)
    {

        var targetPos = new Vector2Int(actor.Pos.x + dir.x, actor.Pos.y + dir.y);
        if (state.Map[targetPos].Ocuppier is not ICarrier  holder) return;
        if (actor is not ICarrier taker) return;

        if (taker.OnCarrier == null)
        {
            Take(holder: holder, taker: taker);
            
        }
        else
        {
            Take(holder: taker, taker: holder);
        }

        MapChanged?.Invoke(state);
    }

    private void  MoveOccupier(IOcuppier player, GameState state, Vector2Int targetPos)
    {
        var map = state.Map;

        map[targetPos] = new(player);
        map[player.Pos] = new(null);

        player.ChangePos(targetPos);

    }

   
    private void Take(ICarrier holder, ICarrier taker)
    {
        if(taker.OnCarrier is IContainer container) 
        {
            container.AddToContainer(holder.OnCarrier);
            holder.Carry(null);
            return;
        }

        if(taker.OnCarrier != null) return;

        if(holder.OnCarrier == null) {Debug.Log("there is nothing to take"); return; }

        taker.Carry(holder.OnCarrier);

        if (taker.OnCarrier == null) return; 
        holder.Carry(null);
    }

    
}



public class TicSystem: GameSystem
{

    public void VarifiyCooking()
    {
        
    }
}
public interface GameSystem{}