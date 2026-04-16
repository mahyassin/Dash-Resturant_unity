using System;
using UnityEngine;

public class MapSystem: GameSystem
{

    public event Action<GameState> MapChanged;
    public void  MoveOccupier(IOcuppier player, GameState state, Vector2Int dir)
    {
        var map = state.Map;
        if (!map.ContainsKey(player.Pos)) return;

        var targetPos = new Vector2Int(player.Pos.x + dir.x, player.Pos.y + dir.y);

        if (!map.ContainsKey(targetPos)) return;
        if(!map[targetPos].IsWalkable)   return;

        map[targetPos] = new(player);
        map[player.Pos] = new(null);

        player.ChangePos(targetPos);

        MapChanged.Invoke(state);

    }
}

public interface GameSystem{}