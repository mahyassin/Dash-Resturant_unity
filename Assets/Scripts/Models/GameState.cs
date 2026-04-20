using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GameState
{
    public Dictionary<Vector2Int, CellState> Map;
    public int MapHieght {get;}
    public int MapWidth {get;}
    public PlayerState PlayerState;
    public List<IInteractable> interactables = new();

    public GameState(
        Dictionary<Vector2Int, CellState> map, 
        PlayerState playerState, 
        int mapWidth, 
        int mapHieght,
        List<IInteractable> stations
    ){
        Map = map;
        PlayerState = playerState;
        MapHieght = mapHieght;
        MapWidth = mapWidth;
        interactables = stations;
        Debug.Log(interactables.Count);
    }

}


