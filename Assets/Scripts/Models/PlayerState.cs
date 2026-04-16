using UnityEngine;

public class PlayerState : IOcuppier
{
    public ICarriable onPlayer;
    private Vector2Int _pos;
    public Vector2Int Pos => _pos;


    public PlayerState(Vector2Int pos)
    {
        _pos = pos;
    }

    public void ChangePos(Vector2Int pos) => _pos = pos;

}

public class Wall : IOcuppier
{
    private Vector2Int _pos;
    public Vector2Int Pos => _pos;


    public void ChangePos(Vector2Int pos) => _pos = pos;

}
