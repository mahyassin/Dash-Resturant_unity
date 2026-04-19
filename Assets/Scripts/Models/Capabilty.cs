using System.Collections.Generic;
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

public interface IContainer
{
    public IEnumerable<ICarriable> Carriables {get;}
    public void AddToContainer(ICarriable carriable);
    public void EmptyTheContainer();
}



