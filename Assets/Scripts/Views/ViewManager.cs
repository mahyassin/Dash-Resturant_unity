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
    [SerializeField] private Transform PoolRoot;
    private readonly List<ITileView> _pool = new();
    public IReadOnlyList<ITileView> Pool => _pool.AsReadOnly();

    

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

     
    public void ViewCutting(CharacterView character, CarriabaleView view, string grade)
    {
        character.PlayCuttingAnimation();
        view.ViewGrade(grade);
    }

    public void ViewCarry(ITileView actor, ITileView holder, CarriabaleView onActor, CarriabaleView onGiver)
    {
        if(actor == null || holder == null) return;

        if(actor.Anchor.childCount > 0)
        {
            var child = actor.Anchor.GetChild(0).GetComponent<CarriabaleView>();
            child.transform.SetParent(PoolRoot);
            _pool.Add(child);

            child.gameObject.SetActive(false);
        }

        if(holder.Anchor.childCount > 0)
        {
            var child = holder.Anchor.GetChild(0).GetComponent<CarriabaleView>();
            child.transform.SetParent(PoolRoot);
            _pool.Add(child);

            child.gameObject.SetActive(false);
        }

        if (onActor != null)
        {

            onActor.transform.SetParent(actor.Anchor.transform);
            onActor.transform.localPosition = Vector3.zero;
            onActor.gameObject.SetActive(true);


        }

        if (onGiver != null)
        {
            onGiver.transform.SetParent(holder.Anchor.transform);
            onGiver.transform.localPosition = Vector3.zero;
            onGiver.gameObject.SetActive(true);
            
        }
    }

    public void ViewStation(StationView station, StoveContext? ctx)
    {
       station.Interact(ctx);
    }

    public void SpawnFromPool(CarriabaleView view, ITileView parent)
    {
        view.transform.SetParent(parent.Anchor);
        view.transform.localPosition = Vector3.zero;
        view.gameObject.SetActive(true);
    }


    public void FocusCamera(IViewable target)
    {
        cameraView.FollowTarget(target);
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
    public Type Type {get;}
}

public enum Type
{
    Fail,
    Container,
    Dish,
    Tomato,
    Potato,
    Onion,
}





