using UnityEngine;

public readonly struct CellState
{
    public IOcuppier Ocuppier {get;}
    public Vector2Int Pos {get;}
   
    public bool IsWalkable => Ocuppier == null;

    public CellState(IOcuppier ocuppier, Vector2Int pos)
    {
        Ocuppier = ocuppier;
        Pos = pos;
    }

}