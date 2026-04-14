using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GameState
{
    public Dictionary<Vector2Int, CellState> Map;
    public PlayerState PlayerState;

    public GameState(Dictionary<Vector2Int, CellState> map, PlayerState playerState)
    {
        Map = map;
        PlayerState = playerState;
    }

    public void UpdateCell(Vector2Int pos, int baseCell, int oncell) => Map[pos] = new(baseCell, oncell);
}


