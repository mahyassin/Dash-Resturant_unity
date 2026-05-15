using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class EventsReporter
{
    
}

public struct MovmentReport: IReport
{
    public Vector2Int From;
    public Vector2Int To;
    public int ActorId;

    public MovmentReport(int id, Vector2Int from, Vector2Int to)
    {
        ActorId = id;
        From = from;
        To = to;
    }

}

public struct CarryReport: IReport
{
    public int Taker;
    public int TakerOnHand;
    public int TakenFrom;
    public int TakenOnHand;

    public CarryReport(int taker, int takerOnHand, int takenFrom, int takenOnHand)
    {
        Taker = taker;
        TakerOnHand = takerOnHand;

        TakenFrom = takenFrom;
        TakenOnHand = takenOnHand;
        
    }
}

public struct StoveInteract: IReport
{
    public int InteractableId;
    public bool IsOn;
    public StoveInteract(int id, bool isOn)
    {
        InteractableId = id;
        IsOn = isOn;
    }
}

public struct ContentChange: IReport
{
    public int ContainerId;
    public List<Icon> Icons;

    public ContentChange(int id, List<Icon> icons)
    {
        ContainerId = id;
        Icons = icons;
    }
}


public struct CuttingBoardInteract: IReport
{
    public int InteractableId;
    public int CutterId;
    public int ingredientId;
    public string CuttingGrade;

    public CuttingBoardInteract(int board, int cutter, string grade, int ingredient)
    {
        InteractableId = board;
        CutterId = cutter;
        CuttingGrade = grade;
        ingredientId = ingredient;
    }
}

public struct SpawnReport: IReport
{
    public ICarriable Spwan;
    public int SpanwnCarrierId;

    public SpawnReport(ICarriable spawn, int carrier)
    {
        Spwan = spawn;
        SpanwnCarrierId = carrier;
    }

}

public struct CookStateChange: IReport
{
    public int Progress;
    public int CookedMark;
    public int OverCookedMark;
    public int CookerId;

    public CookStateChange(int cookerId, int progress, int cookedMark, int overcookedMarck)
    {
        Progress = progress;
        CookerId = cookerId;
        CookedMark = cookedMark;
        OverCookedMark = overcookedMarck;

    }
}

public struct AddOrderReport: IReport
{
    public string Code;
    public Icon Icon;
    public int Id;

    public AddOrderReport(string code, Icon icon, int id)
    {
        Code = code;
        Icon = icon;
        Id = id;
    }
}

public struct CompelteOrderReport: IReport
{
    public int Id;

    public CompelteOrderReport(int id)
    {
        Id = id;
    }
}


public struct OrderTimerReport: IReport
{
    public int OrderId;
    public int Paitance;
    public int MaxPatiance;
    public bool IsFail;

    public OrderTimerReport(int orderId, int paitance, int maxPatiance, bool isFail)
    {
        OrderId = orderId;
        Paitance = paitance;
        MaxPatiance = maxPatiance;
        IsFail = isFail;
    }
}

public interface IReport{}