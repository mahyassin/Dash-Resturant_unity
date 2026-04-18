using System;
using System.Collections.Generic;
using Codice.Client.BaseCommands;
using UnityEngine;

public class MapSystem: GameSystem
{
  
    public event Action<GameState> MapChanged;
    public event Action<int> ClockTicked;

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

    public void  MoveOccupier(IOcuppier player, GameState state, Vector2Int targetPos)
    {
        var map = state.Map;

        map[targetPos] = new(player);
        map[player.Pos] = new(null);

        player.ChangePos(targetPos);

    }

    public void VarrifyCarrying(IOcuppier actor, GameState state, Vector2Int dir)
    {

        //TODO() varification rules 
        //  1- actor will put if he hold something and take if he doesn't have anything
        //  

        var targetPos = new Vector2Int(actor.Pos.x + dir.x, actor.Pos.y + dir.y);
        if (state.Map[targetPos].Ocuppier is not ICarrier  holder) return;
        if (actor is not ICarrier taker) return;


        Carrry(holder, taker);

        MapChanged?.Invoke(state);


    }

    public void Carrry(ICarrier holder, ICarrier taker)
    {
        taker.Carry(holder.OnCarrier);
        holder.Carry(null);
    }
}


public interface GameSystem{}