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

  
    public void ShowMap()
    {
        foreach(var view in _cells.Values)
        {
            _kitchenGrid.SetTile
            (
                (Vector3Int)view.Pos,

                view.Icon switch
                {
                    TileCode.Shelf  => Shelf,
                    TileCode.Stove  => Stove,
                    TileCode.Wall   => Shelf,
                    _               => null,
                }
            );
        }
    }

    public Vector3 ToWorldPos(Vector2Int pos) => _kitchenGrid.GetCellCenterWorld((Vector3Int)pos);
    public void InitializeCells(Dictionary<Vector2Int, CellView> cells)
    {
        _cells = cells;
        ShowMap();
    }

    public void ViewMovment(CharacterView character, Vector2Int from, Vector2Int to)
    {
        if(Math.Abs(to.x - from.x) > 0) character.PlayHorizontalDashAnimation();
        if(to.y - from.y > 0) character.PlayUpDashAnimation();

        character.MoveCharacter(ToWorldPos(from), ToWorldPos(to));
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
    public TileCode Icon {get;}
    public Vector2Int Pos {get;}

    public CellView(TileCode icon, Vector2Int pos)
    {
        Icon = icon;
        Pos = pos;
    }
}



public enum TileCode
{
    Player,
    Stove,
    Shelf,
    Empty,
    Wall,
}




