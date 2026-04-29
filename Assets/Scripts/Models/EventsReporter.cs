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

public interface IReport{}