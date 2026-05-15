using System.Collections.Generic;
using NUnit.Framework;
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

public interface IIdentifialbe
{
    public int Id {get;}
    public static int GetId(object obj)
    {
        if(obj is not IIdentifialbe hasId) return -1;
        return hasId.Id;
    }
}


public interface ICarrier
{
    public ICarriable OnCarrier {get;}
    public void Carry(ICarriable carriable);

}

public interface IContainer
{
    public IEnumerable<ICarriable> Content {get;}
    public void AddToContainer(ICarriable carriable);
    public void AddToContainer(List<ICarriable> carriable);
    public void EmptyTheContainer();
}

public interface ICooker
{
    public void Cook();
    public int GetCookingProgress();
    public void UnConfirm();
}

public interface IOrderMaker
{
    public int Id {get;}
    public bool MakeOrderReady();
    
}






