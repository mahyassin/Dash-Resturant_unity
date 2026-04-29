using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class CharacterState : IOcuppier, ICarrier, IIdentifialbe
{
    private Vector2Int _pos;
    public Vector2Int Pos => _pos;
    public int Id {get;}

    public ICarriable OnCarrier => _carriable;



    private ICarriable _carriable;

    public CharacterState(Vector2Int pos, int id)
    {
        _pos = pos;
        Id = id;
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
