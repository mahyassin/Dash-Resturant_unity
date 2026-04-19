using System;
using System.Collections.Generic;
using UnityEngine;

public class Stove: IInteractable, IOcuppier, ICarrier
{
    public ICarriable OnCarrier => _onStove;
    
    private Pot _onStove;
     private bool _isOn = false;
    public bool IsOn => _isOn;
     private Vector2Int _pos;
    public Vector2Int Pos => _pos;

    public Stove(Pot pot)
    {
        _onStove = pot;
    }



    public void ChangePos(Vector2Int v)
    {
        throw new NotImplementedException();
    }
      public void Interact()
    {
        _isOn = !_isOn;
    }

    public void Carry(ICarriable carriable)
    {
        if (carriable == null) _onStove = null;
        if (carriable is not Pot) return;

        _onStove = carriable as Pot;
    }
}

public class CuttingBoard: IInteractable, IOcuppier, ICarrier
{
    public ICarriable OnCarrier => _onboard;
    
    private Ingredient _onboard;

    private Vector2Int _pos;
    public Vector2Int Pos => _pos;

    public void Carry(ICarriable carriable)
    {
        if (carriable is Ingredient ingredient )
        {
            _onboard = ingredient;
        }
        if (carriable == null)
        {
            _onboard = null;
        }
    }

    public void ChangePos(Vector2Int v)
    {
        throw new NotImplementedException();
    }

    public void Interact()
    {
        if (_onboard == null) return;
        
        _onboard.CuttingProgress++;
    }
}

public class Generator: IOcuppier, ICarrier
{
    public int InStok;
    public ICarriable OnCarrier => _carriable;

    private ICarriable _carriable;

    public Generator(int instok, ICarriable carriable)
    {
        InStok = instok;
        _carriable = carriable;
    }

    private Vector2Int _pos;
    public Vector2Int Pos => _pos;
    public void ChangePos(Vector2Int v)
    {
        throw new NotImplementedException();
    }

    public void Carry(ICarriable carriable)
    {
        if(carriable == null)
        {
            InStok--;
            if (InStok > 0) return;
        }
       
        _carriable = carriable;
    }
}

public class Pot: ICarriable, IContainer
{
    private List<Ingredient> _ingredients = new();

    public IEnumerable<ICarriable> Carriables => _ingredients;

    public void AddToContainer(ICarriable carriable)
    {
        if (carriable is not Ingredient ingredient) return;
        _ingredients.Add(ingredient);
    }

    public void EmptyTheContainer()
    {
        _ingredients = new();
    }
}