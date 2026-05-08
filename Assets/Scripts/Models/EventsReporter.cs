using System.Numerics;
using NUnit.Framework.Constraints;
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

public interface IReport{}