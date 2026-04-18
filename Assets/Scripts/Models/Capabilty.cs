using UnityEngine;

public interface IOcuppier
{
    public Vector2Int Pos {get;}

    public void ChangePos(Vector2Int v);
};

public interface IInteractable
{
    public void Interact();
}
public interface ICarriable{}
public interface ICarrier
{
    public ICarriable OnCarrier {get;}
    public void Carry(ICarriable carriable);

}
public interface IActor{}


