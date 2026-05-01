using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System;



public class ViewManager : MonoBehaviour
{
    [SerializeField] private Tilemap _kitchenGrid;
    [SerializeField] private TileBase Stove;
    [SerializeField] private TileBase Shelf;

    [SerializeField] private CamerView cameraView;

    private Dictionary<Vector2Int, CellView> _cells;

  
   
    public Vector3 ToWorldPos(Vector2Int pos) => _kitchenGrid.GetCellCenterWorld((Vector3Int)pos);
    public void InitializeCells(Dictionary<Vector2Int, CellView> cells)
    {
        _cells = cells;
    }

    public void ViewMovment(CharacterView character, Vector2Int from, Vector2Int to)
    {
        if(Math.Abs(to.x - from.x) > 0) character.PlayHorizontalDashAnimation();
        if(to.y - from.y > 0) character.PlayUpDashAnimation();
        if(to.y - from.y < 0) character.PlayDownDashAnimation();

        character.MoveCharacter(ToWorldPos(from), ToWorldPos(to));
    }

    public void ViewCarry(ITileView actor, ITileView holder, CarriabaleView onActor, CarriabaleView onGiver)
    {
        if(actor == null || holder == null) return;

        if (onActor != null)
        {
            onActor.transform.SetParent(actor.Anchor.transform);
            onActor.transform.localPosition = Vector3.zero;
        }

        if (onGiver != null)
        {
            onGiver.transform.SetParent(holder.Anchor.transform);
            onGiver.transform.localPosition = Vector3.zero;

        }
    }

    public void ViewStationInteraction(StationView station, bool isOn)
    {
        station.Interact(isOn);
    }

    public void FocusCamera(IViewable target)
    {
        cameraView.FollowTarget(target);
    }

    public void UpdateClock(int clock)
    {

        // _clock = $"\nClock: {new string('.', clock)}";
        DisplayProgress();
    }

    public void UpdateStove((string, int)[] progress)
    {
        // _stoveProgress = "";
        foreach((string, int) value in progress)
        {
            // _stoveProgress += $"\n{value.Item1} " + new string('.', value.Item2);
        }
        DisplayProgress();
    }

    public void UpdateChoppingBoard((string, int)[] progress)
    {
        // _chopingProgress = "";

        foreach((string, int) value in progress)
        {
            // _chopingProgress += $"\n{value.Item1}: " + new string('.', value.Item2);
        }
        DisplayProgress();
    }

    public void DisplayProgress()
    {
        // progress.text = _clock + _stoveProgress + _chopingProgress;
    }

    public void DisplayOrders(string orders)
    {
        // OrdersUI.text = orders;
    }
}

public struct CellView
{
    public ITileView MainTile {get;}
    public Vector2Int Pos {get;}

    public CellView(ITileView icon, Vector2Int pos)
    {
        MainTile = icon;
        Pos = pos;
    }
}

public interface ITileView
{
    public Transform Anchor {get;}
}



