using UnityEngine;

public class PlayerState : IOcuppier, ICarrier
{
    private Vector2Int _pos;
    public Vector2Int Pos => _pos;

    public ICarriable OnCarrier => _carriable;



    private ICarriable _carriable;

    public PlayerState(Vector2Int pos)
    {
        _pos = pos;
    }
  
    public void ChangePos(Vector2Int pos) => _pos = pos;

    public void Carry(ICarriable carriable)
    {
        _carriable = carriable;
        
    }
}

public class Wall : IOcuppier
{
    private Vector2Int _pos;
    public Vector2Int Pos => _pos;


    public void ChangePos(Vector2Int pos) => _pos = pos;

}
